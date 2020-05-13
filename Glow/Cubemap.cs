using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {
    public class Cubemap : Texture {

        public override TextureTarget target => TextureTarget.TextureCubeMap;

        #region wrap

        public WrapMode wrapS {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)value);
                GL.BindTexture(target, NullHandle);
            }
        }
        public WrapMode wrapT {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)value);
                GL.BindTexture(target, NullHandle);
            }
        }
        public WrapMode wrapR {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapR, (int)value);
                GL.BindTexture(target, NullHandle);
            }
        }
        public WrapMode wrap {
            set {
                GL.BindTexture(target, gl_handle);
                GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)value);
                GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)value);
                GL.TexParameter(target, TextureParameterName.TextureWrapR, (int)value);
                GL.BindTexture(target, NullHandle);
            }
        }

        #endregion

        public Cubemap() {

        }
    }
}
