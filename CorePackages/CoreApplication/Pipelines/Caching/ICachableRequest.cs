using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApplication.Pipelines.Caching
{
    public interface ICachableRequest
    {
        public string CacheKey { get; }
        public string? CacheGroupKey { get; }
        public bool ByPassCache { get; }
        public TimeSpan? SlidingExpiration { get; }
    }
}
