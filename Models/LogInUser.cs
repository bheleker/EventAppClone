using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{

    public class LogInUser
    {
        public string Password {get; set;}
        public string Email {get; set;}
    }
}