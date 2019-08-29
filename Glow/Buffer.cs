using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public class Buffer<T> : GLObject where T : struct {

        public Buffer() : base(GL.GenBuffer()) {

        }

        public void Initialize(T[] data, BufferUsageHint hint) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, System.Runtime.InteropServices.Marshal.SizeOf<T>(), data, hint);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NullHandle);
        }

        public void Bind(BufferTarget target) => GL.BindBuffer(target, Handle);
        public static void Unbind(BufferTarget target) => GL.BindBuffer(target, NullHandle);

        protected override void Dispose(bool manual) {
            if(manual && HasResources) {
                GL.DeleteBuffer(Handle);
            }
        }
    }
}
