using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    class Conversions
    {
        float _scale = 30;

        float to_pixels(float meters)
        {
            return meters * _scale;
        }
        float to_meters(float pixels)
        {
            return pixels / _scale;
        }
    }
    class VS_Body
    {
        
    }
}
