using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Subscription : Entity<Guid>
    {
        public int ChannelId { get; set; }
        public User Channel { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Subscription()
        {
        }

        public Subscription(int channelId, int userId) : this()
        {
            ChannelId = channelId;
            UserId = userId;
        }

    }
}
