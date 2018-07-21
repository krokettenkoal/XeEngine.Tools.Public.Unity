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
    using dx = SharpDX;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;

    public partial class DrawingDirect3D : Drawing
    {
        private Filter _filter = Filter.Nearest;
        private d3d.RenderTargetView _renderTarget;
        private CSurface _dstSurface;
        private SizeF _viewportSize;

        public override ISurface Surface
        {
            get => _dstSurface;
            set
            {
                if (value is CSurface surface)
                {
                    _renderTarget?.Dispose();
                    _renderTarget = new d3d.RenderTargetView(Device, surface.Texture);
                    Context.OutputMerger.SetRenderTargets(_renderTarget);
                    _dstSurface = surface;

                    var viewport = new dx.Viewport(0, 0, surface.Width, surface.Height);
                    Context.Rasterizer.SetViewport(viewport);
                    _viewportSize = new SizeF(surface.Width, surface.Height);
                }
                else
                {
                    _renderTarget?.Dispose();
                    _renderTarget = null;
                    Context.OutputMerger.SetRenderTargets(_renderTarget);
                }
            }
        }
        public override Filter Filter
        {
            get => _filter;
            set => _filter = value;
        }

        public override void Clear(Color color)
        {
            if (_renderTarget != null)
            {
                Context.ClearRenderTargetView(_renderTarget, new SharpDX.Mathematics.Interop.RawColor4()
                {
                    R = color.R / 255.0f,
                    G = color.G / 255.0f,
                    B = color.B / 255.0f,
                    A = color.A / 255.0f,
                });
            }
        }

        public override void Dispose()
        {
            _renderTarget?.Dispose();
            _device.Dispose();
        }
    }
}
