using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePersistence.Repositories
{
    public class Entity<TEntityID>:IEntityTimeStamps
    {
        public TEntityID EntityID { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Entity()
        {
            EntityID = default;
        }

        public Entity(TEntityID entityID)
        {
            EntityID = entityID;
        }
    }
}
