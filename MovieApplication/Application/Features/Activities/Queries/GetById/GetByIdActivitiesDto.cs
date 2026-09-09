using System;
using Domain.Entities;

namespace Application.Features.Activities.Queries.GetById
{
    public class GetByIdActivitiesDto
    {
        public Guid Id { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }
        public ActivitiesCategory ActivitiesCategory { get; set; }
    }
}
