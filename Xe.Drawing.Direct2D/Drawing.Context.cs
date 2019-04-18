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
using static Xe.Drawing.Helpers;

namespace Xe.Drawing
{
    // use namespaces shortcuts to reduce typing and avoid the messing the same class names from different namespaces
    using d2 = SharpDX.Direct2D1;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;

    public partial class DrawingDirect2D
    {
        private static d2.PixelFormat d2PixelFormat = new d2.PixelFormat(dxgi.Format.B8G8R8A8_UNorm, d2.AlphaMode.Premultiplied);
        private static Guid wicPixelFormat = wic.PixelFormat.Format32bppPBGRA;

        private d2.Device d2dDevice;
        private d2.DeviceContext d2dContext;

        private void Initialize()
        {
            d2dDevice = device.GetD2DDevice();
            d2dContext = new d2.DeviceContext(d2dDevice, d2.DeviceContextOptions.None);
        }
    }
}
