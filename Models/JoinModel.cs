using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{
    public class Join{
        [Key]
        public int JoinId {get; set;}
        [Required]
        [MaxLength(15)]
        public int ActivityId {get;set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public ActivityModel activity {get;set;}
    }
}