using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.History.Command.Update
{
    public class UpdateHistoryResponse
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public int UserID { get; set; }

        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }
    }
}
