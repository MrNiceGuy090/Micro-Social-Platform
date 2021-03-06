using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicroSocialPlatform.Models
{
    public class Request
    {
        [Key, Column(Order = 0)]
        public string Sent { get; set; }
        [Key, Column(Order = 1)]
        public string Received { get; set; }
    }
}