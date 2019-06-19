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
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }
            
            Name = "Mutation";
            
            this.AuthorizeWith("UserPolicy");

            FieldAsync<NonNullGraphType<UserType>>("createUser", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<UserInputType>> {Name = "user"},
                new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "password"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<CreateUserResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            }).AuthorizeWith("AdminPolicy");

            FieldAsync<NonNullGraphType<UserType>>("updateUser", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<UserInputType>> {Name = "user"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UpdateUserResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<BooleanGraphType>>("changePassword", arguments: new QueryArguments(
                new QueryArgument<IdGraphType> {Name = "id"},
                new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "currentPassword"},
                new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "newPassword"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<ChangePasswordResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ProjectType>>("createProject", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<ProjectInputType>> {Name = "project"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<CreateProjectResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ProjectType>>("updateProject", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<ProjectInputType>> {Name = "project"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UpdateProjectResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<ProjectType>>("removeProject", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<RemoveProjectResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementType>>("createMovement", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<MovementInputType>> {Name = "movement"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<CreateMovementResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementType>>("updateMovement", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<MovementInputType>> {Name = "movement"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UpdateMovementResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementType>>("removeMovement", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<RemoveMovementResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementItemType>>("createMovementItem", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<MovementItemInputType>> {Name = "movementItem"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<CreateMovementItemResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementItemType>>("updateMovementItem", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<MovementItemInputType>> {Name = "movementItem"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<UpdateMovementItemResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
            
            FieldAsync<NonNullGraphType<MovementItemType>>("removeMovementItem", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"}
            ), resolve: async context =>
            {
                try
                {
                    return await resolver.Resolve<RemoveMovementItemResolver>().Resolve(context);
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
        }
    }
}