using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {

    public enum Attribtype {
        Float,
        Double,
        Int,

        Vec2,
        DVec2,
        IVec2,

        Vec3,
        DVec3,
        IVec3,
    }

    public class Vertexarray : GLObject {

        private static int get_attrib_size(Attribtype type) {
            switch (type) {
                case Attribtype.Float:
                case Attribtype.Double:
                case Attribtype.Int:
                    return 1;
                case Attribtype.Vec2:
                case Attribtype.DVec2:
                case Attribtype.IVec2:
                    return 2;
                case Attribtype.Vec3:
                case Attribtype.DVec3:
                case Attribtype.IVec3:
                    return 3;
            }
            throw new Exception();
        }

        private static VertexAttribPointerType get_attrib_type(Attribtype type) {
            switch (type) {
                case Attribtype.Float:
                case Attribtype.Vec2:
                case Attribtype.Vec3:
                    return VertexAttribPointerType.Float;
                case Attribtype.Double:
                case Attribtype.DVec2:
                case Attribtype.DVec3:
                    return VertexAttribPointerType.Double;
                case Attribtype.Int:
                case Attribtype.IVec2:
                case Attribtype.IVec3:
                    return VertexAttribPointerType.Int;
            }
            throw new Exception();
        }


        public Vertexarray() : base(GL.GenVertexArray()) {

        }

        public void attrib_pointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
            GL.BindVertexArray(gl_handle);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
            GL.BindVertexArray(NullHandle);
        }

        public void attrib_pointer(int index, Attribtype type, bool normalized, int stride, int offset) => attrib_pointer(index, get_attrib_size(type), get_attrib_type(type), normalized, stride, offset);
        public void attrib_pointer(int index, Attribtype type, int stride, int offset) => attrib_pointer(index, type, false, stride, offset);

        public void set_buffer<T>(BufferTarget target, Buffer<T> buffer) where T : struct {
            GL.BindVertexArray(gl_handle);
            GL.BindBuffer(target, buffer.gl_handle);
            GL.BindVertexArray(NullHandle);
        }

        public void draw_arrays(PrimitiveType type, int first, int count) {
            GL.BindVertexArray(gl_handle);
            GL.DrawArrays(type, first, count);
        }

        public void draw_elements(PrimitiveType type, int count, DrawElementsType elmtype) {
            GL.BindVertexArray(gl_handle);
            GL.DrawElements(type, count, elmtype, 0);
        }

        public static void unbind() {
            GL.BindVertexArray(NullHandle);
        }

        protected override void Dispose(bool manual) {
            if (manual && has_resources) {
                GL.DeleteVertexArray(gl_handle);
            }
        }
    }
}
