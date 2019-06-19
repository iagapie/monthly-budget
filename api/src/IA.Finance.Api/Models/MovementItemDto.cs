using System;

namespace IA.Finance.Api.Models
{
    public class MovementItemDto
    {
        public long Id { get; set; }
        
        public long MovementId { get; set; }
        
        public long? OwnerId { get; set; }
        
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        
        public string Description { get; set; }
        
        public decimal Amount { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}