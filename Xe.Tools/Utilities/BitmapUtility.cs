using System;

namespace Xe.Tools.Utilities
{
    public static class BitmapUtility
	{
		public static void MakeTransparent_Bgra32(IntPtr data, int stride, int height, Xe.Graphics.Color[] colors)
		{
			foreach (var color in colors)
			{
				int to = color.b | (color.g << 8) | (color.r << 16);
				int from = to | (0xFF << 24);
				to = 0; // HACK
				MakeTransparent_Bpp32(data, stride, height, from, to);
			}
		}

		public static unsafe void MakeTransparent_Bpp24(IntPtr data, int stride, int height, int from, int to)
		{
			ushort srcab = (ushort)from;
			byte srcc = (byte)(from >> 16);

			ushort dstab = (ushort)to;
			byte dstc = (byte)(to >> 16);

			for (int i = 0; i < height; i++)
			{
				byte* p = (byte*)(data + i * stride);
				for (int j = 0; j < stride; j += 3, p += 3)
				{
					if (*(ushort*)p == srcab &&
						*(p + 2) == srcc)
					{
						*(ushort*)p = dstab;
						*(p + 2) = dstc;
					}
				}
			}
		}
		public static unsafe void MakeTransparent_Bpp32(IntPtr data, int stride, int height, int from, int to)
		{
			for (int i = 0; i < height; i++)
			{
				int* p = (int*)(data + i * stride);
				for (int j = 0; j < stride; j += 4, p++)
				{
					if (*p == from)
						*p = to;
				}
			}
		}
	}
}
