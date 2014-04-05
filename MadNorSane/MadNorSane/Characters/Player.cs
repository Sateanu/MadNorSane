using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
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

        public float move_speed = 15, jump_speed = -10, health_points = 100, mana_points = 100;

        float width = 1, height = 1;
        float x_coordinate = 0, y_coordinate = 0;
        float turnMultiplier = 1;
        float speed_float = 0.05f;
        Animation animation;
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
                animation = new Animation(my_texture, 18, 50, true);
                animation.Activate();
                
            }
            catch
            {
                my_texture = _my_content.Load<Texture2D>(@"Textures\place_holder");
                animation = new Animation(my_texture, 1, 10, true);
                animation.Activate();
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
            MoveClass.controlGround(this, speed_float);
        }

        public void controlAir()
        {
            MoveClass.controlAir(this, 0.95f);
        }



        public void move_in_air()
        {
        }


        public bool VS_OnCollision(Fixture fixA, Fixture fixB, Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {
                if(fixA.Body.UserData == "player")
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
                        can_jump = false;
                        Console.WriteLine("is on left side of the wall");
                    }
                }
            }
            return true;
        }
        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
           /* spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y),
                                                        (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height)), null,Color.White,0f,origin,SpriteEffects.None,0f);*/
        }

        public Vector2 origin { get { return new Vector2(my_texture.Width / 2, my_texture.Height / 2); } }
    }
}
