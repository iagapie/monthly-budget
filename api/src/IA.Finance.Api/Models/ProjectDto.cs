using System;

namespace IA.Finance.Api.Models
{
    public class ProjectDto
    {
        public long Id { get; set; }
        
        public long OwnerId { get; set; }
        
        public string Name { get; set; }
        
        public string Currency { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}