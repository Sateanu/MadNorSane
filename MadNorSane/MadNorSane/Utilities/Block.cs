using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using MadNorSane.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    class Block : Physics_object
    {
        Block(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));

            set_texture("block");
        }
    }
}
