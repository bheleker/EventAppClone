using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace EventApp.Models
{
    public class ActivityViewModel
    {
        public IEnumerable<ActivityModel> viewActivityList {get;set;}
        public ActivityModel viewActivityModel {get;set;}
        public int viewSessionId {get;set;}
        public IEnumerable<Message> viewMessageList {get;set;}
    }
}