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

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Xe.Tools.Wpf.Dialogs
{
    public class FileDialog
    {
        public enum Behavior
        {
            Open, Save, Folder
        }
        public enum Type
        {
            Any,
            Executable,
            XeGameProject,
            XeAnimation,
            ImagePng,
        }

        private CommonFileDialog _fd;

        private Window WindowParent { get; }

        public Behavior CurrentBehavior { get; }

		public string DefaultFileName
		{
			get => _fd.DefaultFileName;
			set => _fd.DefaultFileName = value;
		}

		public string FileName => _fd.FileName;

		public IEnumerable<string> FileNames => (_fd as CommonOpenFileDialog)?.FileNames ?? new string[] { FileName };

        private FileDialog(CommonFileDialog commonFileDialog, Window wndParent, Behavior behavior)
        {
            _fd = commonFileDialog;
            WindowParent = wndParent;
            CurrentBehavior = behavior;
        }

        public bool? ShowDialog()
        {
			var result = WindowParent != null ? _fd.ShowDialog(WindowParent) : _fd.ShowDialog();

			switch (result)
            {
                case CommonFileDialogResult.Ok: return true;
                case CommonFileDialogResult.None: return false;
                case CommonFileDialogResult.Cancel: return null;
                default: return null;
            }
        }

        public static FileDialog Factory(Window wndParent, Behavior behavior, Type type = Type.Any, bool multipleSelection = false)
        {
			var filters = new List<(string, string[])>();

            if (behavior != Behavior.Folder)
            {
                switch (type)
                {
                    case Type.Any:
						filters.Add(("All files", new string[] { "*" }));
                        break;
                    case Type.Executable:
						filters.Add(("Application", new string[] { "exe" }));
                        break;
                    case Type.XeGameProject:
						filters.Add(("XeEngine project", new string[] { "proj.json" }));
                        break;
                    case Type.XeAnimation:
						filters.Add(("XeEngine 2D animation", new string[] { "anim.json" }));
                        break;
                    case Type.ImagePng:
						filters.Add(("PNG image", new string[] { "png" }));
                        break;
                    default:
                        break;
                }
            }

            return Factory(wndParent, behavior, filters, multipleSelection);
		}

		public static FileDialog Factory(Window wndParent, Behavior behavior, (string, string) filter, bool multipleSelection = false)
		{
			return Factory(wndParent, behavior, new(string, string)[] { filter }, multipleSelection);
		}

		public static FileDialog Factory(Window wndParent, Behavior behavior, IEnumerable<(string, string)> filters, bool multipleSelection = false)
		{
			return Factory(wndParent, behavior, filters.Select(x => (x.Item1, new string[] { x.Item2 })), multipleSelection);
		}

		public static FileDialog Factory(Window wndParent, Behavior behavior, IEnumerable<(string, string[])> filters, bool multipleSelection = false)
		{
			CommonFileDialog fd;
			switch (behavior)
			{
				case Behavior.Open:
					fd = new CommonOpenFileDialog()
					{
						EnsureFileExists = true,
						Multiselect = multipleSelection
					};
					break;
				case Behavior.Save:
					fd = new CommonSaveFileDialog()
					{

					};
					break;
				case Behavior.Folder:
					fd = new CommonOpenFileDialog()
					{
						IsFolderPicker = true,
						Multiselect = multipleSelection
					};
					break;
				default:
					throw new ArgumentException("Invalid parameter", nameof(behavior));
			}
			fd.AddToMostRecentlyUsedList = true;
			fd.EnsurePathExists = true;

			foreach (var filter in filters)
			{
				fd.Filters.Add(CreateFilter(filter.Item1, filter.Item2));
			}

			return new FileDialog(fd, wndParent, behavior);
		}


		private static CommonFileDialogFilter CreateFilter(string name, string[] filters)
        {
            var filter = new CommonFileDialogFilter()
            {
                DisplayName = name
            };
            foreach (var item in filters)
                filter.Extensions.Add(item);
            return filter;
        }
    }
}
