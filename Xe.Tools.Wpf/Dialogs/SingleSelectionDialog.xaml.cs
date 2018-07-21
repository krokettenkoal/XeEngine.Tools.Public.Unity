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

using System.Collections.Generic;
using System.Windows;

namespace Xe.Tools.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for SingleSelectionDialog.xaml
    /// </summary>
    public partial class SingleSelectionDialog : Window
    {
        private class ViewModel : BaseNotifyPropertyChanged
        {
            private string _description;
            private IEnumerable<object> _items;
            private object _selectedItem;

            public string Description
            {
                get => _description;
                set
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }

            public IEnumerable<object> Items
            {
                get => _items;
                set
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }

            public object SelectedValue
            {
                get => _selectedItem;
                set
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private ViewModel _vm;

        public string Description
        {
            get => _vm.Description;
            set => _vm.Description = value;
        }

        public IEnumerable<object> Items
        {
            get => _vm.Items;
            set => _vm.Items = value;
        }

        public object SelectedItem
        {
            get => _vm.SelectedValue;
            set => _vm.SelectedValue = value;
        }

        public SingleSelectionDialog()
        {
            InitializeComponent();
            DataContext = _vm = new ViewModel();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
