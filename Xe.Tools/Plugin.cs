// MIT License
// 
// Copyright(c) 2018 Luciano (Xeeynamo) Ciccariello
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Part of this software belongs to XeEngine toolset and United Lines Studio
// and it is currently used to create commercial games by Luciano Ciccariello.
// Please do not redistribuite this code under your own name, stole it or use
// it artfully, but instead support it and its author. Thank you.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xe.Tools
{
    /// <summary>
    /// Plugin management system to load external assemblies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Plugin<T>
    {
        private static string _pathAssembly;
        private static string _dirAssembly;

        protected Type Type { get; private set; }

        public string Name { get; protected set; }

        protected Plugin(Type type)
        {
            Type = type;
            Name = type.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        protected static IEnumerable<T> GetPlugins(string folder, string[] extensions,
            Func<Type, T> funcComparison)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                if (_pathAssembly == null)
                    _pathAssembly = Assembly.GetExecutingAssembly().Location;
                if (_dirAssembly == null)
                    _dirAssembly = Path.GetDirectoryName(_pathAssembly);
                folder = _dirAssembly;
            }
            else if (!Directory.Exists(folder))
            {
                Log.Warning($"Directory ${folder} for plugins loading was not found.");
                return new T[0];
            }

            var fileNames = Directory.EnumerateFiles(folder)
                .Where(fileName => extensions.Contains(Path.GetExtension(fileName).ToLower()))
                .Select(fileName => Path.GetFullPath(fileName));

            var plugins = new List<T>();
            foreach (var file in fileNames)
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        var plugin = funcComparison.Invoke(type);
                        if (plugin != null)
                            plugins.Add(plugin);
                    }
                }
                catch (BadImageFormatException)
                {
                    Log.Warning($"File {file} is a bad format.");
                }
                catch (Exception e)
                {
                    Log.Error($"Exception on {file}: {e.Message}");
                }
            }
            return plugins;
        }
        
        protected MethodAccessException PrepareMethodNotFoundException(MethodBase method)
        {
            var name = method.Name;
            var parameters = method.GetParameters();
            var ret = (method as MethodInfo)?.ReturnType ?? null;

            var strReturn = ret != null ? $"{ret.Name} " : string.Empty;
            var strParams = string.Join(",", parameters.Select(x => $"{x.ParameterType} {x.Name}"));
            var contract = $"{strReturn}{name}({strParams})";
            return new MethodAccessException($"Method {contract} not found.");
        }
    }
}
