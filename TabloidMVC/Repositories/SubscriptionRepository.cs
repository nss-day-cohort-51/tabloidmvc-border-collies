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
                        INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime
                             )
                        OUTPUT INSERTED.ID
                        VALUES ( @subscriberUserProfileId, @providerUserProfileId, @beginDateTime, @endDateTime
                            )";
                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDateTime", subscription.BeginDateTime);
                    if (subscription.EndDateTime == null)
                    {
                        cmd.Parameters.AddWithValue("@endDateTime", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@endDateTime", subscription.EndDateTime);
                    }


                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void Unsubscribe(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Subscription
                            SET 
                                EndDateTime = SYSDATETIME()
                            WHERE SubscriberUserProfileId = @id AND EndDateTime IS NULL";
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Subscription GetSubscriptionByUserProfileId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        select Id, SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime
                          from (
                                select Id, SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime, row_number() over (partition by SubscriberUserProfileId order by ID desc) as rn
                                 from Subscription
                                ) as t
                          where t.rn = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Subscription subscription = new Subscription()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                                ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                                BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime"))
                            };
                            if (reader.IsDBNull(reader.GetOrdinal("EndDateTime")))
                            {
                                subscription.EndDateTime = null;
                            }
                            else
                            {
                                subscription.EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime"));
                            }
                            return subscription;
                        }
                        return null;
                    }
                }
            }
        }
    }
}
