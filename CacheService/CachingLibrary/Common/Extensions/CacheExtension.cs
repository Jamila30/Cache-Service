
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
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
            dynamic action = ((dynamic)context.HttpContext.Request).RouteValues["action"];
            dynamic controller = ((dynamic)context.HttpContext.Request).RouteValues["controller"] + "Controller";
            StringBuilder functKey = new StringBuilder();
            string? valuesData = string.Empty;
            IDictionary<string, object> arguments = context.ActionArguments;

            List<string> generatedKeyValues = arguments.Select(kvp =>
            {
                string key = kvp.Key;
                var value = kvp.Value;

                return value != null && value.GetType().Namespace != "System"
                    ? string.Join(',', value.GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(value)}"))
                    : $"{key}={value}";
            }).ToList();

            functKey.Append(string.Join(",", generatedKeyValues));

            #region Old version of funktKey generation 
                  //foreach (var item in arguments.Values)
                  //{
                  //    bool isCustom = item?.GetType().Namespace != "System";
                  //    if (isCustom && item is not null)
                  //    {
                  //        Type dtoType = item.GetType();
                  //        PropertyInfo[] properties = dtoType.GetProperties();
                  //        valuesData = string.Join(',', properties.Select(c => $"{c.Name}={c.GetValue(item)}"));
                  //    }
                  //    else
                  //    {
                  //        valuesData = item?.ToString();
                  //    }
                  //    functKey.Append(valuesData + ",");
                  //}
            #endregion

            var namespaceOfAction = context.Controller.GetType().Namespace;
            return $"{namespaceOfAction}.{controller}.{action}({functKey.ToString().Trim(',')})";
        }
    }
}
