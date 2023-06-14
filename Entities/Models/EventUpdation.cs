using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class EventUpdation : EventCreation
    {
        [Required]
        public long EventId { get; set; }
        [Required]
        public string? ReasonToUpdate { get; set; }
        public List<EventImages>? EventMedia { get; set; }
    }
}
