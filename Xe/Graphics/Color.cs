namespace Xe.Graphics
{
	public class Color
	{
		public byte r, g, b, a;

		public Color()
		{ }

		public Color(byte r, byte g, byte b) :
			this(r, g, b, 255)
		{ }

		public Color(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
	}
}
