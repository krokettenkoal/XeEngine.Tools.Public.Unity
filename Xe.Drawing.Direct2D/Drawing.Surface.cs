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

using System.Linq;
using static Xe.Drawing.Helpers;

namespace Xe.Drawing
{
    using SharpDX;
    using SharpDX.IO;
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;
    using dw = SharpDX.DirectWrite;
    using System.IO;
    using System.Drawing;
    using SharpDX.Win32;
    using System;
    using System.Runtime.InteropServices;
	using Xe.Tools.Utilities;

	public partial class DrawingDirect2D
    {
        public class MappedResource : IMappedResource
        {
            public MappedResource(d2.Bitmap1 bitmap)
            {
                var data = bitmap.Map(d2.MapOptions.Read);
                Bitmap = bitmap;
                Stride = data.Pitch;
                Data = data.DataPointer;
                Length = Stride * bitmap.PixelSize.Height;
            }

            public d2.Bitmap1 Bitmap { get; }

            public IntPtr Data { get; }

            public int Stride { get; }

            public int Length { get; }

            public void Dispose()
            {
                Bitmap.Unmap();
            }
        }

        public class CSurface : ISurface
        {
            private DrawingDirect2D _drawing;
            private d2.Bitmap1 _bitmap;
            private PixelFormat _pixelFormat;

            public int Width => _bitmap.PixelSize.Width;

            public int Height => _bitmap.PixelSize.Height;

            public Size Size => new Size(Width, Height);

            public PixelFormat PixelFormat => _pixelFormat;

            internal d2.Bitmap1 Bitmap => _bitmap;

            private d2.Bitmap1 _tmpBitmap;

            public IMappedResource Map()
            {
                if (_tmpBitmap == null || _tmpBitmap.Size != _bitmap.Size)
                {
                    _tmpBitmap?.Dispose();
                    _tmpBitmap = _drawing.CreateBitmap(Width, Height,
                        d2.BitmapOptions.CpuRead | d2.BitmapOptions.CannotDraw,
                        d2PixelFormat);
                }
                _tmpBitmap.CopyFromBitmap(_bitmap);
                return new MappedResource(_tmpBitmap);
            }

            public void Save(string filename)
            {
                _drawing.Save(_bitmap, filename);
            }

            public void Dispose()
            {
                _tmpBitmap?.Dispose();
                _bitmap.Dispose();
            }

            internal CSurface(DrawingDirect2D drawing, d2.Bitmap1 bitmap)
            {
                _drawing = drawing;
                _bitmap = bitmap;
                _pixelFormat = GetPixelFormat(_bitmap.PixelFormat.Format);
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type)
        {
            d2.BitmapOptions options;
            switch (type)
            {
                case SurfaceType.Input:
                    options = d2.BitmapOptions.None;
                    break;
                case SurfaceType.Output:
                    options = d2.BitmapOptions.Target | d2.BitmapOptions.CannotDraw;
                    break;
                case SurfaceType.InputOutput:
                    options = d2.BitmapOptions.Target;
                    break;
                default:
                    options = d2.BitmapOptions.Target;
                    break;
            }
            return CreateSurface(width, height, options);
        }

        public override ISurface CreateSurface(string filename, System.Drawing.Color[] filterColors)
        {
            var imagingFactory = device.ImagingFactory;

            using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var inputStream = new wic.WICStream(imagingFactory, fileStream))
                {
                    using (var pngDecoder = new wic.PngBitmapDecoder(imagingFactory))
                    {
                        pngDecoder.Initialize(inputStream, wic.DecodeOptions.CacheOnLoad);

                        // decode the loaded image to a format that can be consumed by D2D
                        using (var formatConverter = new wic.FormatConverter(imagingFactory))
                        {
                            var frame = pngDecoder.GetFrame(0);
                            wic.BitmapSource bmpSource;
                            if (frame.PixelFormat != wicPixelFormat)
                            {
                                formatConverter.Initialize(frame, wicPixelFormat);
                                bmpSource = formatConverter;
                            }
                            else
                            {
                                bmpSource = frame;
                            }

                            // load the base image into a D2D Bitmap
                            d2.Bitmap1 inputBitmap;
                            var bitmapProperties = new d2.BitmapProperties1(d2PixelFormat);
                            if (filterColors == null || filterColors.Length <= 0)
                            {
                                inputBitmap = d2.Bitmap1.FromWicBitmap(d2dContext, bmpSource, bitmapProperties);
                            }
                            else
                            {
                                var bmpSize = bmpSource.Size;
                                var stride = bmpSize.Width * 32 / 8;
                                var memSize = stride * bmpSize.Height;
                                var ptr = Marshal.AllocHGlobal(memSize);
                                bmpSource.CopyPixels(stride, ptr, memSize);
                                Xe.Tools.Utilities.BitmapUtility.MakeTransparent_Bgra32(ptr, stride, bmpSize.Height,
                                    filterColors
                                    .Select(x => new Xe.Graphics.Color()
                                    {
                                        a = x.A,
                                        r = x.R,
                                        g = x.G,
                                        b = x.B
                                    })
                                    .ToArray()
                                );

                                inputBitmap = new d2.Bitmap1(d2dContext, new Size2()
                                {
                                    Width = bmpSize.Width,
                                    Height = bmpSize.Height
                                }, new DataStream(ptr, memSize, true, false), stride, bitmapProperties);
                                Marshal.FreeHGlobal(ptr);
                            }
                            return new CSurface(this, inputBitmap);
                        }
                    }
                }
            }
        }

        private d2.Bitmap1 CreateBitmap(int width, int height, d2.BitmapOptions options, d2.PixelFormat? pixelFormat = null)
        {
            // create the d2d bitmap description and 96 DPI
            var d2dBitmapProps = new d2.BitmapProperties1(pixelFormat ?? d2PixelFormat, 96, 96, options);
            return new d2.Bitmap1(d2dContext, new Size2(width, height), d2dBitmapProps);
        }

        private ISurface CreateSurface(int width, int height, d2.BitmapOptions options)
        {
            var bitmap = CreateBitmap(width, height, options);
            return new CSurface(this, bitmap);
        }

        internal void Save(d2.Bitmap1 bitmap, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            var imagingFactory = device.ImagingFactory;
            using (var stream = new wic.WICStream(imagingFactory, filename, NativeFileAccess.Write))
            {
                using (var encoder = new wic.PngBitmapEncoder(imagingFactory))
                {
                    encoder.Initialize(stream);
                    using (var bitmapFrameEncode = new wic.BitmapFrameEncode(encoder))
                    {
                        var pixelSize = bitmap.PixelSize;
                        var dpi = bitmap.DotsPerInch;

                        bitmapFrameEncode.Initialize();
                        bitmapFrameEncode.SetSize(pixelSize.Width, pixelSize.Height);
                        bitmapFrameEncode.SetPixelFormat(ref wicPixelFormat);
                        using (var imageEncoder = new wic.ImageEncoder(imagingFactory, d2dDevice))
                        {
                            var imageParameters = new wic.ImageParameters(bitmap.PixelFormat,
                                dpi.Width, dpi.Height, 0, 0, pixelSize.Width, pixelSize.Height);
                            imageEncoder.WriteFrame(bitmap, bitmapFrameEncode, ref imageParameters);

                            bitmapFrameEncode.Commit();
                            encoder.Commit();
                        }
                    }
                }
            }
        }

    }
}
