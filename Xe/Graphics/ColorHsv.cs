using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Graphics
{
    public class ColorHsv
    {
		public float h, s, v, a;

		public ColorHsv()
		{ }

		public ColorHsv(ColorF color)
		{
			float min, max, delta, hue;
			if (color.r > color.g)
			{
				min = color.g;
				max = color.r;
			}
			else
			{
				min = color.r;
				max = color.g;
			}
			if (min > color.b) min = color.b;
			if (max < color.b) max = color.b;
			delta = max - min;

			if (delta != 0.0f)
			{
				if (color.r == max)
					hue = (color.g - color.b) / delta;
				else if (color.g == max)
					hue = 2.0f + (color.b - color.r) / delta;
				else if (color.b == max)
					hue = 4.0f + (color.r - color.g) / delta;
				else
					hue = 0.0f;
			}
			else
				hue = 0.0f;
			if (hue < 0.0f)
				hue += 6.0f;

			h = hue * 60.0f;
			s = (max != 0.0f) ? delta / max : 0.0f;
			v = max;
			a = color.a;
		}

		public ColorF ToColorF()
		{
			float r, g, b;
			float sx = Math.Min(1.0f, s);
			if (sx < 0.0000001f)
				return new ColorF(v, v, v, a);
			float hx = (h % 360.0f) / 60.0f;
			float vx = Math.Min(1.0f, v);

			float chroma = sx * vx;
			float x = chroma * (1.0f - Math.Abs(((hx % 2.0f) - 1.0f)));
			float m = vx - chroma;

			switch ((int)(hx) % 6)
			{
				case 0:
					r = chroma;
					g = x;
					b = 0.0f;
					break;
				case 1:
					r = x;
					g = chroma;
					b = 0.0f;
					break;
				case 2:
					r = 0.0f;
					g = chroma;
					b = x;
					break;
				case 3:
					r = 0.0f;
					g = x;
					b = chroma;
					break;
				case 4:
					r = x;
					g = 0.0f;
					b = chroma;
					break;
				case 5:
					r = chroma;
					g = 0.0f;
					b = x;
					break;
				default:
					r = 0.0f;
					g = 0.0f;
					b = 0.0f;
					break;
			}
			return new ColorF(r + m, g + m, b + m, a);
		}
    }
}
