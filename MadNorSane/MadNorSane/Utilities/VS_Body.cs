using Microsoft.Xna.Framework;
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

        static public int to_pixels(int meters)
        {
            return (int)meters * (int)_scale;
        }
        static public int to_meters(int pixels)
        {
            return (int)pixels / (int)_scale;
        }

        internal static Vector3 to_meters(Vector3 vector)
        {
            return vector / _scale;
        }

        internal static Vector3 to_pixels(Vector3 vector)
        {
            return vector * _scale;
        }
    }
    class VS_Body
    {
        
    }
}
