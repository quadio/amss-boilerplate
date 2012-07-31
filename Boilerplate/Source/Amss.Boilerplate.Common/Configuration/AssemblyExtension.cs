namespace Amss.Boilerplate.Common.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyExtension
    {
        public static string EffectiveVersion(this Assembly assembly)
        {
            return GetVersionOptions(assembly).FirstOrDefault(ver => !string.IsNullOrEmpty(ver));
        }

        private static IEnumerable<string> GetVersionOptions(Assembly assembly)
        {
            yield return assembly
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .Select(a => a.InformationalVersion).FirstOrDefault();
            yield return assembly
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                .OfType<AssemblyFileVersionAttribute>()
                .Select(a => a.Version).FirstOrDefault();
            yield return assembly
                .GetName()
                .Version
                .ToString();
        }
    }
}
