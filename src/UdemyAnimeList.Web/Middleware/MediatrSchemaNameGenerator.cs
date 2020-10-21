using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyAnimeList.Web.Middleware
{
    public class MediatrSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            if (type.IsEnum || !type.Namespace.Contains("UdemyAnimeList.Web"))
            {
                return type.Name;
            }

            var controllerName = type.Namespace.Split(".").Last();
            var className = type.Name;
            var parentClassName = type.FullName.Replace($"{type.Namespace}.", "").Replace(className, "").Split("+").FirstOrDefault();

            return $"{controllerName}{parentClassName}{className}";
        }
    }

}
