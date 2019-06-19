using System;
using System.Collections.Generic;

namespace IA.Finance.Api.Models
{
    public class MovementDto
    {
        public long Id { get; set; }
        
        public long? OwnerId { get; set; }
        
        public long ProjectId { get; set; }
        
        public string Name { get; set; }
        
        public decimal PlanAmount { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset? UpdatedAt { get; set; }
        
        public DirectionDto Direction { get; set; } = new DirectionDto();
        
        public List<MovementItemDto> MovementItems { get; set; } = new List<MovementItemDto>();
    }
}