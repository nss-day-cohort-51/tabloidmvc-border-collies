using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }
        public void Subscribe(Subscription subscription)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeingDateTime, EndDateTime
                             )
                        OUTPUT INSERTED.ID
                        VALUES ( @subscriberUserProfileId, @providerUserProfileId, @beginDateTime, @endDateTime
                            )";
                    cmd.Parameters.AddWithValue("@name", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@email", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@phoneNumber", subscription.BeingDateTime); 
                    cmd.Parameters.AddWithValue("@address", subscription.EndDateTime);
                   

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
