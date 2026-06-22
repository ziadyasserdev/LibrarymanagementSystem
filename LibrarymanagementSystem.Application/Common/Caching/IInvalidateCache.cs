using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Common.Caching
{

    public interface IInvalidateCache
    {
        string[] CacheKeysToInvalidate { get; }
    }
}
