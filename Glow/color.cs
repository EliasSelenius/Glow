using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Glow {

    [StructLayout(LayoutKind.Sequential)]
    public struct color {
        public float red, green, blue, alpha;

        public color(float rgb) {
            red = green = blue = rgb; alpha = 1f;
        }
        public color(float rgb, float a) {
            red = green = blue = rgb; alpha = a;
        }
        public color(float r, float g, float b) {
            red = r; green = g; blue = b; alpha = 1f;
        }
        public color(float r, float g, float b, float a) {
            red = r; green = g; blue = b; alpha = a;
        }

        public static implicit operator color(color32 c8) => new color(c8.red / 255f, c8.green / 255f, c8.blue / 255f, c8.alpha / 255f);

        public static implicit operator color(System.Drawing.Color c) => new color(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

        public static implicit operator color(OpenTK.Graphics.Color4 tkc) => new color(tkc.R, tkc.G, tkc.B, tkc.A);
        public static implicit operator OpenTK.Graphics.Color4(color c32) => new OpenTK.Graphics.Color4(c32.red, c32.green, c32.blue, c32.alpha);
    }
}
