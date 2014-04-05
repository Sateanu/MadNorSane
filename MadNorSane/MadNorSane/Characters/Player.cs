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
       public float move_speed = 15, jump_speed = -10, health_points = 10, mana_points = 100;
       public bool btn_jump = false, btn_move_left = false, btn_move_right = false, btn_atack1 = false, btn_atack2 = false;
       public bool can_jump = false, can_move_left = false, can_move_right = false, can_atack1 = false, can_atack2 = false;
       public Texture2D heart;
        public int maxArrows = 4;
       public Texture2D arrowtext;
      public int arrownr = 4;
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
            MoveClass.controlGround(this, speed_float);
        }

        public void controlAir()
        {
            MoveClass.controlAir(this, 0.95f);
        }

       public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
        }
        public void DrawUI(SpriteBatch spriteBatch,int cadran,Viewport viewport)
       {
            switch(cadran)
            {
                case 0:
                    for (int i = 0; i < HP; i++)
                        spriteBatch.Draw(heart, new Rectangle(i * 34, 0, 32, 32),Color.White);
                    for (int i = 0; i < arrownr; i++)
                        spriteBatch.Draw(arrowtext, new Rectangle(i * 34, 34, 32, 32), Color.White);
                    break;
                case 1:
                    for (int i =0; i < HP; i++)
                        spriteBatch.Draw(heart, new Rectangle(viewport.Width-i * 34 - 34, 0, 32, 32), Color.White);
                    for (int i = 0; i < arrownr; i++)
                        spriteBatch.Draw(arrowtext, new Rectangle(viewport.Width - i * 34 - 34, 34, 32, 32), Color.White);
                    break;
                default:
                    break;
            }
       }
        public void move_in_air()
        {
        }

        public void TakeDamage(int damage)
        {
            this.HP -= damage;
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
                    }
                    else
                    if (fixB.Body.UserData == "ground" && touched_sides.Y < 0)
                    {
                        can_jump = false;
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
                        Console.WriteLine("is on left side of the wall");
                        can_jump = false;
                    }
                    else
                        if (fixB.Body.UserData == "wall" && touched_sides.Y > 0)
                        {
                            Console.WriteLine("i'm on the wall");
                            can_jump = true;
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
}
