using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Framebuffer : GLObject {

        public readonly FramebufferTarget Target;

        public FramebufferStatus Status => GL.CheckNamedFramebufferStatus(Handle, Target);

        public Framebuffer(FramebufferTarget t = FramebufferTarget.Framebuffer) : base(GL.GenFramebuffer()) {
            Target = t;
        }

        public void Attach(Texture2D texture) {
            //GL.NamedFramebufferTexture(Handle, FramebufferAttachment.)
        }

        public void Bind() {
            GL.BindFramebuffer(Target, Handle);
        }

        public void Unbind() {
            GL.BindFramebuffer(Target, NullHandle);
        }

        public static void BindDefault(FramebufferTarget t) {
            GL.BindFramebuffer(t, NullHandle);
        }

        protected override void Dispose(bool manual) {
            if (HasResources && manual) {
                GL.DeleteFramebuffer(Handle);
            }
        }
    }
}
