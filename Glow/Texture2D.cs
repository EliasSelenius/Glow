using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {

    public enum Filter {
        Nearest = 9728,
        Linear = 9729
    }

    public class Texture2D : GLObject {

        public int Width { get; private set; }
        public int Height { get; private set; }

        public static TextureTarget Target => TextureTarget.Texture2D;

        public readonly Color32bit[,] Pixels;

        public Filter MinFilter {   
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }
        public Filter MagFilter {
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }
        public Filter Filter {
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)value);
                GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }

        public TextureWrapMode WrapS {
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }
        public TextureWrapMode WrapT {
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }
        public TextureWrapMode Wrap {
            set {
                GL.BindTexture(Target, Handle);
                GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)value);
                GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)value);
                GL.BindTexture(Target, NullHandle);
            }
        }

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
            GL.BindTexture(Target, Handle);

            // testing:
            //GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
            //GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, Pixels);
            if (genMipMap) {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            GL.BindTexture(Target, NullHandle);
        }



        public void Bind(TextureUnit unit) {
            GL.ActiveTexture(unit);
            GL.BindTexture(Target, Handle);
        }
        public static void Unbind() => GL.BindTexture(Target, NullHandle);

        protected override void Dispose(bool manual) {
            if (manual && HasResources) {
                GL.DeleteTexture(Handle);
            }
        }
    }
}
