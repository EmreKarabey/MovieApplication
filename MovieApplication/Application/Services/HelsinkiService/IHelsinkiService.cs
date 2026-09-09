using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.HelsinkiService
{
    public interface IHelsinkiService
    {
        public Task<string> Translate(string Comment);
    }
}
