using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Glow;

namespace Demo {
    class Program {
        static void Main(string[] args) {

            GameWindow w = new GameWindow();

            ShaderProgram sp = null;

            VertexArray vao = null;
            Buffer<float> vbo = null;
            Buffer<uint> ebo = null;
             
            w.Load += (s, a) => {

                GL.ClearColor(0, 0, 0, 1);

                var frag = new Shader(ShaderType.FragmentShader, File.ReadAllText("frag.glsl"));
                var vert = new Shader(ShaderType.VertexShader, File.ReadAllText("vert.glsl"));
                sp = new ShaderProgram(frag, vert);
                frag.Dispose();
                vert.Dispose();

                vbo = new Buffer<float>();
                vbo.Initialize(new float[] {
                    -.5f, -.5f, 0, 0, 0,
                    0, .5f, 0, .5f, 1,
                    .5f, -.5f, 0, 1, 0
                }, BufferUsageHint.StaticDraw);

                ebo = new Buffer<uint>();
                ebo.Initialize(new uint[] {
                    0, 1, 2
                }, BufferUsageHint.StaticDraw);

                vao = new VertexArray();
                vao.SetBuffer(BufferTarget.ArrayBuffer, vbo);
                vao.SetBuffer(BufferTarget.ElementArrayBuffer, ebo);

                vao.AttribPointer(sp.GetAttribLocation("pos"), 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
                vao.AttribPointer(sp.GetAttribLocation("uv"), 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);


                foreach (var item in GLObject.Instances) {
                    Console.WriteLine(item);
                }

                
            };

            w.RenderFrame += (s, e) => {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                sp.Use();
                //vao.DrawArrays(PrimitiveType.Triangles, 0, 9);
                vao.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt);

                GL.Flush();
                w.SwapBuffers();

            };

            w.Resize += (s, e) => {
                GL.Viewport(0, 0, w.Width, w.Height);
            };

            w.Run();
        }
    }
}
