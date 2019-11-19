using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{

    public class LogUser
    {
        public User Users {get; set;}

        public LogInUser Logs {get; set;}
    }
}