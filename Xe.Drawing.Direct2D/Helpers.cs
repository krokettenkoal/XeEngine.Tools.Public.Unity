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

namespace Xe.Drawing
{
    internal static class Helpers
    {
        internal static PixelFormat GetPixelFormat(Guid guid)
        {
            if (guid == SharpDX.WIC.PixelFormat.Format16bppBGR555) return PixelFormat.Format16bppRgb555;
            if (guid == SharpDX.WIC.PixelFormat.Format16bppBGR565) return PixelFormat.Format16bppRgb565;
            if (guid == SharpDX.WIC.PixelFormat.Format24bppBGR) return PixelFormat.Format24bppRgb;
            if (guid == SharpDX.WIC.PixelFormat.Format32bppBGR) return PixelFormat.Format32bppRgb;
            if (guid == SharpDX.WIC.PixelFormat.Format1bppIndexed) return PixelFormat.Format1bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format2bppIndexed) return PixelFormat.Format2bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format4bppIndexed) return PixelFormat.Format4bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format8bppIndexed) return PixelFormat.Format8bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format16bppBGRA5551) return PixelFormat.Format16bppArgb1555;
            if (guid == SharpDX.WIC.PixelFormat.Format32bppPBGRA) return PixelFormat.Format32bppPArgb;
            if (guid == SharpDX.WIC.PixelFormat.Format16bppGray) return PixelFormat.Format16bppGrayScale;
            if (guid == SharpDX.WIC.PixelFormat.Format48bppBGR) return PixelFormat.Format48bppRgb;
            if (guid == SharpDX.WIC.PixelFormat.Format64bppPBGRA) return PixelFormat.Format64bppPArgb;
            if (guid == SharpDX.WIC.PixelFormat.Format32bppBGRA) return PixelFormat.Format32bppArgb;
            if (guid == SharpDX.WIC.PixelFormat.Format64bppBGRA) return PixelFormat.Format64bppArgb;

            if (guid == SharpDX.WIC.PixelFormat.Format2bppGray) return PixelFormat.Format2bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format4bppGray) return PixelFormat.Format4bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format8bppGray) return PixelFormat.Format8bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format8bppAlpha) return PixelFormat.Format8bppIndexed;
            if (guid == SharpDX.WIC.PixelFormat.Format24bppRGB) return PixelFormat.Format24bppRgb;
            if (guid == SharpDX.WIC.PixelFormat.Format32bppRGB) return PixelFormat.Format32bppRgb;
            if (guid == SharpDX.WIC.PixelFormat.Format32bppRGBA) return PixelFormat.Format32bppArgb;

            return PixelFormat.Undefined;
        }

        internal static PixelFormat GetPixelFormat(SharpDX.DXGI.Format format)
        {
            if (format == SharpDX.DXGI.Format.B5G5R5A1_UNorm) return PixelFormat.Format16bppRgb555;
            if (format == SharpDX.DXGI.Format.B5G6R5_UNorm) return PixelFormat.Format16bppRgb565;
            //if (format == SharpDX.DXGI.Format.) return PixelFormat.Format24bppRgb;
            if (format == SharpDX.DXGI.Format.B8G8R8X8_UNorm) return PixelFormat.Format32bppRgb;
            //if (format == SharpDX.DXGI.Format.) return PixelFormat.Format1bppIndexed;
            //if (format == SharpDX.DXGI.Format.) return PixelFormat.Format4bppIndexed;
            /*if (format == SharpDX.DXGI.Format.R8_UInt) return PixelFormat.Format8bppIndexed;
            if (format == SharpDX.DXGI.Format.Format16bppBGRA5551) return PixelFormat.Format16bppArgb1555;
            if (format == SharpDX.DXGI.Format.Format32bppPBGRA) return PixelFormat.Format32bppPArgb;
            if (format == SharpDX.DXGI.Format.Format16bppGray) return PixelFormat.Format16bppGrayScale;
            if (format == SharpDX.DXGI.Format.Format48bppBGR) return PixelFormat.Format48bppRgb;
            if (format == SharpDX.DXGI.Format.Format64bppPBGRA) return PixelFormat.Format64bppPArgb;
            if (format == SharpDX.DXGI.Format.Format32bppBGRA) return PixelFormat.Format32bppArgb;
            if (format == SharpDX.DXGI.Format.Format64bppBGRA) return PixelFormat.Format64bppArgb;

            if (format == SharpDX.DXGI.Format.Format2bppGray) return PixelFormat.Indexed;
            if (format == SharpDX.DXGI.Format.Format4bppGray) return PixelFormat.Format4bppIndexed;
            if (format == SharpDX.DXGI.Format.Format8bppGray) return PixelFormat.Format8bppIndexed;
            if (format == SharpDX.DXGI.Format.Format8bppAlpha) return PixelFormat.Format8bppIndexed;
            if (format == SharpDX.DXGI.Format.Format24bppRGB) return PixelFormat.Format24bppRgb;
            if (format == SharpDX.DXGI.Format.Format32bppRGB) return PixelFormat.Format32bppRgb;
            if (format == SharpDX.DXGI.Format.Format32bppRGBA) return PixelFormat.Format32bppArgb;*/

            return PixelFormat.Undefined;
        }

        internal static Guid GetPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppRgb555: return SharpDX.WIC.PixelFormat.Format16bppBGR555;
                case PixelFormat.Format16bppRgb565: return SharpDX.WIC.PixelFormat.Format16bppBGR565;
                case PixelFormat.Format24bppRgb: return SharpDX.WIC.PixelFormat.Format24bppBGR;
                case PixelFormat.Format32bppRgb: return SharpDX.WIC.PixelFormat.Format32bppBGR;
                case PixelFormat.Format1bppIndexed: return SharpDX.WIC.PixelFormat.Format1bppIndexed;
                case PixelFormat.Format4bppIndexed: return SharpDX.WIC.PixelFormat.Format4bppIndexed;
                case PixelFormat.Format8bppIndexed: return SharpDX.WIC.PixelFormat.Format8bppIndexed;
                case PixelFormat.Format16bppArgb1555: return SharpDX.WIC.PixelFormat.Format16bppBGRA5551;
                case PixelFormat.Format32bppPArgb: return SharpDX.WIC.PixelFormat.Format32bppPBGRA;
                case PixelFormat.Format16bppGrayScale: return SharpDX.WIC.PixelFormat.Format16bppGray;
                case PixelFormat.Format48bppRgb: return SharpDX.WIC.PixelFormat.Format48bppBGR;
                case PixelFormat.Format64bppPArgb: return SharpDX.WIC.PixelFormat.Format64bppPBGRA;
                case PixelFormat.Format32bppArgb: return SharpDX.WIC.PixelFormat.Format32bppBGRA;
                case PixelFormat.Format64bppArgb: return SharpDX.WIC.PixelFormat.Format64bppBGRA;
                default: return SharpDX.WIC.PixelFormat.FormatDontCare;
            }
        }
    }
}
