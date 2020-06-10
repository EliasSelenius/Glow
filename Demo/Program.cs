﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Glow;
using System.Diagnostics;
using System.Drawing;

namespace Demo {
    class Program {
        static void Main(string[] args) {

            GameWindow w = new GameWindow();

            ShaderProgram sp = null;

            Vertexarray vao = null;
            Buffer<float> vbo_pos = null;
            Buffer<float> vbo_uv = null;
            Buffer<uint> ebo = null;

            Texture2D texture = null;

            w.Load += (s, a) => {

                GL.ClearColor(0, 0, 0, 1);

                // setting up shader
                var frag = new Shader(ShaderType.FragmentShader, File.ReadAllText("frag.glsl"));
                var vert = new Shader(ShaderType.VertexShader, File.ReadAllText("vert.glsl"));
                sp = new ShaderProgram(frag, vert);
                frag.Dispose();
                vert.Dispose();


                // setting up buffers
                vbo_pos = Glow.Buffer.create(new float[] {
                    -.5f, -.5f, 0,
                    0, .5f, 0,
                    .5f, -.5f, 0
                });

                vbo_uv = Glow.Buffer.create(new float[] {
                    -0.5f, -0.5f,
                    .5f, 1.5f,
                    1.5f, -0.5f
                });

                ebo = Glow.Buffer.create(new uint[] {
                    0, 1, 2
                });

                // setting up vao
                int pos_loc = sp.get_attrib_location("pos");
                int uv_loc = sp.get_attrib_location("uv");

                vao = new Vertexarray();
                vao.set_elementbuffer(ebo);

                vao.set_attribute_pointer(pos_loc, AttribType.Vec3, vbo_pos, sizeof(float) * 3);
                vao.set_attribute_pointer(uv_loc, AttribType.Vec2, vbo_uv, sizeof(float) * 2);

                GLObject.check_glerror();

                // setting up texture
                /*
                var r = new Random();
                var pixels = new Color32bit[16, 16];
                for (int x = 0; x < 16; x++) {
                    for (int y = 0; y < 16; y++) {
                        if (x == 0 || y == 0 || x == 15 || y == 15) {
                            pixels[x, y] = new Color32bit(1.0f);
                        } else {
                            pixels[x, y] = new Color32bit((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                        }
                    }
                }
                texture = new Texture2D(pixels) {
                    Filter = Filter.Nearest,
                };*/
                var sw = new Stopwatch();
                sw.Start();
                texture = new Texture2D("Skybox_back.png");
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);

                foreach (var item in GLObject.Instances) {
                    Console.WriteLine(item);
                }

                
            };

            w.RenderFrame += (s, e) => {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                sp.use();
                texture.bind(TextureUnit.Texture0);
                //vao.DrawArrays(PrimitiveType.Triangles, 0, 9);
                vao.draw_elements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt);

                GL.Flush();
                w.SwapBuffers();

            };
            var r = new Random();

            
            w.UpdateFrame += (s, e) => {
                //texture.pixels[r.Next(texture.width), r.Next(texture.height)] = new color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                var h = r.Next(texture.height);
                for (int i = 0; i < texture.width; i++) {
                    texture.pixels[i, h] = new color(1);
                }

                texture.apply();
            };

            w.Resize += (s, e) => {
                GL.Viewport(0, 0, w.Width, w.Height);
            };

            w.Run();
        }
    }
}
