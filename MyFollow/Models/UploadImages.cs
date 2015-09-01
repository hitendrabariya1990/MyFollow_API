using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyFollow.Models
{
    public class UploadImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public int ProductId { get; set; }

        public string ImageName { get; set; }

        //public string Image2 { get; set; }

        //public string Image3 { get; set; }

        //public string Image4 { get; set; }

        //public string Image5 { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

    }
}