using LibrarymanagementSystem.Application.Common.Caching;
using LibrarymanagementSystem.Application.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarymanagementSystem.Application.Common.Behaviors
{
    public class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService _cache;
        public CacheInvalidationBehavior(ICacheService cache) => _cache = cache;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            if (request is IInvalidateCache invalidate)
            {
                foreach (var key in invalidate.CacheKeysToInvalidate)
                    await _cache.RemoveAsync(key);
            }

            return response;
        }
    }
}
