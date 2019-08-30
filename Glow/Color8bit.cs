using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow {
    public struct Color8bit {
        public byte Red, Green, Blue, Alpha;

        public Color8bit(byte rgb) {
            Red = Green = Blue = rgb; Alpha = byte.MaxValue;
        }
        public Color8bit(byte rgb, byte a) {
            Red = Green = Blue = rgb; Alpha = a;
        }
        public Color8bit(byte r, byte g, byte b) {
            Red = r; Green = g; Blue = b; Alpha = byte.MaxValue;
        }
        public Color8bit(byte r, byte g, byte b, byte a) {
            Red = r; Green = g; Blue = b; Alpha = a;
        }

        public static explicit operator Color8bit(Color32bit c32) 
            => new Color8bit((byte)(c32.Red * 255), (byte)(c32.Green * 255), (byte)(c32.Blue * 255), (byte)(c32.Alpha * 255));

        public static implicit operator Color8bit(System.Drawing.Color c) => new Color8bit(c.R, c.G, c.B, c.A);
        
        public static explicit operator Color8bit(OpenTK.Graphics.Color4 tkc) 
            => new Color8bit((byte)(tkc.R * 255f), (byte)(tkc.G * 255f), (byte)(tkc.B * 255f), (byte)(tkc.A * 255f));
        public static implicit operator OpenTK.Graphics.Color4(Color8bit c8) => new OpenTK.Graphics.Color4(c8.Red, c8.Green, c8.Blue, c8.Alpha); // TODO: check if this converts correctly

    }
}
