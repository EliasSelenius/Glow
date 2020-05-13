using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {

    public interface IAttachment {
        void resize(int w, int h);
    }

    public class Framebuffer : GLObject {

        public readonly FramebufferTarget target;

        public FramebufferStatus status => GL.CheckNamedFramebufferStatus(gl_handle, target);

        private readonly Dictionary<FramebufferAttachment, IAttachment> attachments = new Dictionary<FramebufferAttachment, IAttachment>();


        public Framebuffer(FramebufferTarget t = FramebufferTarget.Framebuffer) : base(GL.GenFramebuffer()) {
            target = t;
        }

        public void attach(FramebufferAttachment attachment, Texture2D texture) {

            // NamedFramebufferTexture did not seem to work, probably a problem with opengl version
            //GL.NamedFramebufferTexture(Handle, attachment, texture.Handle, 0);

            bind();
            GL.FramebufferTexture2D(target, attachment, TextureTarget.Texture2D, texture.gl_handle, 0);
            unbind();
            attachments[attachment] = texture;
        }

        public void attach(FramebufferAttachment attachment, Renderbuffer renderbuffer) {
            //GL.NamedFramebufferRenderbuffer(Handle, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.Handle);

            bind();
            GL.FramebufferRenderbuffer(target, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.gl_handle);
            unbind();
            attachments[attachment] = renderbuffer;
        }

        public void draw_buffers(params DrawBuffersEnum[] bufs) {
            bind();
            GL.DrawBuffers(bufs.Length, bufs);
            unbind();
        }

        public void resize(int w, int h) {
            foreach (var item in attachments) item.Value.resize(w, h);
        }

        public void bind() => GL.BindFramebuffer(target, gl_handle);
        public void unbind() => GL.BindFramebuffer(target, NullHandle);
        public static void bind_default(FramebufferTarget t) => GL.BindFramebuffer(t, NullHandle);
        

        protected override void Dispose(bool manual) {
            if (has_resources && manual) {
                GL.DeleteFramebuffer(gl_handle);
            }
        }
    }
}
