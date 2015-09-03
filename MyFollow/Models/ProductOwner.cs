using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyFollow.Models
{
    public class ProductOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string OwnerName { get; set; }

        [StringLength(50, ErrorMessage = "Company name cannot be longer than 50 characters.")]
        public string CompanyName { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Descrption name cannot be longer than 150 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofJoin { get; set; }


        public string FoundedIn { get; set; }

        [StringLength(100,ErrorMessage="Not More then 100 Character")]
        public string Street1 { get; set; }

        [StringLength(100, ErrorMessage = "Not More then 100 Character")]
        public string Street2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int Pincode { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter proper contact details.")]
        [StringLength (10)]
        [Display(Name = "Contact No")]
        public string ContactNumber { get; set; }

        public string Website { get; set; }

        public string Twitter { get; set; }

        public string Facebook { get; set; }

        public string Password { get; set; }

        public bool ApprovalFlag { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}