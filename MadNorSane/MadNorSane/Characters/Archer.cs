using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class Archer : Player
    {
        List<Body> arrows = new List<Body>(5);
        public Archer(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;

            my_body.CollisionGroup = -1;

            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = this;

           
            set_texture("archeranim");
        }
        private DateTime previousJump = DateTime.Now;   // time at which we previously jumped
        private const float jumpInterval = 1.0f;        // in seconds
        private Vector2 jumpForce = new Vector2(0, -5); // applied force when jumping

        public void Jump()
        {
            if ((DateTime.Now - previousJump).TotalSeconds >= jumpInterval)
            {
                my_body.ApplyLinearImpulse(ref jumpForce);
                previousJump = DateTime.Now;
            }
        }
        
        public bool atack()
        {
            
            return true;
        }
    }
}
