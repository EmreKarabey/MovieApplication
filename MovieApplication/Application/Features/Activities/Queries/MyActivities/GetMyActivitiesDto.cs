using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.Activities.Queries.MyActivity
{
    public class GetMyActivitiesDto
    {
        public Guid Id { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }

        public string MovieName { get; set; }
        public string ImageURL { get; set; }
        public int ActivitiesCategory { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
