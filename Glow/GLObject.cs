using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Glow {
    public abstract class GLObject : IDisposable {

        public const int null_handle = 0;

        public static readonly List<GLObject> Instances = new List<GLObject>();

        public static string ListInstances() {
            var str = "";
            //foreach (var item in from o in Instances
            //                     orderby o.GetType()
            //                     select o) {
            //    str += item.ToString() + "\n";
            //}

            for (int i = 0; i < Instances.Count; i++) {
                str += Instances[i].ToString() + "\n";
            }
            return str;
        }

        public static void check_glerror() {
            var code = GL.GetError();
            if (code != ErrorCode.NoError) {
                throw new Exception("OpenGL error: " + code);
            }
        }

        public int gl_handle { get; private set; } = null_handle;
        public bool has_resources => gl_handle != null_handle;

        // Note: default_wrapper is used to create fake object to represent default objects for example: Framebuffer.default_buffer
        internal GLObject(int h, bool default_wrapper = false) {
            this.gl_handle = h;
            if (!default_wrapper) { 
                if (gl_handle == null_handle) throw new Exception("Failed to generate " + this.GetType().Name);
                Instances.Add(this);
            }
        }

        public void Dispose() {
            this.Dispose(true);
            this.gl_handle = null_handle;
            Instances.Remove(this);
            GC.SuppressFinalize(this);
        }

        ~GLObject() {
            Dispose(false);
            // no need to remove this from Instances, since destructor will not be called when there is a refrence in Instances
        }

        protected abstract void Dispose(bool manual);

        public override string ToString() => GetType().Name + (has_resources ? $" ({gl_handle})" : " (null)");
        public override int GetHashCode() => gl_handle.GetHashCode();
    }
}
