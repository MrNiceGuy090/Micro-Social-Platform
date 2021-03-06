using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicroSocialPlatform.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Comentariul nu poate fi lasat gol")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int PostId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }

    }
}