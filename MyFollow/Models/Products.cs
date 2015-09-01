using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyFollow.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Poid { get; set; }

        [StringLength(140, ErrorMessage = "It is more than 140 Character")]
        public string Introduction { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }

        public List<UploadImages> UploadImages { get; set; }
         
        [ForeignKey("Poid")]
        public virtual ProductOwner ProductOwner { get; set; }
    }

    public class ProductsList
    {
        public int Id { get; set; }

        public int Poid { get; set; }

        public string Introduction { get; set; }

        public string Details { get; set; }

        public string  UploadImagesName { get; set; }
     
    }
}