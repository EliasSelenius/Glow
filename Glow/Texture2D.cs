using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Texture2D : GLObject {

        public int Width { get; private set; }
        public int Height { get; private set; }

        public readonly Color32bit[,] Pixels;

        public Texture2D(Color32bit[,] pixels) : base(GL.GenTexture()) {
            Width = pixels.GetLength(0);
            Height = pixels.GetLength(1);
            this.Pixels = pixels;
            Apply();
        }

        public Texture2D(int w, int h) : base(GL.GenTexture()) {
            Width = w; Height = h;
            Pixels = new Color32bit[Width, Height];
        }

        public Texture2D(System.Drawing.Bitmap bitmap) : this(bitmap.Width, bitmap.Height) {
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    Pixels[x, y] = bitmap.GetPixel(x, y);
                }
            }
            Apply();
        }

        public void Apply(bool genMipMap = true) {
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // testing:
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, Pixels);
            if (genMipMap) {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            GL.BindTexture(TextureTarget.Texture2D, NullHandle);
        }



        public void Bind(TextureUnit unit) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        public static void Unbind() => GL.BindTexture(TextureTarget.Texture2D, NullHandle);

        protected override void Dispose(bool manual) {
            if (manual && HasResources) {
                GL.DeleteTexture(Handle);
            }
        }
    }
}
