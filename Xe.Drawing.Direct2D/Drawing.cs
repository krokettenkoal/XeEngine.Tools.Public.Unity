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

using SharpDX.Direct2D1;
using SharpDX.Mathematics;
using SharpDX.Mathematics.Interop;
using System.Drawing;

namespace Xe.Drawing
{
    public partial class DrawingDirect2D : Drawing
    {
        private CSurface _surface;
        private bool _invalidated;
        private Filter _filter;
        private InterpolationMode _interpolationMode;

        public override ISurface Surface
        {
            get
            {
                if (_invalidated)
                {
                    _invalidated = false;
					Flush();
                    d2dContext.EndDraw();
                }
                return _surface;
            }
            set
            {
                var oldSurface = Surface;
                if (value is CSurface surface)
                {
                    _surface = surface;
                    d2dContext.Target = surface.Bitmap;
                }
            }
        }

        public override Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                switch (value)
                {
                    case Filter.Nearest:
                        _interpolationMode = InterpolationMode.NearestNeighbor;
                        break;
                    case Filter.Linear:
                        _interpolationMode = InterpolationMode.Linear;
                        break;
                    case Filter.Cubic:
                        _interpolationMode = InterpolationMode.Cubic;
                        break;
                }
            }
        }

		public override void Flush()
		{
			d2dContext.Flush();
		}

		public override void Clear(Color color)
        {
            var r = color.R / 255.0f;
            var g = color.G / 255.0f;
            var b = color.B / 255.0f;
            var a = color.A / 255.0f;
            Invalidate();
            d2dContext.Clear(new RawColor4(r, g, b, a));
		}

		public override void DrawRectangle(RectangleF rect, Color color, float width)
		{
			using (var brush = new SolidColorBrush(d2dContext, ToRaw(color)))
			{
				d2dContext.DrawRectangle(ToRaw(rect), brush, width);
			}
		}

		public override void FillRectangle(RectangleF rect, Color color)
		{
			using (var brush = new SolidColorBrush(d2dContext, ToRaw(color)))
			{
				d2dContext.FillRectangle(ToRaw(rect), brush);
			}
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip)
		{
			DrawSurface(surface, src, dst, 1.0f, flip);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip)
		{
            RawMatrix? matrix = flip != Flip.None ?
                ToMatrix(dst, flip) : (RawMatrix?)null;
            DrawSurface(surface, src, dst, alpha, matrix);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None)
		{
			DrawSurface(surface, src, dst, color.A, flip);
		}

        public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color,
            float centerX, float centerY, float centerZ, float scaleX, float scaleY, float scaleZ,
            float rotateX, float rotateY, float rotateZ, Flip flip = Flip.None)
        {
            var sr = System.Math.Sin(rotateY);
            var cr = System.Math.Cos(rotateY);
            var sp = System.Math.Sin(rotateX);
            var cp = System.Math.Cos(rotateX);
            var sy = System.Math.Sin(rotateZ);
            var cy = System.Math.Cos(rotateZ);

            var _11 = cp * cy;
            var _12 = sr * sp * cy - cr * sy;
            var _13 = cr * sp * cy + sr * sy;
            var _21 = cp * sy;
            var _22 = sr * sp * sy + cr * cy;
            var _23 = cr * sp * sy - sr * cy;
            var _31 = -sp;
            var _32 = sr * cp;
            var _33 = cr * cp;

            RawMatrix matrix = new RawMatrix
            {
                M11 = (float)(_11 * scaleX),
                M12 = (float)(_12 * scaleY),
                M13 = (float)(_13 * scaleZ),
                M14 = 0.0f,
                M21 = (float)(_21 * scaleX),
                M22 = (float)(_22 * scaleY),
                M23 = (float)(_23 * scaleZ),
                M24 = 0.0f,
                M31 = (float)(_31 * scaleX),
                M32 = (float)(_32 * scaleY),
                M33 = (float)(_33 * scaleZ),
                M34 = 0.0f,
                M41 = (float)((centerX * _11 + centerY * _21 + centerZ * _31) * scaleX),
                M42 = (float)((centerX * _12 + centerY * _22 + centerZ * _32) * scaleY),
                M43 = (float)((centerX * _13 + centerY * _23 + centerZ * _33) * scaleZ),
                M44 = 1.0f
            };
            DrawSurface(surface, src, dst, color.A, matrix);
        }

        private void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, RawMatrix? matrix)
        {
            var s = surface as CSurface;
            var srcf = new RawRectangleF(src.Left, src.Top, src.Right, src.Bottom);
            var dstf = new RawRectangleF(dst.Left, dst.Top, dst.Right, dst.Bottom);
            Invalidate();
            d2dContext.DrawBitmap(s.Bitmap, dstf, alpha, _interpolationMode, srcf, matrix);
        }

        public override void Dispose()
        {
            _surface?.Dispose();
            d2dContext?.Dispose();
        }

        private void Invalidate()
        {
            if (!_invalidated)
            {
                _invalidated = true;
                d2dContext.BeginDraw();
            }
        }

        public DrawingDirect2D()
        {
            CommonInit();
        }

        private void CommonInit()
        {
            Filter = Filter.Nearest;
            Initialize();
        }

        private static RawRectangle ToRaw(Rectangle rect)
        {
            return new RawRectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private static RawRectangleF ToRaw(RectangleF rect)
        {
            return new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private static RawMatrix ToMatrix(RectangleF rect, Flip flip)
        {
            switch (flip)
            {
                case Flip.FlipHorizontal:
                    return new RawMatrix()
                    {
                        M11 = -1,
                        M22 = +1,
                        M33 = +1,
                        M44 = +1,
                        M41 = rect.Left * 2 + rect.Width,
                    };
                case Flip.FlipVertical:
                    return new RawMatrix
                    {
                        M11 = +1,
                        M22 = -1,
                        M33 = +1,
                        M44 = +1,
                        M42 = rect.Top * 2 + rect.Height,
                    };
                case Flip.FlipBoth:
                    return new RawMatrix
                    {
                        M11 = -1,
                        M22 = -1,
                        M33 = +1,
                        M44 = +1,
                        M41 = rect.Left * 2 + rect.Width,
                        M42 = rect.Top * 2 + rect.Height,
                    };
                default:
                    return new RawMatrix()
                    {
                        M11 = +1,
                        M22 = +1,
                        M33 = +1,
                        M44 = +1,
                    };
            }
        }

        private static RawColor4 ToRaw(Color color)
        {
            return new RawColor4(color.R / 255.0f, color.G / 255.0f,
                color.B / 255.0f, color.A / 255.0f);
        }
    }
}
