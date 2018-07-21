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

using System.ComponentModel;
using System.Windows;

namespace Xe.Tools.Wpf.Controls
{
    public abstract class WindowEx : Window
    {
        public static readonly DependencyProperty AskExitConfirmationProperty =
            DependencyProperty.Register(
                "AskExitConfirmation",
                typeof(bool),
                typeof(WindowEx),
                new PropertyMetadata(false, new PropertyChangedCallback(OnAskExitConfirmationPropertyChanged)),
                new ValidateValueCallback(ValidateBoolean));

        public bool AskExitConfirmation
        {
            get => (bool)GetValue(AskExitConfirmationProperty);
            set => SetValue(AskExitConfirmationProperty, value);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (AskExitConfirmation)
            {
                e.Cancel = !DoAskExitConfirmation().HasValue;
            }
            base.OnClosing(e);
        }

        protected virtual bool? DoAskExitConfirmation()
        {
            const string Message = "There are pending changes. Do you want to save them?";
            const string Title = "Save confirmation";
            const MessageBoxButton Buttons = MessageBoxButton.YesNoCancel;
            const MessageBoxImage Icon = MessageBoxImage.Warning;

            bool? result;
            switch (MessageBox.Show(Message, Title, Buttons, Icon))
            {
                case MessageBoxResult.None:
                    result = null;
                    break;
                case MessageBoxResult.OK:
                    result = true;
                    break;
                case MessageBoxResult.Cancel:
                    result = null;
                    break;
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
                default:
                    result = null;
                    break;
            }

            if (result == true)
            {
                if (!DoSaveChanges())
                {
                    const string SaveErrMessage = "There was an error during saving.";
                    const string SaveErrTitle = "Save error";
                    const MessageBoxButton SaveErrButtons = MessageBoxButton.OK;
                    const MessageBoxImage SaveErrIcon = MessageBoxImage.Error;
                    MessageBox.Show(SaveErrMessage, SaveErrTitle, SaveErrButtons, SaveErrIcon);
                    result = null;
                }
            }
            return result;
        }

        protected abstract bool DoSaveChanges();

        private static void OnAskExitConfirmationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as WindowEx)
        }
        private static bool ValidateBoolean(object o)
        {
            return o is bool;
        }
    }
}
