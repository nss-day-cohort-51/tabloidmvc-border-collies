using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        public void Subscribe(Subscription subscription);
        public void Unsubscribe(int userId);
        Subscription GetSubscriptionByUserProfileId(int id);
    }
}


