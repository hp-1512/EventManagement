using Microsoft.AspNetCore.Mvc;
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
        
        [Required(ErrorMessage = "Event Description is Required")]
        [Remote("IfValidDesc", "Methods", AdditionalFields = "eventDesc", ErrorMessage = "Event Description must be of 20 characters excluding space.")]
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
    public class UpdateDeleteEventClass
    {
        public string? Email { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Vanue { get; set; }

    }
}
