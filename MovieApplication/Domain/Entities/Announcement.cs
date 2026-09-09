using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;

namespace Domain.Entities
{
    public class Announcement : Entity<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subtitle1 { get; set; }
        public string Subtitle2 { get; set; }
    }
}
