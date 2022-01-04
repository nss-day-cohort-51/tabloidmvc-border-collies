using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostViewModel
    { 
            public User User { get; set; }
            public int UserId { get; set; }
            public Subscription Subscription { get; set; }
            public int SubscriptionId { get; set; }
            public Post Post { get; set; }
    }
}
