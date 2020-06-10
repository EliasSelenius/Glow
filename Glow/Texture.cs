using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {

    public enum Filter {
        Nearest = 9728,
        Linear = 9729
    }

    public enum WrapMode {
        Repeat = 10497,
        ClampToBorder = 33069,
        ClampToEdge = 33071,
        MirroredRepeat = 33648
    }

    public abstract class Texture : GLObject {

        private static readonly Dictionary<TextureUnit, Texture> bound_textures = new Dictionary<TextureUnit, Texture>();

        public abstract TextureTarget target { get; }

        #region filter

        // TODO: filter messes up bound_textures. v4.5 direct_state_access?

        public Filter min_filter {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }
        public Filter mag_filter {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }
        public Filter filter {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)value);
                GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)value);
                GL.BindTexture(target, null_handle);
            }
        }

        #endregion

        public Texture() : base(GL.GenTexture()) {

        }

        public void bind(TextureUnit unit) {
            if (bound_textures.ContainsKey(unit) && bound_textures[unit] == this) return; 

            GL.ActiveTexture(unit);
            GL.BindTexture(target, gl_handle);
            bound_textures[unit] = this;
        }

        public void unbind() {
            if (bound_textures.ContainsValue(this)) {
                var unit = bound_textures.Where(x => x.Value == this).First().Key;
                GL.ActiveTexture(unit);
                GL.BindTexture(target, null_handle);
                bound_textures.Remove(unit);
            }
        }

        public static void unbind(TextureUnit unit) {
            if (bound_textures.ContainsKey(unit)) bound_textures[unit].unbind();
        }

        protected override void Dispose(bool manual) {
            if (manual && has_resources) {
                unbind();
                GL.DeleteTexture(gl_handle);
            }
        }
    }
}
