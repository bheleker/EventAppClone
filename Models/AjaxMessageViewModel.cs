using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{
    public class AjaxMessageViewModel
    {
        public string message {get;set;}
        public int activityId {get;set;}
    }
}