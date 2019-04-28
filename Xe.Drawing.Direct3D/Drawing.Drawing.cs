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

namespace Xe.Drawing
{
    using dx = SharpDX;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System;
    using System.Security;

	public partial class DrawingDirect3D : Drawing
	{
		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong count);

		[StructLayout(LayoutKind.Sequential)]
		private struct Vertex
		{
			public float X, Y;
			public float U, V;
			public ColorF Color;
		}

		private delegate void DrawSurfaceFunction(DrawingDirect3D drawing, ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip);

		private static readonly DrawSurfaceFunction[] DrawSurfaceFunctions = new DrawSurfaceFunction[4]
		{
			DrawSurfaceFlipNone,
			DrawSurfaceFlipX,
			DrawSurfaceFlipY,
			DrawSurfaceFlipXY
		};


		public override void DrawRectangle(RectangleF rect, Color color, float width = 1)
		{
		}

		public override void FillRectangle(RectangleF rect, Color color)
		{
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip)
		{
			DrawSurface(surface, src, dst, new ColorF(1.0f, 1.0f, 1.0f, 1.0f), flip);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip)
		{
			DrawSurface(surface, src, dst, new ColorF(1.0f, 1.0f, 1.0f, alpha), flip);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			SetTextureToDraw(surface);
			DrawSurfaceFunctions[(int)flip](this, surface, src, dst, color, flip);
		}

        public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color0, ColorF color1, ColorF color2, ColorF color3)
        {
            SetTextureToDraw(surface);
            var size = surface.Size;
            var viewport = _viewportSize;
            var index = RequestVertices(4);
            var buffer = _dataBuffer;

            buffer[index++] = new Vertex()
            {
                X = dst.Left / viewport.Width * +2.0f - 1.0f,
                Y = dst.Top / viewport.Height * -2.0f + 1.0f,
                U = (float)src.Left / size.Width,
                V = (float)src.Top / size.Height,
                Color = color0
            };
            buffer[index++] = new Vertex()
            {
                X = dst.Right / viewport.Width * +2.0f - 1.0f,
                Y = dst.Top / viewport.Height * -2.0f + 1.0f,
                U = (float)src.Right / size.Width,
                V = (float)src.Top / size.Height,
                Color = color1
            };
            buffer[index++] = new Vertex()
            {
                X = dst.Left / viewport.Width * +2.0f - 1.0f,
                Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
                U = (float)src.Left / size.Width,
                V = (float)src.Bottom / size.Height,
                Color = color2
            };
            buffer[index++] = new Vertex()
            {
                X = dst.Right / viewport.Width * +2.0f - 1.0f,
                Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
                U = (float)src.Right / size.Width,
                V = (float)src.Bottom / size.Height,
                Color = color3
            };
        }

        private static void DrawSurfaceFlipNone(DrawingDirect3D drawing, ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			var size = surface.Size;
			var viewport = drawing._viewportSize;
			var index = drawing.RequestVertices(4);
			var buffer = drawing._dataBuffer;

			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
		}
		private static void DrawSurfaceFlipX(DrawingDirect3D drawing, ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			var size = surface.Size;
			var viewport = drawing._viewportSize;
			var index = drawing.RequestVertices(4);
			var buffer = drawing._dataBuffer;

			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
		}
		private static void DrawSurfaceFlipY(DrawingDirect3D drawing, ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			var size = surface.Size;
			var viewport = drawing._viewportSize;
			var index = drawing.RequestVertices(4);
			var buffer = drawing._dataBuffer;

			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
		}
		private static void DrawSurfaceFlipXY(DrawingDirect3D drawing, ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip)
		{
			var size = surface.Size;
			var viewport = drawing._viewportSize;
			var index = drawing.RequestVertices(4);
			var buffer = drawing._dataBuffer;

			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Top / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Bottom / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Left / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Right / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
			buffer[index++] = new Vertex()
			{
				X = dst.Right / viewport.Width * +2.0f - 1.0f,
				Y = dst.Bottom / viewport.Height * -2.0f + 1.0f,
				U = (float)src.Left / size.Width,
				V = (float)src.Top / size.Height,
				Color = color
			};
		}

		private CSurface _prevSurface;
        private void SetTextureToDraw(ISurface surface)
        {
            if (_prevSurface != surface)
            {
                Flush();
                var internalSurface = surface as CSurface;
                Context.PixelShader.SetShaderResource(0, internalSurface?.ShaderResourceView);
                _prevSurface = internalSurface;
            }
        }

        private const int MaximumQuadsCount = 65536;
        private d3d.Buffer _vertexBuffer;
        private d3d.Buffer _indexBuffer;
        private Vertex[] _dataBuffer = new Vertex[MaximumQuadsCount];
        private int _pendingVerticesCount;

		private int RequestVertices(int count)
		{
			if (_pendingVerticesCount + count >= MaximumQuadsCount)
			{
				Flush();
			}

			var index = _pendingVerticesCount;
			_pendingVerticesCount += count;

			return index;
		}
		
        public override void Flush()
        {
            if (_pendingVerticesCount == 0)
                return;

            if (_vertexBuffer == null)
            {
                var vertexBufferDesc = new d3d.BufferDescription()
                {
                    SizeInBytes = MaximumQuadsCount * dx.Utilities.SizeOf<Vertex>(),
                    Usage = d3d.ResourceUsage.Dynamic,
                    BindFlags = d3d.BindFlags.VertexBuffer,
                    CpuAccessFlags = d3d.CpuAccessFlags.Write,
                    OptionFlags = 0,
                    StructureByteStride = 0
                };
                _vertexBuffer = d3d.Buffer.Create(Device, _dataBuffer, vertexBufferDesc);
                Context.InputAssembler.SetVertexBuffers(0, new d3d.VertexBufferBinding(_vertexBuffer, dx.Utilities.SizeOf<Vertex>(), 0));

                var indexBufferDesc = new d3d.BufferDescription()
                {
                    SizeInBytes = MaximumQuadsCount * 6 * sizeof(ushort),
                    Usage = d3d.ResourceUsage.Immutable,
                    BindFlags = d3d.BindFlags.IndexBuffer,
                    CpuAccessFlags = d3d.CpuAccessFlags.None,
                    OptionFlags = 0,
                    StructureByteStride = 0
                };
                uint[] indices = new uint[MaximumQuadsCount * 6];
                for (uint i = 0, nx = 0x00000000; i < MaximumQuadsCount * 6; nx += 4)
                {
                    indices[i++] = nx + 1;
                    indices[i++] = nx + 0;
                    indices[i++] = nx + 2;
                    indices[i++] = nx + 1;
                    indices[i++] = nx + 2;
                    indices[i++] = nx + 3;
                }
                _indexBuffer = d3d.Buffer.Create(Device, indices, indexBufferDesc);
                Context.InputAssembler.SetIndexBuffer(_indexBuffer, dxgi.Format.R32_UInt, 0);
            }
            else
            {
				var size = _pendingVerticesCount * dx.Utilities.SizeOf<Vertex>();
				var dataBox = Context.MapSubresource(_vertexBuffer, 0, d3d.MapMode.WriteDiscard, d3d.MapFlags.None);
                IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(_dataBuffer, 0);
                CopyMemory(dataBox.DataPointer, addr, (ulong)size);
                Context.UnmapSubresource(_vertexBuffer, 0);
            }
            
            Context.DrawIndexed(_pendingVerticesCount / 4 * 6, 0, 0);
            Context.Flush();
            _pendingVerticesCount = 0;
        }
    }
}
