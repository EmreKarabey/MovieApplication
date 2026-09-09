using System;

namespace Application.Features.Activities.Command.Update
{
    public class UpdateActivitiesResponse
    {
        public Guid Id { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }
        public Domain.Entities.ActivitiesCategory ActivitiesCategory { get; set; }
    }
}
