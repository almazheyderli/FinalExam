using Microsoft.AspNetCore.Http;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public  class Team:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = null!;
        public string ?ImgUrl { get; set; }
        [NotMapped]
   
        public IFormFile? ImgFile { get; set; }
    }
}
