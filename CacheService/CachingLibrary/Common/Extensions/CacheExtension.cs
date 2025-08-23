
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace CachingLibrary.Common.Extensions
{
    public static class CacheExtension
    {
        public static T? GetCachingService<T>(this ActionExecutingContext context)
        {
            var service = context.HttpContext.RequestServices.GetService(typeof(T));
            if (service != null && service is T)
            {
                return (T)service;
            }
            else
            {
                return default(T);
            }
        }
        public static string GenerateCachingKey(this ActionExecutingContext context)
        {
            var action = ((dynamic)context.HttpContext.Request).RouteValues["action"];
            var controller = ((dynamic)context.HttpContext.Request).RouteValues["controller"] + "Controller";
            var functKey = new StringBuilder();
            var valuesData = string.Empty;
            var arguments = context.ActionArguments.Values;
            foreach (var item in arguments)
            {
                bool isCustom = item?.GetType().Namespace != "System";
                if (isCustom && item is not null)
                {
                    var dtoType = item.GetType();
                    var properties = dtoType.GetProperties();
                    valuesData = string.Join(',', properties.Select(c => c.GetValue(item)));
                }
                else
                {
                    valuesData = item?.ToString();
                }
                functKey.Append(valuesData + ",");
            }
            var namespaceOfAction = context.Controller.GetType().Namespace;
            return $"{namespaceOfAction}.{controller}.{action}({functKey.ToString().Trim(',')})";
        }
    }
}
