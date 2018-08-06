namespace Xe.Graphics
{
	public class ColorF
	{
		public float r, g, b, a;

		public ColorF()
		{ }

		public ColorF(float r, float g, float b) :
			this(r, g, b, 255)
		{ }

		public ColorF(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public ColorF(Color color)
		{
			r = color.r / 255.0f;
			g = color.g / 255.0f;
			b = color.b / 255.0f;
			a = color.a / 255.0f;
		}

		public Color ToColor()
		{
			var ri = (byte)(Math.Max(0.0f, Math.Min(1.0f, r)) * 255.0f);
			var gi = (byte)(Math.Max(0.0f, Math.Min(1.0f, g)) * 255.0f);
			var bi = (byte)(Math.Max(0.0f, Math.Min(1.0f, b)) * 255.0f);
			var ai = (byte)(Math.Max(0.0f, Math.Min(1.0f, a)) * 255.0f);

			return new Color(ri, gi, bi, ai);
		}
	}
}
