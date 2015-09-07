using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyFollow.Models
{
    public class MainProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Poid { get; set; }

        [Required]
        public string ProductName { get; set; }

        [ForeignKey("Poid")]
        public virtual ProductOwner ProductOwner { get; set; }

       
    }
}