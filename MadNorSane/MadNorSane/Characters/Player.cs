using FarseerPhysics.Dynamics;
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
        public World my_world = null;
        public Body my_body = null;
        public Texture2D my_texture = null;
        public ContentManager _my_content = null;

        public void set_texture(String name)
        {
            try
            {
                my_texture = _my_content.Load<Texture2D>(name);
            }
            catch
            {
                my_texture = _my_content.Load<Texture2D>("place_holder");
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

        }
        public void move_in_air()
        {

        }
    }
}
