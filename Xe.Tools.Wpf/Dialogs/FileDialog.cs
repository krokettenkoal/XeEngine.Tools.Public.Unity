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
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Xe.Tools.Wpf.Dialogs
{
    public class FileDialogFilter
    {
        public string Name { get; }
        public string[] Patterns { get; }

        private FileDialogFilter(string name, IEnumerable<string> extensions)
        {
            Name = name;
            Patterns = extensions.ToArray();
        }

        public override string ToString() => $"{Name}|{string.Join(";", Patterns)}";

        public static FileDialogFilter ByAllFiles(string name = "All files") => ByPatterns(name, "*");
        public static FileDialogFilter ByExtensions(string name, params string[] extensions) => ByExtensions(name, extensions.AsEnumerable());
        public static FileDialogFilter ByExtensions(string name, IEnumerable<string> extensions) =>
            ByPatterns(name, extensions.Select(x => $"*.{x}"));
        public static FileDialogFilter ByPatterns(string name, params string[] patterns) => ByExtensions(name, patterns.AsEnumerable());
        public static FileDialogFilter ByPatterns(string name, IEnumerable<string> patterns) => new FileDialogFilter(name, patterns);
    }

    public static class FileDialogFilterComposer
    {
        public static List<FileDialogFilter> Compose() => new List<FileDialogFilter>();
        public static List<FileDialogFilter> AddAllFiles(this List<FileDialogFilter> filters, string name = "All files")
        {
            filters.Add(FileDialogFilter.ByAllFiles(name));
            return filters;
        }
        public static List<FileDialogFilter> AddExtensions(this List<FileDialogFilter> filters, string name, params string[] extensions)
        {
            filters.Add(FileDialogFilter.ByExtensions(name, extensions));
            return filters;
        }
        public static List<FileDialogFilter> AddExtensions(this List<FileDialogFilter> filters, string name, IEnumerable<string> extensions)
        {
            filters.Add(FileDialogFilter.ByExtensions(name, extensions));
            return filters;
        }
        public static List<FileDialogFilter> AddPatterns(this List<FileDialogFilter> filters, string name, params string[] patterns)
        {
            filters.Add(FileDialogFilter.ByPatterns(name, patterns));
            return filters;
        }
        public static List<FileDialogFilter> AddPatterns(this List<FileDialogFilter> filters, string name, IEnumerable<string> patterns)
        {
            filters.Add(FileDialogFilter.ByPatterns(name, patterns));
            return filters;
        }
    }

    public class FileDialog
    {
        private class WrapperWin32Window : IWin32Window
        {
            private readonly Window window;

            public WrapperWin32Window(Window window)
            {
                this.window = window;
            }

            public IntPtr Handle => new System.Windows.Interop.WindowInteropHelper(window).Handle;
        }

        private enum Behavior
        {
            Open, Save, Folder
        }

        private Microsoft.Win32.FileDialog _fd;
        private FolderBrowserDialog _ffd;

        private Window WindowParent { get; }

        public string DefaultFileName
        {
            get => _fd?.FileName ?? _ffd.SelectedPath;
            set
            {
                if (_fd != null)
                    _fd.FileName = value;
                else if (_ffd != null)
                    _ffd.SelectedPath = value;
            }
        }

        public string FileName => _fd.FileName;

        public IEnumerable<string> FileNames => _fd?.FileNames ?? new[] { DefaultFileName };

        private FileDialog(Microsoft.Win32.FileDialog fileDialog, Window wndParent)
        {
            _fd = fileDialog;
            WindowParent = wndParent;
        }

        private FileDialog(FolderBrowserDialog fileDialog, Window wndParent)
        {
            _ffd = fileDialog;
            WindowParent = wndParent;
        }

        private bool? InternalShowDialog()
        {
            if (_fd != null)
                return WindowParent == null ? _fd.ShowDialog() : _fd.ShowDialog(WindowParent);
            else if (_ffd != null)
                switch ( WindowParent == null ? _ffd.ShowDialog() :
                    _ffd.ShowDialog(new WrapperWin32Window(WindowParent)))
                {
                    case DialogResult.OK: return true;
                    case DialogResult.Cancel: return null;
                    case DialogResult.Abort: return false;
                    case DialogResult.Retry: return true;
                    case DialogResult.Ignore: return null;
                    case DialogResult.Yes: return true;
                    case DialogResult.No: return false;
                    default: return null;
                }

            return null;
        }

		private static FileDialog Factory(Window wndParent, Behavior behavior, IEnumerable<FileDialogFilter> filters, bool multipleSelection = false)
		{
            Microsoft.Win32.FileDialog fd;

            switch (behavior)
			{
				case Behavior.Open:
					fd = new Microsoft.Win32.OpenFileDialog()
					{
                        CheckFileExists = true,
                        CheckPathExists = true,
						Multiselect = multipleSelection,
					};
					break;
				case Behavior.Save:
                    fd = new Microsoft.Win32.SaveFileDialog()
                    {
                        CheckPathExists = true,
                    };
					break;
				default:
					throw new ArgumentException("Invalid parameter", nameof(behavior));
			}

            fd.Filter = string.Join("|", filters.Select(filter => filter.ToString()));

            return new FileDialog(fd, wndParent);
		}

        public static bool? OnOpen(
            Action<string> callback,
            IEnumerable<FileDialogFilter> filters = null,
            string defaultFileName = null,
            Window parent = null)
        {
            var fileDialog = FileDialog.Factory(parent, FileDialog.Behavior.Open, filters);
            fileDialog.DefaultFileName = defaultFileName;

            var result = fileDialog.InternalShowDialog();
            if (result == true)
                callback(fileDialog.FileName);

            return result;
        }

        public static bool? OnOpenMultiple(
            Action<string[]> callback,
            IEnumerable<FileDialogFilter> filters = null,
            string defaultFileName = null,
            Window parent = null)
        {
            var fileDialog = Factory(parent, Behavior.Open, filters, true);
            fileDialog.DefaultFileName = defaultFileName;

            var result = fileDialog.InternalShowDialog();
            if (result == true)
                callback(fileDialog.FileNames.ToArray());

            return result;
        }

        public static bool? OnSave(
            Action<string> callback,
            IEnumerable<FileDialogFilter> filters = null,
            string defaultFileName = null,
            System.Windows.Window parent = null)
        {
            var fileDialog = Factory(parent, Behavior.Save, filters);
            fileDialog.DefaultFileName = defaultFileName;

            var result = fileDialog.InternalShowDialog();
            if (result == true)
                callback(fileDialog.FileName);

            return result;
        }

        public static bool? OnFolder(
            Action<string> callback,
            IEnumerable<FileDialogFilter> filters = null,
            string defaultFileName = null,
            System.Windows.Window parent = null)
        {
            var fileDialog = new FileDialog(new FolderBrowserDialog(), parent);
            fileDialog.DefaultFileName = defaultFileName;

            var result = fileDialog.InternalShowDialog();
            if (result == true)
                callback(fileDialog.FileName);

            return result;
        }
    }
}
