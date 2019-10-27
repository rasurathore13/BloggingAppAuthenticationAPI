using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingAppAuthenticationAPI.Model
{
    public class Blogger
    {
        [Key]
        public int BloggerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BloggerFullName { get; set; }

        [Required]
        [EmailAddress]
        public string BloggerEmail { get; set; }

        [Required]
        public DateTime BloggerDOB { get; set; }
        
        public string BloggerPasswordHash { get; set; }
        
        public string BloggerSalt { get; set; }
    }
}
