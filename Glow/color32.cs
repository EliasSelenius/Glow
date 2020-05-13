using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Glow {

    [StructLayout(LayoutKind.Sequential)]
    public struct color32 {
        public byte red, green, blue, alpha;

        public color32(byte rgb) {
            red = green = blue = rgb; alpha = byte.MaxValue;
        }
        public color32(byte rgb, byte a) {
            red = green = blue = rgb; alpha = a;
        }
        public color32(byte r, byte g, byte b) {
            red = r; green = g; blue = b; alpha = byte.MaxValue;
        }
        public color32(byte r, byte g, byte b, byte a) {
            red = r; green = g; blue = b; alpha = a;
        }

        public static explicit operator color32(color c32) 
            => new color32((byte)(c32.red * 255), (byte)(c32.green * 255), (byte)(c32.blue * 255), (byte)(c32.alpha * 255));

        public static implicit operator color32(System.Drawing.Color c) => new color32(c.R, c.G, c.B, c.A);
        
        public static explicit operator color32(OpenTK.Graphics.Color4 tkc) 
            => new color32((byte)(tkc.R * 255f), (byte)(tkc.G * 255f), (byte)(tkc.B * 255f), (byte)(tkc.A * 255f));
        public static implicit operator OpenTK.Graphics.Color4(color32 c8) => new OpenTK.Graphics.Color4(c8.red, c8.green, c8.blue, c8.alpha); // TODO: check if this converts correctly

    }
}
