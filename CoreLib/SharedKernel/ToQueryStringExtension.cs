using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CoreLib.SharedKernel
{
    public static class ToQueryStringExtension
    {
        public static string ToQueryString(this object data)
        {
            var result = new List<string>();
            
            var props = data.GetType().GetProperties();//.Where(p => p.GetValue(data, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(data, null);
                if (value is ICollection enumerable)
                {
                    result.AddRange(enumerable.Cast<object>().Select(v => $"{p.Name}={HttpUtility.UrlEncode(v.ToString())}"));
                }
                else
                {
                    result.Add($"{p.Name}={HttpUtility.UrlEncode(value?.ToString())}");
                }
            }

            return string.Join("&", result.ToArray());
        }
    }
}
