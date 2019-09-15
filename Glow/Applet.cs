using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public abstract class Applet {

        public readonly GameWindow Window;

        public float DeltaTime;

        public Applet() {
            Window = new GameWindow(1600, 900, GraphicsMode.Default, "");

            Window.Load += Window_Load;
            Window.Unload += Window_Unload;
            Window.RenderFrame += Window_RenderFrame;
            Window.UpdateFrame += Window_UpdateFrame;
            Window.Resize += Window_Resize;
        }

        public ShaderProgram CreateShader(string fragsrc, string vertsrc) {
            var f = new Shader(ShaderType.FragmentShader, fragsrc);
            var v = new Shader(ShaderType.VertexShader, vertsrc);
            var res = new ShaderProgram(f, v);
            f.Dispose();
            v.Dispose();
            return res;
        }

        public void Start() {
            Window.Run();
        }

        private void Window_Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, Window.Width, Window.Height);

            OnWindowResize();
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e) {
            DeltaTime = (float)e.Time;
            Update();
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Render();
            GL.Flush();
            Window.SwapBuffers();
        }

        private void Window_Unload(object sender, EventArgs e) {
            //TODO: do we want to dispose all GLObjects here, but whats the point if any

            Unload();
        }

        private void Window_Load(object sender, EventArgs e) {
            GL.ClearColor(.3f, .3f, .3f, 1);

            Load();
        }

        protected virtual void OnWindowResize() { }

        protected virtual void Load() { }
        protected virtual void Unload() { }

        protected virtual void Update() { }
        protected virtual void Render() { }

    }
}
