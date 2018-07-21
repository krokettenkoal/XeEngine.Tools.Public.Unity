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
	public static class Plugins
	{
		public static IEnumerable<(Assembly, Type)> GetPlugins(string folder, Func<string, bool> assemblyFilter,
			Func<Type, bool> typeFilter)
		{
			if (string.IsNullOrWhiteSpace(folder))
			{
				folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}

			var fileNames = Directory.EnumerateFiles(folder)
				.Where(assemblyFilter)
				.Select(fileName => Path.GetFullPath(fileName));

			var plugins = new List<(Assembly, Type)>();
			foreach (var file in fileNames)
			{
				var assembly = Assembly.LoadFile(file);
				foreach (var type in assembly.GetTypes())
				{
					if (typeFilter.Invoke(type))
						plugins.Add((assembly, type));
				}
			}
			return plugins;
		}

		public static IEnumerable<(Assembly, Type)> GetModules<T>(string folder, Func<string, bool> assemblyFilter)
			where T : IModule
		{
			var strType = typeof(T).FullName;

			return GetPlugins(folder, assemblyFilter, a => a.GetInterfaces().Any(t => t.FullName == strType));
		}
	}
}
