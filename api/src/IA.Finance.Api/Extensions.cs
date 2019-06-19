using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Types;
using IA.Finance.Api.GraphQL.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace IA.Finance.Api
{
    public static class Extensions
    {
        public static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyInfo>> PropertyInfoCache =
            new ConcurrentDictionary<Type, ConcurrentDictionary<string, PropertyInfo>>();
        
        public static T Deserialize<T>(this Stream s)
        {
            using (var streamReader = new StreamReader(s))
            using (var jsonTextReader = new JsonTextReader(streamReader))
                return new JsonSerializer().Deserialize<T>(jsonTextReader);
        }

        public static async Task JsonResponse(this HttpContext context, object data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
        }

        public static bool IsJsonPost(this HttpRequest request) =>
            HttpMethods.IsPost(request.Method) &&
            MediaTypeHeaderValue.TryParse(request.ContentType, out var parsedValue) &&
            parsedValue.MediaType == "application/json";

        public static T Get<T>(this HttpContext context) => context.RequestServices.GetRequiredService<T>();

        public static ClaimsIdentity Identity(this ResolveFieldContext<object> context) =>
            (context.UserContext as UserContext)?.User.Identity as ClaimsIdentity;

        public static bool IsAuthenticated(this ResolveFieldContext<object> context) => context.Identity()?.IsAuthenticated ?? false;

        public static string IdentityId(this ResolveFieldContext<object> context) => context.Identity()?.IdentityId();
        
        public static string Role(this ResolveFieldContext<object> context) => context.Identity()?.Role();
        
        public static string IdentityId(this ClaimsIdentity identity) => identity?.Claims.First(c => c.Type == "id").Value;
        
        public static string Role(this ClaimsIdentity identity) => identity?.Claims.First(c => c.Type == "rol").Value;
        
        public static ConcurrentDictionary<string, PropertyInfo> GetCachedProperties(this Type type)
        {
            var props = new ConcurrentDictionary<string, PropertyInfo>();
    
            if (type != null && !PropertyInfoCache.TryGetValue(type, out props))
            {
                props = new ConcurrentDictionary<string, PropertyInfo>(type
                    .GetProperties()
                    .GroupBy(x => x.Name)
                    .Select(x => x.First())
                    .ToDictionary(x => x.Name, x => x));
                PropertyInfoCache[type] = props;
            }
    
            return props;
        }
    
        public static IEnumerable<PropertyInfo> GetCachedProps(this Type type)
        {
            return type == null ? Enumerable.Empty<PropertyInfo>() : type.GetCachedProperties().Values;
        }
    
        public static PropertyInfo GetCachedProperty(this Type type, string name)
        {
            PropertyInfo prop;
            type.GetCachedProperties().TryGetValue(name, out prop);
            return prop;
        }
    
        public static string ToPropertyName(this Expression<Func<object>> expression, bool noVarName = true)
        {
            return expression.Body.ToPropertyName(noVarName);
        }
    
        public static string ToPropertyName(this Expression expr, bool noVarName = true)
        {
            var name = string.Empty;
    
            if (expr == null) return name;
    
            switch (expr.NodeType)
            {
                case ExpressionType.Convert:
                    return ToPropertyName(((UnaryExpression)expr).Operand, noVarName);
                case ExpressionType.MemberAccess:
                {
                    var memExpr = (MemberExpression)expr;
                    name = memExpr.Member.Name;
    
                    if (memExpr.Expression.NodeType == ExpressionType.MemberAccess)
                    {
                        var memExpr2 = (MemberExpression)memExpr.Expression;
    
                        if (!(noVarName && memExpr2.Expression.NodeType == ExpressionType.Constant))
                            name = $"{ToPropertyName(memExpr.Expression, noVarName)}.{name}";
                    }

                    break;
                }
            }

            return name;
        }
    
        public static TTo Populate<TTo, TFrom>(this TTo to, TFrom from, params Expression<Func<object>>[] excludeProps)
            where TTo : class
            where TFrom : class => to.Populate(from, excludeProps.Select(x => x.ToPropertyName()).ToArray());
    
        private static TTo Populate<TTo, TFrom>(this TTo to, TFrom from, params string[] excludeProps) where TTo : class where TFrom : class
        {
            if (from == null || to == null) return to;
    
            if (excludeProps == null) excludeProps = new string[0];
    
            var entityType = to.GetType();
    
            Parallel.ForEach(from.GetType().GetCachedProps().Where(x => x.CanRead && !excludeProps.Contains(x.Name)), fromProp =>
            {
                var name = fromProp.Name;
                
                var toProp = entityType.GetCachedProperty(name);
                
                if (toProp == null || !toProp.CanWrite) return;
                
                var nullToType = Nullable.GetUnderlyingType(toProp.PropertyType);
                
                var fromType = Nullable.GetUnderlyingType(fromProp.PropertyType) ?? fromProp.PropertyType;
                var toType = nullToType ?? toProp.PropertyType;
                
                var toValue = toProp.GetValue(to);
                var fromValue = fromProp.GetValue(from);
                
                if (!typeof(IEnumerable).IsAssignableFrom(toType) &&
                    !typeof(IEnumerable).IsAssignableFrom(fromType) &&
                    typeof(string) != toType &&
                    typeof(string) != fromType &&
                    (toType.IsClass || toType.IsInterface) &&
                    (fromType.IsClass || fromType.IsInterface) &&
                    toValue != null && fromValue != null)
                {
                    toProp.SetValue(to, toValue.Populate(fromValue, excludeProps.Select(x =>
                    {
                        var index = x.IndexOf('.');
                        return index < 0 || x.Substring(0, name.Length) != name ? null : x.Substring(index + 1);
                    }).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()));
                
                    return;
                }
                
                try
                {
                    if (fromValue != null)
                    {
                        if (fromType.IsEnum && toType.IsEnum)
                            fromValue = Enum.Parse(toType, fromValue.ToString());
                        
                        if (nullToType != null)
                            fromValue = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(nullToType), fromValue);
                    }
                    
                    toProp.SetValue(to, fromValue);
                }
                catch
                {
                    // ignored
                }
            });
    
            return to;
        }
    }
}