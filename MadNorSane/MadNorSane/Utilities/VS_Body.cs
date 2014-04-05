using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    public class Conversions
    {
        static float _scale = 30;

        static public float to_pixels(float meters)
        {
            return meters * _scale;
        }
        static public float to_meters(float pixels)
        {
            return pixels / _scale;
        }
    }
    class VS_Body
    {
        
    }
}
