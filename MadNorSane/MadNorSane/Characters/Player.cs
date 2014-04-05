using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
   public abstract class Player : Physics_object
    {
       public float move_speed = 15, jump_speed = -10, health_points = 100, mana_points = 100;
       public bool btn_jump = false, btn_move_left = false, btn_move_right = false, btn_atack1 = false, btn_atack2 = false;
       public bool can_jump = false, can_move_left = false, can_move_right = false, can_atack1 = false, can_atack2 = false;
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

        public virtual bool atack()
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
            Console.WriteLine("is on ground");
            MoveClass.controlGround(this, speed_float);
            /*
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
                velocity.X = 0;
                my_body.LinearVelocity = velocity;
            }*/
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
            if (sign_wanted == 0)
            {
                return false;
            }
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
        public void controlAir()
        {
            Console.WriteLine("is in air");
            MoveClass.controlAir(this, 0.95f);
            /*
            if (btn_jump)
            {
                can_jump = false;
                velocity.Y = jump_speed;
                btn_jump = false;
                my_body.LinearVelocity = (velocity);
                return;
            }

            // Abort jump if user lets go of button
            if (velocity.Y > 0 && !btn_jump)
            {
                velocity.Y = 0;
            }

            itWalksInAir(speed_float * 0.2f);
             */
        }
        bool itWalksInAir(float T)
        {
            float speedVelocity = 0;
            speedVelocity = velocity.X;/// * -sgn(worldGravity.y);

            float linearVelocity = 0;
            linearVelocity = my_body.LinearVelocity.X;

            float sign = 0;

            if (btn_move_right && btn_move_left == false)
            {
                sign = 1;
            }
            else
                if (btn_move_right == false && btn_move_left)
                {
                    sign = -1;
                }
                else
                    return false;

            float currentSign = 0;

            if (linearVelocity < 0)
            {
                currentSign = -1;
            }
            else
                if (linearVelocity > 0)
                {
                    currentSign = 1;
                }

            float v = move_speed;

            if (currentSign != 0 && currentSign != sign)
            {
                //iau pozitia ca sa stiu cum misc camera
                v *= turnMultiplier;
            }

            //verific daca viteza curenta este mai mica decat viteza maxima, altfel nu mai maresc viteza
            if (sign < 0)
            {
                if (speedVelocity > -move_speed)
                {
                    speedVelocity += v * sign * T;
                }
                else
                {
                    speedVelocity = -move_speed;
                }
            }
            else
            {
                if (speedVelocity < move_speed)
                {
                    speedVelocity += v * sign * T;
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

       public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
        }

        public void move_in_air()
        {
        }


        public bool VS_OnCollision(Fixture fixA, Fixture fixB, Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {

                if (fixA.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                {
                    if (fixB.Body.UserData == "ground" && touched_sides.Y > 0)
                    {
                        can_jump = true;
                        Console.WriteLine("is on ground");
                    }
                    else
                        if (fixB.Body.UserData == "ground" && touched_sides.Y < 0)
                        {
                            can_jump = false;
                            Console.WriteLine("is under ground");
                        }
                        else
                            if (fixB.Body.UserData == "wall" && touched_sides.X > 0)
                            {
                                can_jump = false;
                                Console.WriteLine("is on right side of the wall");
                            }
                            else
                                if (fixB.Body.UserData == "wall" && touched_sides.X < 0)
                                {
                                    can_jump = false;
                                    Console.WriteLine("is on left side of the wall");
                                }
                    if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                    {
                        Console.WriteLine("Am lovit player");
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
