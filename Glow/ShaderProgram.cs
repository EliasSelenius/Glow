using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK;

using Nums;

namespace Glow {
    public class ShaderProgram : GLObject {

        public static ShaderProgram create(string fragmentCode, string vertexCode) {
            var fs = new Shader(ShaderType.FragmentShader, fragmentCode);
            var vs = new Shader(ShaderType.VertexShader, vertexCode);
            var s = new ShaderProgram(fs, vs);
            fs.Dispose();
            vs.Dispose();
            return s;
        }

        private readonly Dictionary<string, int> uniform_loc_cache = new Dictionary<string, int>();

        public ShaderProgram(params Shader[] shaders) : base(GL.CreateProgram()) {
            link(shaders);
        }

        public void use() => GL.UseProgram(gl_handle);

        public void link(Shader[] shaders) {
            foreach (var shader in shaders) {
                GL.AttachShader(gl_handle, shader.gl_handle);
            }

            GL.LinkProgram(gl_handle);

            var info = GL.GetProgramInfoLog(gl_handle);
            if (!string.IsNullOrWhiteSpace(info)) {
                // TODO: log info somewhere
                Console.WriteLine(info);
            }

            foreach (var shader in shaders) {
                GL.DetachShader(gl_handle, shader.gl_handle);
            }
        }



        #region bool uniforms
        public void set_bool(int loc, bool value) => GL.ProgramUniform1(gl_handle, loc, value ? 1 : 0);
        public void set_bool(string name, bool value) => GL.ProgramUniform1(gl_handle, get_uniform_location(name), value ? 1 : 0);
        #endregion

        #region int uniforms
        public void set_int(int loc, int value) => GL.ProgramUniform1(gl_handle, loc, value);
        public void set_int(string name, int value) => GL.ProgramUniform1(gl_handle, get_uniform_location(name), value);
        #endregion

        #region float uniforms
        public void SetFloat(int loc, float value) => GL.ProgramUniform1(gl_handle, loc, value);
        public void SetFloat(string name, float value) => GL.ProgramUniform1(gl_handle, get_uniform_location(name), value);
        #endregion

        #region vec2 uniforms
        public void set_vec2(int loc, float x, float y) => GL.ProgramUniform2(gl_handle, loc, x, y);
        public void set_vec2(string name, float x, float y) => GL.ProgramUniform2(gl_handle, get_uniform_location(name), x, y);
        public void set_vec2(int loc, Vector2 vec) => GL.ProgramUniform2(gl_handle, loc, vec);
        public void set_vec2(string name, Vector2 vec) => GL.ProgramUniform2(gl_handle, get_uniform_location(name), vec);
        public void set_vec2(int loc, vec2 vec) => GL.ProgramUniform2(gl_handle, loc, vec.x, vec.y);
        public void set_vec2(string name, vec2 vec) => GL.ProgramUniform2(gl_handle, get_uniform_location(name), vec.x, vec.y);
        #endregion

        #region vec3 uniforms
        public void set_vec3(int loc, float x, float y, float z) => GL.ProgramUniform3(gl_handle, loc, x, y, z);
        public void set_vec3(string name, float x, float y, float z) => GL.ProgramUniform3(gl_handle, get_uniform_location(name), x, y, z);
        public void set_vec3(int loc, Vector3 vec) => GL.ProgramUniform3(gl_handle, loc, vec);
        public void set_vec3(string name, Vector3 vec) => GL.ProgramUniform3(gl_handle, get_uniform_location(name), vec);
        public void set_vec3(int loc, vec3 vec) => GL.ProgramUniform3(gl_handle, loc, vec.x, vec.y, vec.z);
        public void set_vec3(string name, vec3 vec) => GL.ProgramUniform3(gl_handle, get_uniform_location(name), vec.x, vec.y, vec.z);
        #endregion

        #region vec4 uniforms
        public void set_vec4(int loc, float x, float y, float z, float w) => GL.ProgramUniform4(gl_handle, loc, x, y, z, w);
        public void set_vec4(string name, float x, float y, float z, float w) => GL.ProgramUniform4(gl_handle, get_uniform_location(name), x, y, z, w);
        public void set_vec4(int loc, Vector4 vec) => GL.ProgramUniform4(gl_handle, loc, vec);
        public void set_vec4(string name, Vector4 vec) => GL.ProgramUniform4(gl_handle, get_uniform_location(name), vec);
        public void set_vec4(int loc, vec4 vec) => GL.ProgramUniform4(gl_handle, loc, vec.x, vec.y, vec.z, vec.w);
        public void set_vec4(string name, vec4 vec) => GL.ProgramUniform4(gl_handle, get_uniform_location(name), vec.x, vec.y, vec.z, vec.w);
        #endregion

        #region mat4 uniforms

        public void set_mat4(int loc, float[] value, bool transpose = false) => GL.ProgramUniformMatrix4(gl_handle, loc, 16, transpose, value);
        public void set_mat4(string name, float[] value, bool transpose = false) => GL.ProgramUniformMatrix4(gl_handle, get_uniform_location(name), 16, transpose, value);
        public void set_mat4(int loc, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(gl_handle, loc, transpose, ref value);
        public void set_mat4(string name, Matrix4 value, bool transpose = false) => GL.ProgramUniformMatrix4(gl_handle, get_uniform_location(name), transpose, ref value);

        /*
        void d(mat4 m) {
            GL.ProgramUniformMatrix4(gl_handle, 0, 16, false, ref m.row1.x);
        }*/

        #endregion


        public int get_attrib_location(string name) => GL.GetAttribLocation(gl_handle, name);
        public int get_uniform_location(string name) {
            if (uniform_loc_cache.ContainsKey(name)) return uniform_loc_cache[name];

            var l = GL.GetUniformLocation(gl_handle, name);
            uniform_loc_cache[name] = l;
            return l;
        }

        protected override void Dispose(bool manual) {
            if (manual && has_resources) GL.DeleteProgram(gl_handle);
        }
    }
}
