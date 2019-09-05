using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Glow {
    public class ShaderProgram : GLObject {

        public ShaderProgram(params Shader[] shaders) : base(GL.CreateProgram()) {
            Link(shaders);
        }

        public void Use() => GL.UseProgram(Handle);

        private void Link(Shader[] shaders) {
            foreach (var shader in shaders) {
                GL.AttachShader(Handle, shader.Handle);
            }

            GL.LinkProgram(Handle);

            var info = GL.GetProgramInfoLog(Handle);
            if (!string.IsNullOrWhiteSpace(info)) {
                // TODO: log info somewhere
                Console.WriteLine(info);
            }

            foreach (var shader in shaders) {
                GL.DetachShader(Handle, shader.Handle);
            }
        }

        #region Uniforms

        public void SetInt(int loc, int value) => GL.ProgramUniform1(Handle, loc, value);
        public void SetInt(string name, int value) => GL.ProgramUniform1(Handle, GL.GetUniformLocation(Handle, name), value);

        public void SetFloat(int loc, float value) => GL.ProgramUniform1(Handle, loc, value);
        public void SetFloat(string name, float value) => GL.ProgramUniform1(Handle, GL.GetUniformLocation(Handle, name), value);

        public void SetVec2(int loc, float x, float y) => GL.ProgramUniform2(Handle, loc, x, y);
        public void SetVec2(string name, float x, float y) => GL.ProgramUniform2(Handle, GL.GetUniformLocation(Handle, name), x, y);

        public void SetVec3(int loc, float x, float y, float z) => GL.ProgramUniform3(Handle, loc, x, y, z);
        public void SetVec3(string name, float x, float y, float z) => GL.ProgramUniform3(Handle, GL.GetUniformLocation(Handle, name), x, y, z);

        public void SetVec4(int loc, float x, float y, float z, float w) => GL.ProgramUniform4(Handle, loc, x, y, z, w);
        public void SetVec4(string name, float x, float y, float z, float w) => GL.ProgramUniform4(Handle, GL.GetUniformLocation(Handle, name), x, y, z, w);

        #region SetMat4

        public void SetMat4(int loc, float[] value, bool transpose = false) => GL.ProgramUniformMatrix4(Handle, loc, 16, transpose, value);
        public void SetMat4(string name, float[] value, bool transpose = false) => GL.ProgramUniformMatrix4(Handle, GL.GetUniformLocation(Handle, name), 16, transpose, value);
        public void SetMat4(int loc, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(Handle, loc, transpose, ref value);
        public void SetMat4(string name, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(Handle, GL.GetUniformLocation(Handle, name), transpose, ref value);
        #endregion

        #endregion

        public int GetAttribLocation(string name) => GL.GetAttribLocation(Handle, name);
        public int GetUniformLocation(string name) => GL.GetUniformLocation(Handle, name);

        protected override void Dispose(bool manual) {
            if (manual && HasResources) {
                GL.DeleteProgram(Handle);
            }
        }
    }
}
