using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Renderbuffer : GLObject, Framebuffer.IAttachment {

        public int width { get; private set; }
        public int height { get; private set; }
        public RenderbufferStorage internal_format { get; private set; }

        public Renderbuffer(RenderbufferStorage storage, int width, int height) : base(GL.GenRenderbuffer()) {
            internal_format = storage;
            resize(width, height);
        }

        public void resize(int w, int h) {
            width = w;
            height = h;
            bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, internal_format, width, height);
            unbind();
        }

        public void bind() {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, gl_handle);
        }

        public static void unbind() {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, null_handle);
        }

        protected override void Dispose(bool manual) {
            if (has_resources && manual) {
                GL.DeleteRenderbuffer(gl_handle);
            }
        }

    }
}
