using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {

    public enum AttribType {
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

        private static int get_attrib_size(AttribType type) {
            switch (type) {
                case AttribType.Float:
                case AttribType.Double:
                case AttribType.Int:
                    return 1;
                case AttribType.Vec2:
                case AttribType.DVec2:
                case AttribType.IVec2:
                    return 2;
                case AttribType.Vec3:
                case AttribType.DVec3:
                case AttribType.IVec3:
                    return 3;
            }
            throw new Exception();
        }

        private static VertexAttribPointerType get_attrib_type(AttribType type) {
            switch (type) {
                case AttribType.Float:
                case AttribType.Vec2:
                case AttribType.Vec3:
                    return VertexAttribPointerType.Float;
                case AttribType.Double:
                case AttribType.DVec2:
                case AttribType.DVec3:
                    return VertexAttribPointerType.Double;
                case AttribType.Int:
                case AttribType.IVec2:
                case AttribType.IVec3:
                    return VertexAttribPointerType.Int;
            }
            throw new Exception();
        }


        public Vertexarray() : base(GL.GenVertexArray()) {
            
        }

        private static void validate_attribute_index(int attribute_index) {

        }


        public void set_attribute_pointer<T>(int attribute_index, AttribType type, Buffer<T> array_buffer, int stride, int offset = 0, bool normalized = false) where T : struct {
            bind();
            GL.EnableVertexAttribArray(attribute_index);
            array_buffer.bind(BufferTarget.ArrayBuffer);
            GL.VertexAttribPointer(attribute_index, get_attrib_size(type), get_attrib_type(type), normalized, stride, offset);
            unbind();
        }

        public void attrib_pointer(int attribute_index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset) {
            bind();
            GL.EnableVertexAttribArray(attribute_index);
            GL.VertexAttribPointer(attribute_index, size, type, normalized, stride, offset);
            unbind();
        }
        public void attrib_pointer(int attribute_index, AttribType type, bool normalized, int stride, int offset) => attrib_pointer(attribute_index, get_attrib_size(type), get_attrib_type(type), normalized, stride, offset);
        public void attrib_pointer(int attribute_index, AttribType type, int stride, int offset) => attrib_pointer(attribute_index, type, false, stride, offset);


        public void set_buffer<T>(BufferTarget target, Buffer<T> buffer) where T : struct {
            bind();
            GL.BindBuffer(target, buffer.gl_handle);
            unbind();
        }
        public void set_elementbuffer<T>(Buffer<T> buffer) where T : struct {
            bind();
            buffer.bind(BufferTarget.ElementArrayBuffer);
            unbind();
        }


        public void draw_arrays(PrimitiveType type, int first, int count) {
            bind();
            GL.DrawArrays(type, first, count);
        }
        public void draw_elements(PrimitiveType type, int count, DrawElementsType elmtype) {
            bind();
            GL.DrawElements(type, count, elmtype, 0);
        }

        private void bind() => GL.BindVertexArray(gl_handle);
        public static void unbind() {
            GL.BindVertexArray(null_handle);
        }


        protected override void Dispose(bool manual) {
            if (manual && has_resources) {
                GL.DeleteVertexArray(gl_handle);
            }
        }
    }
}
