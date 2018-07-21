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

using System.Drawing;

namespace Xe.Drawing
{
    public enum SurfaceType
    {
        // Surface used as input for drawing
        Input,
        // Surface used where the content will be drawn.
        Output,
        // Used as input and output
        InputOutput
    }

    public abstract class Drawing : IDrawing
    {
        public abstract ISurface Surface { get; set; }
        public abstract Filter Filter { get; set; }

		public abstract void Flush();
        public abstract void Clear(Color color);
        public abstract ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type);
        public abstract ISurface CreateSurface(string filename, Color[] filterColors = null);
        public abstract void Dispose();
        
        public void DrawSurface(ISurface surface, int x, int y, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            var dst = new Rectangle(x, y, src.Width, src.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, int x, int y, int width, int height, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            var dst = new Rectangle(x, y, width, height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle dst, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle src, int x, int y, Flip flip)
        {
            var dst = new Rectangle(x, y, src.Width, src.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle src, int x, int y, int width, int height, Flip flip)
        {
            var dst = new Rectangle(x, y, width, height);
            DrawSurface(surface, src, dst, flip);
		}

		public void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
		{
			DrawSurface(surface, src, new RectangleF(dst.X, dst.Y, dst.Width, dst.Height), flip);
		}

		public abstract void DrawRectangle(RectangleF rect, Color color, float width = 1.0f);
		public abstract void FillRectangle(RectangleF rect, Color color);
		public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip);
		public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None);
		public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None);
        public virtual void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color,
            float centerX, float centerY, float centerZ, float scaleX, float scaleY, float scaleZ,
            float rotateX, float rotateY, float rotateZ, Flip flip = Flip.None)
        { }
    }
}
