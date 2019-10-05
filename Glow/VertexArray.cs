using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class VertexArray : GLObject {

        public VertexArray() : base(GL.GenVertexArray()) {

        }

        public void AttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
            GL.BindVertexArray(Handle);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
            GL.BindVertexArray(NullHandle);
        }

        public void SetBuffer<T>(BufferTarget target, Buffer<T> buffer) where T : struct {
            GL.BindVertexArray(Handle);
            GL.BindBuffer(target, buffer.Handle);
            GL.BindVertexArray(NullHandle);
        }

        public void DrawArrays(PrimitiveType type, int first, int count) {
            GL.BindVertexArray(Handle);
            GL.DrawArrays(type, first, count);
        }

        public void DrawElements(PrimitiveType type, int count, DrawElementsType elmtype) {
            GL.BindVertexArray(Handle);
            GL.DrawElements(type, count, elmtype, 0);
        }

        public static void Unbind() {
            GL.BindVertexArray(NullHandle);
        }

        protected override void Dispose(bool manual) {
            if (manual && HasResources) {
                GL.DeleteVertexArray(Handle);
            }
        }
    }
}
