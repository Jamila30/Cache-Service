using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CachingLibrary.Common.Utilities.Attributes
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RemoveStoredCacheAttribute : ActionFilterAttribute
    {
        public string RemovePatternKey { get; set; } = null!;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _cacheService = context.GetCachingService<ICacheService>();

            if (RemovePatternKey != null && _cacheService != null)
            {
                _cacheService.RemoveByPattern(RemovePatternKey);
                await next();
            }

        }
    }
}
