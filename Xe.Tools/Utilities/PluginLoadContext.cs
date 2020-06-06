using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Xe.Tools.Utilities
{
    /// <summary>
    /// Avoid to raise TypeInitializationException on trying load referencedAssembly from plugin dll.
    /// </summary>
    class PluginLoadContext : AssemblyLoadContext
    {
        public PluginLoadContext()
        {
            Resolving += NeedToResolveNotLoadedAssembly;
        }

        private Assembly NeedToResolveNotLoadedAssembly(AssemblyLoadContext loader, AssemblyName assemblyName)
        {
            var dllName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName.Name + ".dll");
            if (File.Exists(dllName))
            {
                var assembly = Assembly.LoadFrom(dllName);
                if (assembly != null && assembly.FullName == assemblyName.FullName)
                {
                    return assembly;
                }
            }
            return null;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
