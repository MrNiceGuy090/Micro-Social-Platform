using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicroSocialPlatform.Models
{
    public class Grouppost
    {
        [Key, Column(Order = 0)]
        public int GroupId { get; set; }
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Continutul postarii nu poate fi lasat gol")]
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}