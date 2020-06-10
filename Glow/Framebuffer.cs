using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nums;
using OpenTK.Graphics.OpenGL4;

namespace Glow {


    public class Framebuffer : GLObject {

        public interface IAttachment {
            int width { get; }
            int height { get; }
            void resize(int w, int h);
        }

        public static readonly Framebuffer default_buffer = new Framebuffer(0);

        //public int width { get; private set; }
        //public int height { get; private set; }

        public FramebufferStatus status => GL.CheckNamedFramebufferStatus(gl_handle, FramebufferTarget.Framebuffer);

        private readonly Dictionary<FramebufferAttachment, IAttachment> attachments = new Dictionary<FramebufferAttachment, IAttachment>();


        public Framebuffer(/*int w, int h*/) : base(GL.GenFramebuffer()) {
            //width = w;
            //height = h;
        }

        private Framebuffer(int gl) : base(gl, default_wrapper: false) { }

        public void attach(FramebufferAttachment attachment, Texture2D texture) {

            // NamedFramebufferTexture did not seem to work, probably a problem with opengl version
            //GL.NamedFramebufferTexture(Handle, attachment, texture.Handle, 0);

            bind();
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture.gl_handle, 0);
            unbind();
            attachments[attachment] = texture;
        }

        public void attach(FramebufferAttachment attachment, Renderbuffer renderbuffer) {
            //GL.NamedFramebufferRenderbuffer(Handle, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.Handle);

            bind();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, renderbuffer.gl_handle);
            unbind();
            attachments[attachment] = renderbuffer;
        }

        public void draw_buffers(params DrawBuffersEnum[] bufs) {
            bind();
            GL.DrawBuffers(bufs.Length, bufs);
            unbind();
        }

        public void resize(int w, int h) {
            //width = w;
            //height = h;
            foreach (var item in attachments) item.Value.resize(w, h);
        }

        public void bind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, gl_handle); // TODO: we may have to do a glViewport here
        public void unbind() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, null_handle);
        //public static void bind_default() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, NullHandle);

        /// <summary>
        /// Copies a region of one framebuffer to another
        /// </summary>
        /// <param name="source">The buffer to be copied from</param>
        /// <param name="src0x">The x coord of the first point in the source buffer</param>
        /// <param name="src0y">The y coord of the first point in the source buffer</param>
        /// <param name="src1x">The x coord of the second point in the source buffer</param>
        /// <param name="src1y">The y coord of the second point in the source buffer</param>
        /// <param name="destination">The buffer to copy to</param>
        /// <param name="dst0x">The x coord of the first point in the destination buffer</param>
        /// <param name="dst0y">The y coord of the first point in the destination buffer</param>
        /// <param name="dst1x">The x coord of the second point in the destination buffer</param>
        /// <param name="dst1y">The y coord of the second point in the destination buffer</param>
        /// <param name="mask">The bitwise OR of the flags indicating wich buffers are to be copied</param>
        /// <param name="filter">Specifies the interpolation to be applied if the image is stretched</param>
        public static void copy_region(Framebuffer source, int src0x, int src0y, int src1x, int src1y, Framebuffer destination, int dst0x, int dst0y, int dst1x, int dst1y, ClearBufferMask mask, Filter filter) {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, source.gl_handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, destination.gl_handle);

            GL.BlitFramebuffer(src0x, src0y, src1x, src1y, dst0x, dst0y, dst1x, dst1y, mask, (BlitFramebufferFilter)filter);
        }
        public static void copy_region(Framebuffer source, ivec2 src0, ivec2 src1, Framebuffer destination, ivec2 dst0, ivec2 dst1, ClearBufferMask mask, Filter filter) => copy_region(source, src0.x, src0.y, src1.x, src1.y, destination, dst0.x, dst0.y, dst1.x, dst1.y, mask, filter);
        public static void copy_region(Framebuffer source, ivec4 src_region, Framebuffer destination, ivec4 dst_region, ClearBufferMask mask, Filter filter) => copy_region(source, src_region.x, src_region.y, src_region.z, src_region.w, destination, dst_region.x, dst_region.y, dst_region.z, dst_region.w, mask, filter);
        //public static void copy(Framebuffer source, Framebuffer destination, ClearBufferMask mask, Filter filter) => copy_region(source, 0, 0, source.width, source.height, destination, 0, 0, destination.width, destination.height, mask, filter);


        protected override void Dispose(bool manual) {
            if (has_resources && manual) {
                GL.DeleteFramebuffer(gl_handle);
            }
        }
    }
}
