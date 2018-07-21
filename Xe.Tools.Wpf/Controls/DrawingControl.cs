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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Drawing;

namespace Xe.Tools.Wpf.Controls
{
	public class DrawingControl : FrameworkElement
	{
		[DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
		private static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

		private VisualCollection _children;
		private DrawingVisual _visual;
		private WriteableBitmap _writeableBitmap;
		private System.Timers.Timer _timer = new System.Timers.Timer();
		private System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
		private System.Diagnostics.Stopwatch _stopwatchDeltaTime = new System.Diagnostics.Stopwatch();

		protected IDrawing Drawing { get; private set; }

		public static readonly DependencyProperty FramesPerSecondProperty =
			DependencyProperty.Register(
				"FramesPerSecond",
				typeof(double),
				typeof(DrawingControl),
				new PropertyMetadata(30.0, new PropertyChangedCallback(OnFramesPerSecondPropertyChanged)),
				new ValidateValueCallback(ValidateFramesPerSecond));

		/// <summary>
		/// Get or set how frames per second needs to be drawn.
		/// A value of 0 stops the execution.
		/// Values below 0 are not valid.
		/// </summary>
		public double FramesPerSecond
		{
			get => (double)GetValue(FramesPerSecondProperty);
			set => SetValue(FramesPerSecondProperty, value);
		}

		public double LastDrawTime { get; private set; }

		public double LastDrawAndPresentTime { get; private set; }

		public double DeltaTime { get; private set; }

		public DrawingControl()
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				_visual = new DrawingVisual();
				_children = new VisualCollection(this)
				{
					_visual
				};

				_timer.Elapsed += (sender, args) =>
				{
					if (Drawing == null)
						return;
					Application.Current?.Dispatcher.Invoke(new Action(() =>
					{
						DoRender();
					}));
				};

				SetFramesPerSecond(FramesPerSecond);
			}
			else
			{
				_children = new VisualCollection(this);
			}
		}

		/// <summary>
		/// Rendering on demand.
		/// </summary>
		public void DoRender()
		{
			DeltaTime = _stopwatchDeltaTime.Elapsed.TotalMilliseconds / 1000.0;
			_stopwatchDeltaTime.Restart();

			_stopwatch.Restart();
			OnDrawRequired();
			LastDrawTime = _stopwatch.Elapsed.TotalMilliseconds;
			Present();
			LastDrawAndPresentTime = _stopwatch.Elapsed.TotalMilliseconds;
			OnDrawCompleted();
		}

		public async Task DoRenderAsync()
		{
			await DoRenderTask();
		}

		public Task DoRenderTask()
		{
			return Task.Run(() =>
			{
				DoRender();
			});
		}

		// Provide a required override for the VisualChildrenCount property.
		protected override int VisualChildrenCount
		{
			get { return _children.Count; }
		}

		// Provide a required override for the GetVisualChild method.
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= _children.Count)
			{
				throw new ArgumentOutOfRangeException();
			}

			return _children[index];
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				var size = sizeInfo.NewSize;
				ResizeRenderingEngine((int)size.Width, (int)size.Height);
				base.OnRenderSizeChanged(sizeInfo);
			}
		}

		protected virtual void OnDrawingCreated()
		{

		}

		protected virtual void OnDrawRequired()
		{

		}

		protected virtual void OnDrawCompleted()
		{

		}

		private void SetFramesPerSecond(double framesPerSec)
		{
			if (framesPerSec < 0)
				throw new ArgumentException($"{nameof(FramesPerSecond)} value is set to {framesPerSec}, but it cannot be below than 0.");
			if (framesPerSec > 0)
			{
				_timer.Enabled = true;
				_timer.Interval = 1000.0 / framesPerSec;
			}
			else
			{
				_timer.Enabled = false;
			}
		}

		private void Present()
		{
            Present(Drawing.Surface);
		}

		private void Present(ISurface surface)
        {
            if (surface != null && surface.Width > 0 && surface.Height > 0)
            {
                using (var map = surface.Map())
                {
                    if (_writeableBitmap == null ||
                        surface.Width != _writeableBitmap.Width ||
                        surface.Height != _writeableBitmap.Height ||
                        map.Stride / 4 != _writeableBitmap.Width)
                    {
                        _writeableBitmap = new WriteableBitmap(map.Stride / 4, surface.Height, 96.0, 96.0, PixelFormats.Bgra32, null);
                    }

                    _writeableBitmap.Lock();
                    CopyMemory(_writeableBitmap.BackBuffer, map.Data, map.Length);
                    _writeableBitmap.AddDirtyRect(new Int32Rect()
                    {
                        X = 0,
                        Y = 0,
                        Width = surface.Width,
                        Height = surface.Height
                    });
                    _writeableBitmap.Unlock();
                }
            }

            using (var dc = _visual.RenderOpen())
            {
                Present(dc, _writeableBitmap);
            }
        }

        private void Present(DrawingContext dc, ImageSource image)
        {
            dc.DrawImage(image, new Rect()
            {
                X = 0,
                Y = 0,
                Width = image.Width,
                Height = image.Height
            });
        }

		private void ResizeRenderingEngine(int width, int height)
		{
			if (Drawing == null)
			{
				Drawing = Factory.Resolve<IDrawing>();
				if (Drawing != null)
					OnDrawingCreated();
			}
			if (Drawing != null)
			{
				Drawing.Surface?.Dispose();
				Drawing.Surface = Drawing.CreateSurface(
					width, height, Xe.Drawing.PixelFormat.Format32bppArgb, SurfaceType.InputOutput);
				DoRender();
			}
		}

		private static void OnFramesPerSecondPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DrawingControl).SetFramesPerSecond((double)e.NewValue);
		}

		private static bool ValidateFramesPerSecond(object o)
		{
			if (o is double value)
				return value >= 0.0;
			return false;
		}
	}
}
