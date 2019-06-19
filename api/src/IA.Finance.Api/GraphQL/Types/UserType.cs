using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class UserType : ObjectGraphType<UserDto>
    {
        public UserType()
        {
            Name = "User";
            
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.FirstName, true);
            Field(x => x.LastName, true);
            Field(x => x.UserName);
            Field(x => x.Email);
            Field(x => x.Role);
            Field(x => x.CreatedAt);
            Field(x => x.UpdatedAt, true);
        }
    }
}