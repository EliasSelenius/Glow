using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Buffer<T> : GLObject where T : struct {

        public readonly int element_bytesize = -1;

        public Buffer() : base(GL.GenBuffer()) {
            element_bytesize = System.Runtime.InteropServices.Marshal.SizeOf<T>();
        }

        public Buffer(T[] data, BufferUsageHint hint) : this() {
            bufferdata(data, hint);
        }

        public void bufferdata(T[] data, BufferUsageHint hint) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, gl_handle);
            GL.BufferData(BufferTarget.ArrayBuffer, element_bytesize * data.Length, data, hint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NullHandle);
        }

        // TODO: needs to be tested
        public void SubData(int offset, T[] data) {
            GL.NamedBufferSubData(gl_handle, new IntPtr(offset), System.Runtime.InteropServices.Marshal.SizeOf<T>() * data.Length, data);
        }

        // TODO: needs to be tested
        public T[] GetSubData(int offset, int size) {
            T[] data = new T[size / element_bytesize];
            GL.GetNamedBufferSubData(gl_handle, new IntPtr(offset), size, data);

            /*GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.GetBufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), size, data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NullHandle);*/

            return data;
        }

        public T[] GetData(int startIndex, int length) {
            T[] data = new T[length];
            GL.GetNamedBufferSubData(gl_handle, new IntPtr(startIndex * element_bytesize), length * element_bytesize, data);
            return data;
        }


        public void bind(BufferTarget target) => GL.BindBuffer(target, gl_handle);
        public static void unbind(BufferTarget target) => GL.BindBuffer(target, NullHandle);

        protected override void Dispose(bool manual) {
            if(manual && has_resources) {
                GL.DeleteBuffer(gl_handle);
            }
        }
    }
}
