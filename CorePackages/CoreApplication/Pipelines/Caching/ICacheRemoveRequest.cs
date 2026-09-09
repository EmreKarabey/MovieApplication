using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApplication.Pipelines.Caching
{
    public interface ICacheRemoveRequest
    {
        public string? CacheKey { get; }
        public bool ByPassCache { get;}
        public string? CacheGroupKey { get; }
    }
}
