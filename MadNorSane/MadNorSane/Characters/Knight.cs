using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class Knight : Physics_object
    {
        Knight(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;

            set_texture("knight");
        }

        public bool VS_OnCollision(Fixture fixA, Fixture fixB, Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {
                if (fixA.Body.UserData.GetType().IsSubclassOf(typeof(Sword)))
                {
                    if (fixB.Body.UserData == "wall" && touched_sides.X < 0)
                    {
                        Console.WriteLine("Am lovit wall");
                        return false;
                    }

                    if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))// && touched_sides.X != 0)
                    {
                        Console.WriteLine("Am lovit player");
                        return false;
                    }
                }
            }
            return true;
        }
    }

    class Sword
    {

    }
}
