using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{
    public class Message
    {
        [Key]
        public int MessageId {get;set;}

        [Required]
        public string MessageBody {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}
        public User Creator {get;set;} //navigator
        public int ActivityId {get;set;}
        public ActivityModel SpecificActivity {get;set;} //navigator


    }
}