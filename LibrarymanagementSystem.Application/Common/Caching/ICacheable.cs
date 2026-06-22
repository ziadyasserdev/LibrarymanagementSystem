using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Common.Caching
{
    public interface ICacheable
    {
        string CacheKey { get; }
        TimeSpan CacheDuration { get; }
    }
}
