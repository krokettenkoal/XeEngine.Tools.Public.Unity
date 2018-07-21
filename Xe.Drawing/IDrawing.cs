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

namespace Xe.Drawing
{
    [Flags]
    public enum Flip
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2,
        FlipBoth = FlipHorizontal | FlipVertical
    }

    public enum Filter
    {
        Nearest,
        Linear,
        Cubic
    }

    public interface IDrawing : IDisposable
    {
        ISurface Surface { get; set; }
        Filter Filter { get; set; }

        ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type = SurfaceType.Input);
        ISurface CreateSurface(string filename, Color[] filterColors = null);

		void Flush();

        void Clear(Color color);
		void DrawRectangle(RectangleF rect, Color color, float width = 1.0f);
		void FillRectangle(RectangleF rect, Color color);
		void DrawSurface(ISurface surface, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, int x, int y, int width, int height, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle dst, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, int width, int height, Flip flip = Flip.None);
		void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip = Flip.None);

		void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip = Flip.None);

		void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None);

        void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color,
            float centerX, float centerY, float centerZ, float scaleX, float scaleY, float scaleZ,
            float rotateX, float rotateY, float rotateZ, Flip flip = Flip.None);
    }
}