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
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>, ICacheable
    {
        private readonly ICacheService _cache;
        public CacheBehavior(ICacheService cache) => _cache = cache;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<TResponse>(request.CacheKey);
            if (cached != null) return cached;

            var response = await next();
            await _cache.SetAsync(request.CacheKey, response, request.CacheDuration);
            return response;
        }
    }
}
