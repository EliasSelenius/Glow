using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Glow;

namespace Demo {
    class Program {
        static void Main(string[] args) {

            GameWindow w = new GameWindow();

            VertexArray vao = null;

            w.Load += (s, a) => {
                Buffer<float> b = new Buffer<float>();
                b.Initialize(new float[] { 1 }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

                var b2 = new Buffer<uint>();
                b2.Initialize(new uint[] { }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

                vao = new VertexArray();
                vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, b);
                vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, b2);


                foreach (var item in GLObject.Instances) {
                    Console.WriteLine(item);
                }


            };

            w.RenderFrame += (s, e) => {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                vao.DrawArrays(PrimitiveType.Triangles, 0, 3);
                GL.Flush();
                w.SwapBuffers();

            };

            w.Run();
        }
    }
}
