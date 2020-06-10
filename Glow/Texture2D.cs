using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Texture2D : Texture, Framebuffer.IAttachment {

        public int width { get; private set; }
        public int height { get; private set; }

        public override TextureTarget target => TextureTarget.Texture2D;

        public color[,] pixels { get; private set; }

        public PixelInternalFormat internal_format = PixelInternalFormat.Rgba;


        #region wrap

        public WrapMode wrapS {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }
        public WrapMode wrapT {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }
        public WrapMode wrap {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)value);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }

        #endregion

        #region constructors


        public Texture2D(color[,] pixels, bool genMipmap = true, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba) {
            width = pixels.GetLength(0);
            height = pixels.GetLength(1);
            this.pixels = pixels;
            internal_format = internalFormat;
            apply(genMipmap);
            
        }

        public Texture2D(int w, int h) {
            width = w; height = h;
            pixels = new color[width, height];
        }

        public Texture2D(System.Drawing.Bitmap bitmap, bool genMipmap = true, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba) : this(bitmap.Width, bitmap.Height) {

            internal_format = internalFormat;

            /*
            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    Pixels[x, y] = bitmap.GetPixel(x, y);
                }
            }
            */


            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bytes = new byte[data.Stride * bitmap.Height];
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            var bpp = data.Stride / data.Width;

            for (int i = 0; i < bytes.Length; i += bpp) {
                var pixelindex = (i / bpp);
                pixels[pixelindex % bitmap.Width, pixelindex / bitmap.Width] = new color32(bytes[i + 2], bytes[i + 1], bytes[i], bpp == 4 ? bytes[i + 3] : (byte)255);
            }
            bitmap.UnlockBits(data);

            apply(genMipmap);
        }

        public Texture2D(string filename) : this((Bitmap)Image.FromFile(filename)) { }

        #endregion

        
        public void apply(bool genMipMap = true) {
            bind(TextureUnit.Texture0);
            GL.TexImage2D(target, 0, internal_format, width, height, 0, PixelFormat.Rgba, PixelType.Float, pixels);
            if (genMipMap) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            unbind();
        }

        public void resize(int w, int h) {
            width = w; height = h;
            pixels = new color[width, height];
            bind(TextureUnit.Texture0);
            GL.TexImage2D(target, 0, internal_format, width, height, 0, PixelFormat.Rgba, PixelType.Float, pixels);
            unbind();
        }

        // TODO: test this function
        public void load_pixels() {
            GL.BindTexture(target, gl_handle);
            GL.GetTexImage(target, 0, PixelFormat.Rgba, PixelType.Float, pixels);
            GL.BindTexture(target, null_handle);
        }

    }
}
