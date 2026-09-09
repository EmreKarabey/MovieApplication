using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.Activities.Command.Create
{
    public class CreatedActivitiesResponse
    {
        public int UserID { get; set; }
        public Guid MovieID { get; set; }

        public ActivitiesCategory ActivitiesCategory { get; set; }
    }
}
