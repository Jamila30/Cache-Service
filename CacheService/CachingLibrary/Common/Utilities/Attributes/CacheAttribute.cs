using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CachingLibrary.Common.Utilities.Attributes
{
    public class CacheAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = context.GenerateCachingKey();

            var _cacheService = context.GetCachingService<ICacheService>();
            if (_cacheService != null)
            {
                if (_cacheService.IsExist(key))
                {
                    context.Result = new ContentResult()
                    {
                        Content = _cacheService.GetCachedData<string>(key),
                        StatusCode = 200
                    };
                }
                else
                {
                    var result = await next();
                    var resposneResult = result.Result as ObjectResult;

                    var serializedForm = System.Text.Json.JsonSerializer.Serialize(resposneResult?.Value);
                    _cacheService!.SetCacheValue(key, serializedForm, DateTimeOffset.UtcNow.AddMinutes(120));
                }
            }

        }
    }
}
