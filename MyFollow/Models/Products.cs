using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFollow.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MProductId { get; set; }

        [StringLength(140, ErrorMessage = "It is more than 140 Character")]
        public string Introduction { get; set; }

        [StringLength(1000, ErrorMessage = "It is more than 1000 Character")]
        public string Details { get; set; }

        public string VideoLink { get; set; }

        [ForeignKey("MProductId")]
        public virtual MainProduct MainProduct { get; set; }

        public List<UploadImages> UploadImages { get; set; }
    }

    public class ProductsList
    {
        public int Id { get; set; }

        public int Poid { get; set; }
        public string CompanyName { get; set; }

        public int MProductId { get; set; }

        public string ProductName { get; set; }

        public string Introduction { get; set; }

        public string Details { get; set; }

        public bool Flag { get; set; }

        public string UploadImagesName { get; set; }

        public string VideoLink { get; set; }

    }
}