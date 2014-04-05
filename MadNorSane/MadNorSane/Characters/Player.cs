﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
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
        float turnMultiplier = 1;
        float speed_float = 0.05f;

        public bool btn_jump = false, btn_move_left = false, btn_move_right = false, btn_atack1 = false, btn_atack2 = false;
        public bool can_jump = false, can_move_left = false, can_move_right = false, can_atack1 = false, can_atack2 = false;

        public Vector2 velocity = Vector2.Zero;

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
            if (btn_jump && can_jump)
            {
                can_jump = false;

                velocity.Y = jump_speed;

                my_body.ApplyLinearImpulse(velocity);
                //my_body.LinearVelocity = velocity;
                return;
            }

            // Run on ground
            if (!itWalks(speed_float))
            {
                velocity.Y = 0;
                my_body.LinearVelocity = velocity;
            }
        }

        int get_wanted_direction_of_moving()
        {
            int sign = 0;
            if (btn_move_right && btn_move_left == false)
            {
                sign = 1;
            }
            else
                if (btn_move_right == false && btn_move_left)
                {
                    sign = -1;
                }
            return sign;
        }
        int get_current_direction_of_moving()
        {
            int sign = 0;
            if (my_body.LinearVelocity.X < 0)
            {
                sign = -1;
            }
            else
                if (my_body.LinearVelocity.X > 0)
                {
                    sign = 1;
                }
            return sign;
        }

        bool itWalks(float T)
        {
            int sign_wanted = get_wanted_direction_of_moving();
            float sign_current = get_current_direction_of_moving();
            
            float new_speed = move_speed;

            if (sign_current != 0 && sign_current != sign_wanted)
            {
                new_speed *= turnMultiplier;
            }

            float speedVelocity = 0;
            speedVelocity = velocity.X;

            //verific daca viteza curenta este mai mica decat viteza maxima, altfel nu mai maresc viteza
            if (sign_wanted < 0)
            {
                if (speedVelocity > 0)
                {
                    speedVelocity *= -1 * T;
                }

                if (speedVelocity > -move_speed)
                {
                    speedVelocity += new_speed * sign_wanted * T;
                }
                else
                {
                    speedVelocity = -move_speed;
                }
            }
            else
                if (sign_wanted > 0)
                {
                    if (speedVelocity < 0)
                    {
                        speedVelocity *= -1 * 0.1f;
                    }

                    if (speedVelocity < move_speed)
                    {
                        speedVelocity += new_speed * sign_wanted * T;
                    }
                    else
                    {
                        speedVelocity = move_speed;
                    }
                }

            float velChangeX = speedVelocity - my_body.LinearVelocity.X;
            float impulseX = my_body.Mass * velChangeX;
            my_body.ApplyLinearImpulse(new Vector2(impulseX, 0), my_body.WorldCenter);
            velocity.X = speedVelocity;


            return true;
        }


        public void move_in_air()
        {
        }


        public bool VS_OnCollision(Fixture fixA, Fixture fixB, Contact contact)
        {
            if (contact.IsTouching)
            {
                if (fixA.Body.UserData == "player" && fixB.Body.UserData == "block")
                {
                    can_jump = true;
                    Console.WriteLine("is on ground");
                }
                else
                {
                    can_jump = false;
                    Console.WriteLine("is NOT on ground");
                }
            }
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y),
                                                        (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height)), null,Color.White,0f,origin,SpriteEffects.None,0f);
        }

        public Vector2 origin { get { return new Vector2(my_texture.Width / 2, my_texture.Height / 2); } }
    }
}
