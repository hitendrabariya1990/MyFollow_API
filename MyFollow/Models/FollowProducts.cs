using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MyFollow.DAL;

namespace MyFollow.Models
{
    public class FollowProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Euid { get; set; }
        public int ProductId { get; set; }
        public bool Flag { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Product { get; set; }

        [ForeignKey("Euid")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}