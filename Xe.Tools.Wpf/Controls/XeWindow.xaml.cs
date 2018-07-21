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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Xe.Tools.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for XeWindow.xaml
    /// </summary>
    public partial class XeWindow : UserControl
    {
        private Window _window;

        private Window Window
        {
            get
            {
                if (_window == null)
                {
                    var parent = Parent;
                    while (!(parent is Window))
                        parent = (parent as FrameworkElement).Parent;
                    _window = parent as Window;
                }
                return _window;
            }
        }

        public XeWindow()
        {
            InitializeComponent();
            Loaded += (x, y) =>
            {
                Window.WindowStyle = WindowStyle.None;
                ProcessState(Window?.WindowState);
            };
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    InvertWindowState();
                }
                else
                {
                    Window.DragMove();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Maximized);
        }
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Normal);
        }

        private void InvertWindowState()
        {
            SetWindowState(Window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        }
        private void SetWindowState(WindowState state)
        {
            Window.WindowState = state;
            ProcessState(state);
        }
        private void ProcessState(WindowState? state)
        {
            switch (state)
            {
                case WindowState.Maximized:
                    ButtonRestore.Visibility = Visibility.Visible;
                    ButtonMaximize.Visibility = Visibility.Collapsed;
                    break;
                case WindowState.Normal:
                    ButtonRestore.Visibility = Visibility.Collapsed;
                    ButtonMaximize.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
