// MIT License
// 
// Copyright(c) 2017 Luciano (Xeeynamo) Ciccariello
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus : IDrawing
    {
        private Graphics _graphics;
        private bool _invalidated;
        private CSurface _surface;
        private Filter _filter;

        public ISurface Surface
        {
            get
            {
                if (_invalidated)
                {
                    _invalidated = false;
					Flush();
                }
                return _surface;
            }
            set
            {
                if (_surface != value)
                {
                    _surface?.Dispose();
					Flush();
					if (value is CSurface surface)
                    {
                        _surface = surface;
                        _graphics = Graphics.FromImage(_surface.Bitmap);
                    }
                    else
                    {
                        _surface = null;
                        _graphics = null;
                    }
                }
            }
        }

        public Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                InterpolationMode interpolationMode;
                switch (value)
                {
                    case Filter.Nearest:
                        interpolationMode = InterpolationMode.NearestNeighbor;
                        break;
                    case Filter.Linear:
                        interpolationMode = InterpolationMode.Bilinear;
                        break;
                    case Filter.Cubic:
                        interpolationMode = InterpolationMode.Bicubic;
                        break;
                    default:
                        interpolationMode = InterpolationMode.Invalid;
                        break;
                }
                _graphics.InterpolationMode = interpolationMode;
            }
        }

        public ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type)
        {
            using (var bitmap = new Bitmap(width, height, GetPixelFormat(pixelFormat)))
            {
                return new CSurface(bitmap)
                {
                    PixelFormat = pixelFormat
                };
            }
        }

		public void Flush()
		{
			_graphics.Flush();
		}

		public void Clear(Color color)
        {
            _graphics.Clear(color);
		}

		public void DrawRectangle(RectangleF rect, Color color, float width = 1.0f)
		{
			using (var brush = new SolidBrush(color))
			{
				using (var pen = new Pen(brush, width))
				{
					_graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
				}
			}
		}

		public void FillRectangle(RectangleF rect, Color color)
		{
			using (var brush = new SolidBrush(color))
			{
				_graphics.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
			}
		}

		public void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip)
        {
            var mySurface = surface as CSurface;
            if (mySurface == null)
                throw new ArgumentException("Invalid surface specified", nameof(surface));
            var bitmap = mySurface.Bitmap;
            switch (flip)
            {
                case Flip.None:
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    break;
                case Flip.FlipHorizontal:
                    _graphics.TranslateTransform(dst.Width, 0);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.TranslateTransform(-dst.Width, 0);
                    break;
                case Flip.FlipVertical:
                    _graphics.TranslateTransform(0, dst.Height);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.TranslateTransform(0, -dst.Height);
                    break;
                case Flip.FlipBoth:
                    _graphics.TranslateTransform(dst.Width, dst.Height);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.TranslateTransform(-dst.Width, -dst.Height);
                    break;
            }
            Invalidate();
		}

		public void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None)
		{
			DrawSurface(surface, src, dst, flip);
		}

		public void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None)
		{
			DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color0, ColorF color1, ColorF color2, ColorF color3)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _surface.Dispose();
            _graphics.Dispose();
        }

        /*private DrawingGdiPlus(int width, int height, PixelFormat pixelFormat)
        {
            using (var bitmap = new Bitmap(width, height, GetPixelFormat(pixelFormat)))
            {
                _surface = CreateSurface(bitmap) as CSurface;
                _surface.PixelFormat = pixelFormat;
                _graphics = Graphics.FromImage(_surface.Bitmap);
            }
        }
        private DrawingGdiPlus(CSurface surface)
        {
            _surface = surface;
            _graphics = Graphics.FromImage(_surface.Bitmap);
        }*/

        private void Invalidate()
        {
            _invalidated = true;
        }

        private static System.Drawing.Imaging.PixelFormat GetPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppRgb555: return System.Drawing.Imaging.PixelFormat.Format16bppRgb555;
                case PixelFormat.Format16bppRgb565: return System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
                case PixelFormat.Format24bppRgb: return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                case PixelFormat.Format32bppRgb: return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
                case PixelFormat.Format1bppIndexed: return System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
                case PixelFormat.Format4bppIndexed: return System.Drawing.Imaging.PixelFormat.Format4bppIndexed;
                case PixelFormat.Format8bppIndexed: return System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                case PixelFormat.Format16bppArgb1555: return System.Drawing.Imaging.PixelFormat.Format16bppArgb1555;
                case PixelFormat.Format32bppPArgb: return System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
                case PixelFormat.Format16bppGrayScale: return System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
                case PixelFormat.Format48bppRgb: return System.Drawing.Imaging.PixelFormat.Format48bppRgb;
                case PixelFormat.Format64bppPArgb: return System.Drawing.Imaging.PixelFormat.Format64bppPArgb;
                case PixelFormat.Format32bppArgb: return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case PixelFormat.Format64bppArgb: return System.Drawing.Imaging.PixelFormat.Format64bppArgb;
                default: return System.Drawing.Imaging.PixelFormat.Undefined;
            }
        }
    }
}
