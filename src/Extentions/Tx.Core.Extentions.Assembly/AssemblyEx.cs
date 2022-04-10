using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tx.Core.Extensions.Assembly
{
    public static class AssemblyEx
    {
        public static string GetDirectoryPathX(this System.Reflection.Assembly assembly)
        {
            string filePath = new Uri(assembly.Location).LocalPath;
            return Path.GetDirectoryName(filePath);
        }

        public static List<Type> GetAllIEntityTypeConfigurationAssebliesByNamespaceContains(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace)) return new List<Type>();

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x =>
                    x.Namespace != null &&
                    x.Namespace.Contains(@namespace) &&
                    x.GetMethods().FirstOrDefault(m => m.Name == "Configure") != null &&
                    !x.IsInterface &&
                    !x.IsAbstract)
                .ToList();
        }

        public static List<Type> GetAllAssebliesByInterface<T>()
        {
            if (!typeof(T).IsInterface) return new List<Type>();

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }

        public static List<Type> GetAllIEntityTypeConfigurationAssebliesInterface<T>()
        {
            if (!typeof(T).IsInterface) return new List<Type>();

            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();
        }

        public static Type GetTypeOf<T>(string name)
        {
            var type = typeof(T);
            var types = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.ToLowerInvariant() == name.ToLowerInvariant() && t.IsClass && !t.IsInterface);

            if (type == null) throw new Exception("No such type");

            return types as Type;
        }

        public static IEnumerable<Type> GetClassOfType<T>()
        {
            var type = typeof(T);
            IEnumerable<Type> types = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => type.IsAssignableFrom(t) && t.IsClass);

            if (types == null) throw new Exception("No such type");

            return types;
        }

        public static List<Type> GetTypesAssignableFrom<T>(this System.Reflection.Assembly assembly) => assembly.GetTypesAssignableFrom(typeof(T));

        public static List<Type> GetTypesAssignableFrom(this System.Reflection.Assembly assembly, Type compareType)
        {
            List<Type> ret = new List<Type>();
            foreach (var type in assembly.DefinedTypes)
            {
                if (compareType.IsAssignableFrom(type) && compareType != type)
                {
                    ret.Add(type);
                }
            }
            return ret;
        }
    }

}
