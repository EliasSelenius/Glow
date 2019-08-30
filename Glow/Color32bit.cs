using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {
    public struct Color32bit {
        public float Red, Green, Blue, Alpha;

        public Color32bit(float rgb) {
            Red = Green = Blue = rgb; Alpha = 1f;
        }
        public Color32bit(float rgb, float a) {
            Red = Green = Blue = rgb; Alpha = a;
        }
        public Color32bit(float r, float g, float b) {
            Red = r; Green = g; Blue = b; Alpha = 1f;
        }
        public Color32bit(float r, float g, float b, float a) {
            Red = r; Green = g; Blue = b; Alpha = a;
        }

        public static implicit operator Color32bit(Color8bit c8) => new Color32bit(c8.Red / 255f, c8.Green / 255f, c8.Blue / 255f, c8.Alpha / 255f);

        public static implicit operator Color32bit(System.Drawing.Color c) => new Color32bit(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

        public static implicit operator Color32bit(OpenTK.Graphics.Color4 tkc) => new Color32bit(tkc.R, tkc.G, tkc.B, tkc.A);
        public static implicit operator OpenTK.Graphics.Color4(Color32bit c32) => new OpenTK.Graphics.Color4(c32.Red, c32.Green, c32.Blue, c32.Alpha);
    }
}
