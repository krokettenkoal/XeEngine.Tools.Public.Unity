using System.Drawing;

namespace Xe.Drawing
{
	public partial class DrawingNull : Drawing
	{
		private Filter _filter = Filter.Nearest;

		public override ISurface Surface { get => null; set { } }
		public override Filter Filter
		{
			get => _filter;
			set => _filter = value;
		}

		public override void Flush()
		{
			
		}

		public override void Clear(Color color)
		{
		}

		public override void Dispose()
		{
		}

		public override void DrawRectangle(RectangleF rect, Color color, float width = 1) { }

		public override void FillRectangle(RectangleF rect, Color color) { }

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip) { }

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None) { }

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None) { }
	}
}
