using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {
    public abstract class GLObject : IDisposable {

        public const int NullHandle = 0;

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

        public int gl_handle { get; private set; } = NullHandle;
        public bool has_resources => gl_handle != NullHandle;

        public GLObject(int h) {
            this.gl_handle = h;
            Instances.Add(this);
        }

        public void Dispose() {
            this.Dispose(true);
            this.gl_handle = NullHandle;
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
