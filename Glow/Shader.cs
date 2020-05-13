using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Shader : GLObject {

        public Shader(ShaderType type, string src) : base(GL.CreateShader(type)) {
            GL.ShaderSource(gl_handle, src);
            GL.CompileShader(gl_handle);

            var info = GL.GetShaderInfoLog(gl_handle);
            if (!string.IsNullOrWhiteSpace(info)) {
                // TODO: log info somewhere
                Console.WriteLine(info);
            }
        }

        protected override void Dispose(bool manual) {
            if (manual && has_resources) {
                GL.DeleteShader(gl_handle);
            }
        }
    }
}
