using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostViewModel
    { 
            public UserProfile User { get; set; }
            public int ProviderUserProfileId { get; set; }
            public int PostId { get; set; }
            public int SubscriberUserProfileId { get; set; }
            public Subscription Subscription { get; set; }
            public int SubscriptionId { get; set; }
            public Post Post { get; set; }
            public string EstimatedReadTime
            {
                get
                {
                int wordCount = Post.Title.Split(' ').Count() + Post.Content.Split(' ').Count();
                decimal time = Math.Ceiling(wordCount / 60m);
                if (time > 1)
                {
                    return $"{time} minutes";
                }
                else
                {
                    return $"{time} minute";
                }                
                }
            }
    }
}
