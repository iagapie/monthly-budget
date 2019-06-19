using GraphQL.Types;
using IA.Finance.Api.Models;

namespace IA.Finance.Api.GraphQL.Types
{
    public class UserInputType : InputObjectGraphType<UserDto>
    {
        public UserInputType()
        {
            Name = "UserInput";

            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.UserName);
            Field(x => x.Email);
            Field(x => x.Role, true);
            Field(x => x.FirstName, true);
            Field(x => x.LastName, true);
        }
    }
}