using FarseerPhysics.Dynamics;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    public class Skills
    {
        public String arrow_shoot = "arrow shoot";
        public String sword_swing = "sword swing";
        public String energy_wave = "energy wave";
        public String axe_throw = "axe throw";
    }

    public abstract class Physics_object
    {
        float move_speed = 10, jump_speed = 10, health_points = 100, mana_points = 100;
        float width = 1, height = 1;
        float x_coordinate = 0, y_coordinate = 0;

        public float Get_X()
        {
            return my_body.Position.X;
        }
        public float Get_Y()
        {
            return my_body.Position.Y;
        }

        public World my_world = null;
        public Body my_body = null;
        public Texture2D my_texture = null;
        public ContentManager _my_content = null;

        public void set_texture(String name)
        {
            try
            {
                my_texture = _my_content.Load<Texture2D>(@"Textures\" + name);
            }
            catch
            {
                my_texture = _my_content.Load<Texture2D>(@"Textures\place_holder");
            }
        }



        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float MP
        {
            get { return mana_points; }
            set { mana_points = value; }
        }

        public float HP
        {
            get { return health_points; }
            set { health_points = value; }
        }

        public float Jump_speed
        {
            get { return jump_speed; }
            set { jump_speed = value; }
        }

        public float Move_speed
        {
            get { return move_speed; }
            set { move_speed = value; }
        }
        bool has_weapon = true;

        public virtual bool atack(String _skill)
        {
            return true;
        }
        public bool use_buff(String _skill)
        {
            return true;
        }
        public void jump()
        {

        }
        public void move_on_ground()
        {
            /*if (ButtonJump && canJumpVertically)
            {
                alSourcePlay(SourceSoundJump);

                canJumpVertically = false;

                if (worldGravity.y != 0)
                {
                    velocity.y = speedJumpUp * -sgn(worldGravity.y);
                }
                else
                {
                    velocity.x = speedJumpUp * -sgn(worldGravity.x);
                }
                ButtonJump = false;
                body->SetLinearVelocity(velocity);
                return;
            }

            // Run on ground
            if (!itWalks(body, T))
            {
                if (worldGravity.y != 0)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.y = 0;
                }
                body->SetLinearVelocity(velocity);
            }*/
        }
        public void move_in_air()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y),
                                                        (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height)), null,Color.White,0f,origin,SpriteEffects.None,0f);
        }

        public Vector2 origin { get { return new Vector2(my_texture.Width / 2, my_texture.Height / 2); } }
    }
}
