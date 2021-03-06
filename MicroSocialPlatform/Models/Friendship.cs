using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models
{
    public class Friendship
    {
        [Key, Column(Order = 0)]
        public string Id { get; set; }
        [Key, Column(Order = 1)]
        public string Id2 { get; set; }

    }
}