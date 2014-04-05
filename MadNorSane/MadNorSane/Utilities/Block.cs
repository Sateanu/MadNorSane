using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using MadNorSane.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krypton;
namespace MadNorSane.Utilities
{
    public class Block : Physics_object
    {
       
        public Block(World _new_world,KryptonEngine krypton ,ContentManager _new_content, float x_coordinate, float y_coordinate,float width,float height,string type)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, width, height, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.UserData = type;
            this.width = width;
            this.height = height;
            var hull = ShadowHull.CreateRectangle(Conversions.to_pixels(new Vector2(width, height)));
            hull.Position.X =Conversions.to_pixels(x_coordinate);
            hull.Position.Y =Conversions.to_pixels(y_coordinate);

            krypton.Hulls.Add(hull);

            set_texture(@"Textures\knight");
        }
    }
}
