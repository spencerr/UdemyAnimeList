using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UdemyAnimeList.Web.Intrastructure
{
    public class CdnUrlAttribute : Attribute
    {
        
    }

    public class CdnUrlResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly string _cdnUrl;
        public CdnUrlResolver(string cdnUrl)
        {
            _cdnUrl = cdnUrl;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (member.GetCustomAttribute<CdnUrlAttribute>() is CdnUrlAttribute)
            {
                prop.ValueProvider = new CdnUrlValueProvider(member as PropertyInfo, _cdnUrl);
            }

            return prop;
        }
    }

    public class CdnUrlValueProvider : IValueProvider
    {
        private PropertyInfo _targetProperty;
        private string _cdnUrl;

        public CdnUrlValueProvider(PropertyInfo targetProperty, string cdnUrl)
        {
            _targetProperty = targetProperty;
            _cdnUrl = cdnUrl;
        }

        public object GetValue(object target)
        {
            var value = _targetProperty.GetValue(target);
            return string.IsNullOrEmpty(_cdnUrl) ? value : Path.Join(_cdnUrl, (string) value);
        }

        public void SetValue(object target, object value)
        {
            _targetProperty.SetValue(target, value);
        }
    }
}
