﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class Archer : Physics_object
    {
        public Archer(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;

            my_body.FixedRotation = true;

            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = "player";

            set_texture("archer");
        }

        public bool atack(String _skill)
        {
            
            return true;
        }
    }
}
