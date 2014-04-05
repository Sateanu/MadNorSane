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
        List<Arrow> arrows = new List<Arrow>(5);
        public Archer(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;

            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = this;
            my_body.CollisionGroup = -1;
           
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
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
            foreach (var arr in arrows)
                arr.Draw(spriteBatch);
        }
        public bool atack(Vector2 direction)
        {
            arrows.Add(new Arrow(my_world, _my_content, this, direction));
            return true;
        }
    }
}
