using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
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
    class Mage : Player
    {
        public Skill my_attack1 = null, my_attack2 = null;

        List<EnergyBall> my_energy_balls = new List<EnergyBall>();

        public Mage(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate)
        {
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 1, 1, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;

            my_body.CollisionGroup = -1;
            move_speed = 10f;
            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = this;
            my_body.CollisionGroup = -1;
            heart = _new_content.Load<Texture2D>(@"Textures\heart");
            heartMP = _new_content.Load<Texture2D>(@"Textures\heartMP");
            set_texture("archeranim");

            my_attack1 = new Skill(4, 0, 0, 3, 2);
            my_attack2 = new Skill(2, 0, 0, 5, 1);
        }


        public void update_archer(GameTime _game_time)
        {
            my_attack1.update_skill_cool_down(_game_time);
            my_attack2.update_skill_cool_down(_game_time);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));

            foreach (var arr in my_energy_balls)
            {
                if (arr.my_body.UserData != "energy_ball_used")
                {
                    arr.Draw(spriteBatch);
                }
            }
        }

        private bool atack_with_energy_ball(Vector2 direction, GameTime _game_time, Skill _my_attack)
        {
            if (MP >= _my_attack.used_mp)
            {
                my_attack1.use_skill(_game_time);
                my_energy_balls.Add(new EnergyBall(my_world, _my_content, this, direction, _my_attack.damage));
                MP -= _my_attack.used_mp;
                //arrows.Add(new Arrow(my_world, _my_content, this, direction, _my_attack.damage));
                //arrownr--;
                return true;
            }
            else
                return false;
        }

        public bool atack(Vector2 direction, int _my_skill, GameTime _game_time)
        {
            if (_my_skill == 1)
            {
                return atack_with_energy_ball(direction, _game_time, my_attack1);
            }
            else
                if (_my_skill == 2)
                {
                    return atack_with_energy_ball(direction, _game_time, my_attack2);
                }

            return true;
        }
    }
}
