using System.Drawing;

namespace Xe.Drawing
{
    public partial class DrawingNull
    {
        public class CSurface : ISurface
        {
            public int Width => Size.Width;

            public int Height => Size.Height;

            public Size Size { get; }

            public PixelFormat PixelFormat { get; }

            public CSurface(int width, int height, PixelFormat pixelFormat)
            {
                Size = new Size(width, height);
                PixelFormat = PixelFormat;
            }

            public void Dispose()
            {
            }

            public IMappedResource Map()
            {
                return null;
            }

            public void Save(string filename)
            {
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type)
        {
            return new CSurface(width, height, pixelFormat);
        }

        public override ISurface CreateSurface(string filename, Color[] filterColors = null)
        {
            return null;
        }
    }
}
