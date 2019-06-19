using System;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using IA.Finance.Api.GraphQL.Resolvers.Movement;
using IA.Finance.Api.GraphQL.Resolvers.Project;
using IA.Finance.Api.GraphQL.Resolvers.User;
using IA.Finance.Api.GraphQL.Types;

namespace IA.Finance.Api.GraphQL
{
    public class AppQuery : ObjectGraphType<object>
    {
        public AppQuery(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }
            
            Name = "Query";

            this.AuthorizeWith("UserPolicy");

            FieldAsync<NonNullGraphType<ListGraphType<ProjectType>>>("projects", resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<ProjectsResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ProjectType>>("project", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<ProjectResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ListGraphType<MovementType>>>("movements", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "projectId"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<MovementsResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ListGraphType<UserType>>>("users", resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UsersResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            }).AuthorizeWith("AdminPolicy");
            
            FieldAsync<NonNullGraphType<UserType>>("user", arguments: new QueryArguments(
                new QueryArgument<IdGraphType> {Name = "id"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UserResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
        }
    }
}