using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [MinLength(2, ErrorMessage="Name must be between 2 and 15 characters.")]
        [MaxLength(15, ErrorMessage="Name must be between 2 and 15 characters.")]
        public string Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,50}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$", ErrorMessage="Password must be 8 characters or longer and contain one uppercase and one lowercase letter, one number and one special character.")]

        public string Password {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string MatchPassword {get;set;}
    
    }
}