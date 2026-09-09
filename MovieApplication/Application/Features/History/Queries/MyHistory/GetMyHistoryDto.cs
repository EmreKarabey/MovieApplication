using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.History.Queries.MyHistory
{
    public class GetMyHistoryDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<string> CategoryName { get; set; }

        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }
    }
}
