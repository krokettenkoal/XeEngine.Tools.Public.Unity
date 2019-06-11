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
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus
    {
        private class MappedResource : IMappedResource
        {
            public MappedResource(Bitmap bitmap)
            {
                Bitmap = bitmap;
                BitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    bitmap.PixelFormat);
            }

            public Bitmap Bitmap { get; }

            public BitmapData BitmapData { get; }

            public IntPtr Data => BitmapData.Scan0;

            public int Stride => BitmapData.Stride;

            public int Length => BitmapData.Stride * BitmapData.Height;

            public void Dispose()
            {
                Bitmap.UnlockBits(BitmapData);
            }
        }

        internal class CSurface : ISurface
        {
            internal Bitmap Bitmap { get; set; }

            public int Width => Bitmap.Width;

            public int Height => Bitmap.Height;

            public Size Size => Bitmap.Size;

            public PixelFormat PixelFormat { get; internal set; }


            public IMappedResource Map()
            {
                return new MappedResource(Bitmap);
            }

            public void Save(string filename)
            {
                Bitmap.Save(filename);
            }

            public void Dispose()
            {
                Bitmap.Dispose();
            }

            internal CSurface(string filename)
            {
                Bitmap = new Bitmap(filename);
                Init();
            }
            internal CSurface(Image image)
            {
                Bitmap = new Bitmap(image);
                Init();
            }
            internal CSurface(Bitmap bitmap)
            {
                Bitmap = bitmap.Clone() as Bitmap;
                Init();
            }

            private void Init()
            {
                Bitmap.MakeTransparent(Color.Magenta);
            }
        }

        public ISurface CreateSurface(string filename, Color[] filterColors)
        {
            var surface = new CSurface(filename);
            if (filterColors != null && filterColors.Length > 0)
            {
                foreach (var color in filterColors)
                {
                    surface.Bitmap.MakeTransparent(color);
                }
            }
            return surface;
        }
        public ISurface CreateSurface(Image image)
        {
            return new CSurface(image);
        }
        public ISurface CreateSurface(Bitmap bitmap)
        {
            return new CSurface(bitmap);
        }

        public ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type = SurfaceType.Input, DataResource dataResource = null)
        {
            throw new NotImplementedException();
        }
    }
}
