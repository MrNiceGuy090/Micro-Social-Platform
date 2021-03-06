using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicroSocialPlatform.Models
{
    public class Profile
    {
        [Key]
        public String Id { get; set; }
        public String Username { get; set; }
        public String Name { get; set; }
        public String About { get; set; }
        public String City { get; set; }
        public int Age { get; set; }
        public String Job { get; set; }
        public bool publicProfile { get; set; }
    }
}