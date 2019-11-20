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
        public string FirstName {get;set;}
        public string LastName {get;set;}

        public string Email {get;set;}

        public string IDToken {get; set;}
    
        public List<Message> CreatedMessages {get;set;}
    }
}