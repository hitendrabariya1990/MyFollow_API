using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFollow.DAL;

namespace MyFollow.Models
{
    public class FollowProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Euid { get; set; }
        public int MProductId { get; set; }


        [ForeignKey("MProductId")]
        public virtual MainProduct MainProduct { get; set; }

       // [ForeignKey("Euid")]
        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
}