using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{
    public class ActivityModel{
        [Key]
        public int ActivityId {get; set;}
        [Required]
        [MaxLength(40)]
        public string Title {get;set;}
        [Required]
        public DateTime Date {get;set;}
        [Required]
        [MinLength(10, ErrorMessage="Description must be at least ten characters long")]
        public string Description {get; set;}
        [Required]
        [RegularExpression("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$", ErrorMessage= "Please Submit a valid time in XX:XX format")]
        
        public string Time {get; set;}
        [RegularExpression("^[1-9]+[0-9]*$", ErrorMessage = "Duration must be a positive number greater than zero.")]
        public string Duration {get; set;}
        public string DurationFormat {get; set;}
        public String AMPM {get;set;}
        public string Address {get;set;}
        public int UserId {get; set;}
        public User Creator {get; set;}
        public List<Join> Joins {get; set;}
        public List<Message> AttachedMessages {get;set;}

    }
}