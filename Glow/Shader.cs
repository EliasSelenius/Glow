using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Shader : GLObject {

        public Shader(ShaderType type, string src) : base(GL.CreateShader(type)) {
            GL.ShaderSource(Handle, src);
            GL.CompileShader(Handle);

            var info = GL.GetShaderInfoLog(Handle);
            if (!string.IsNullOrWhiteSpace(info)) {
                // TODO: log info somewhere
                Console.WriteLine(info);
            }
        }

        protected override void Dispose(bool manual) {
            if (manual && HasResources) {
                GL.DeleteShader(Handle);
            }
        }
    }
}
