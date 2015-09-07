using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFollow.Models
{

    public class UploadImages
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImageName { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

    }

}