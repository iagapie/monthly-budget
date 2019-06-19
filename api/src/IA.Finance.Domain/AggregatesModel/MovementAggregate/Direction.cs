using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.MovementAggregate
{
    public class Direction : Enumeration
    {
        public static Direction In = new Direction(1, nameof(In).ToLowerInvariant());
        
        public static Direction Out = new Direction(-1, nameof(Out).ToLowerInvariant());
        
        public Direction(int id, string name) : base(id, name)
        {
        }
    }
}