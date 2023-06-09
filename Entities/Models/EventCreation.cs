using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class EventCreation
    {
        [Required(ErrorMessage = "Event Title is Required")]
        public string? EventTitle { get; set; }
        [Required]
        public string? EventDesc { get; set; }
        public string? Note { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string? Vanue { get; set; }
        public string? CreatedBy { get; set; }
        [Required]
        public int? MaxParticipants { get; set; }
    }

    public class EventImages
    {
        public long EventId { get; set; }
        public string? Path { get; set; }
    }
}
