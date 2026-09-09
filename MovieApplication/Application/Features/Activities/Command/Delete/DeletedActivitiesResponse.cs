using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.Activities.Command.Delete
{
    public class DeletedActivitiesResponse
    {
        public Guid EntityID { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }

        public ActivitiesCategory ActivitiesCategory { get; set; }
    }
}
