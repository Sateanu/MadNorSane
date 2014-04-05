using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class Knight : Player
    {
        Knight(World _new_world)
        {
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1);
        }
    }
}
