using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {
    public abstract class GLObject : IDisposable {

        public const int NullHandle = 0;

        public static readonly List<GLObject> Instances = new List<GLObject>();

        public int Handle { get; private set; } = NullHandle;
        public bool HasResources => Handle != NullHandle;

        public GLObject(int h) {
            this.Handle = h;
            Instances.Add(this);
        }

        public void Dispose() {
            this.Dispose(true);
            this.Handle = NullHandle;
            Instances.Remove(this);
            GC.SuppressFinalize(this);
        }

        ~GLObject() {
            Dispose(false);
        }

        protected abstract void Dispose(bool manual);

        public override string ToString() => GetType().Name + (HasResources ? $" ({Handle})" : " (null)");
        public override int GetHashCode() => Handle.GetHashCode();
    }
}
