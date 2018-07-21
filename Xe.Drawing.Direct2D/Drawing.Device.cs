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
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;

    public partial class DrawingDirect2D
    {
        private static Device device = new Device();

        private class Device : IDisposable
        {
            private d3d.Device d3dDevice;
            private d3d.Device1 d3dDevice1;
            private dxgi.Device dxgiDevice;
            private d2.Device d2dDevice;

            internal wic.ImagingFactory2 ImagingFactory { get; private set; }

            internal Device()
            {
                var flags = d3d.DeviceCreationFlags.VideoSupport |
                    d3d.DeviceCreationFlags.BgraSupport;
                d3dDevice = new d3d.Device(SharpDX.Direct3D.DriverType.Hardware, flags);
                d3dDevice1 = d3dDevice.QueryInterface<d3d.Device1>();
                dxgiDevice = d3dDevice.QueryInterface<dxgi.Device>();
                d2dDevice = new d2.Device(dxgiDevice);

                ImagingFactory = new wic.ImagingFactory2();
            }

            internal d2.Device GetD2DDevice()
            {
                return d2dDevice;
            }

            public void Dispose()
            {
                d2dDevice.Dispose();
                dxgiDevice.Dispose();
                d3dDevice1.Dispose();
                d3dDevice.Dispose();
            }
        }
    }
}
