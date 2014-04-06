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
        public Skill my_attack1 = null, my_attack2 = null;
        List<Arrow> arrows = new List<Arrow>(5);
        
        public Archer(World _new_world, ContentManager _new_content, float x_coordinate, float y_coordinate,List<Modifier> modifiers)
        {
            _my_content = _new_content;
            stat = new Stats();
            this.modifiers = new List<Modifier>();
            foreach (var mod in modifiers)
            {
                stat.apply(mod);
                this.modifiers.Add(mod);
            }
            
            width += stat.width;
            height += stat.height;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, width, height, 1, new Vector2(x_coordinate, y_coordinate));
            my_body.BodyType = BodyType.Dynamic;
            my_body.FixedRotation = true;
            my_body.CollisionGroup = -1;
            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = this;
            my_body.CollisionGroup = -1;
            heart = _new_content.Load<Texture2D>(@"Textures\heart");
            arrowtext = _new_content.Load<Texture2D>(@"Textures\arrow");
            set_texture("archer");
            Color[] data=new Color[my_texture.Width*my_texture.Height];
            my_texture.GetData<Color>(data);
            color = data[0];
            health_color = _new_content.Load<Texture2D>(@"Textures\red");
            tinta = _new_content.Load<Texture2D>(@"Textures\direction");
            my_attack1 = new Skill(stat.primaryDamage, 0, 0, 3);
            my_attack2 = new Skill(stat.secondaryDamage, 0, 0, 5);

        }
        

        public void update_archer(GameTime _game_time)
        {
            my_attack1.update_skill_cool_down(_game_time);
            my_attack2.update_skill_cool_down(_game_time);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X) - (int)Conversions.to_pixels(width) / 2,
                                                        (int)Conversions.to_pixels(my_body.Position.Y) - (int)Conversions.to_pixels(height) / 2,
                                                        (int)Conversions.to_pixels(width), (int)Conversions.to_pixels(height)), Color.White);
            int health_size = (int)Conversions.to_pixels(height) * ((int)stat.original_health_points - (int)stat.health_points) / (int)stat.original_health_points;
            if ((int)stat.health_points <= 0)
            {
                health_size = (int)Conversions.to_pixels(height);
            }
            spriteBatch.Draw(health_color, new Rectangle((int)Conversions.to_pixels(my_body.Position.X) - (int)Conversions.to_pixels(width) / 2,
                                                        (int)Conversions.to_pixels(my_body.Position.Y) - (int)Conversions.to_pixels(height) / 2,
                                                        (int)Conversions.to_pixels(width), health_size), Color.White);
            //animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
            foreach (var arr in arrows)
                arr.Draw(spriteBatch);
        }

        private bool atack_with_arrows(Vector2 direction, GameTime _game_time, Skill _my_attack)
        {
            if (stat.arrownr > 0)
            {
                playSound("bow_atack");

                my_attack1.use_skill(_game_time);
                arrows.Add(new Arrow(my_world, _my_content, this, direction, _my_attack.damage));
                stat.arrownr--;
                return true;
            }
            else
                return false;
        }

        public bool atack(Vector2 direction, int _my_skill, GameTime _game_time)
        {
            if(_my_skill == 1)
            {
                return atack_with_arrows(direction, _game_time, my_attack1);

            }
            else
                if(_my_skill == 2)
                {
                    return atack_with_arrows(direction, _game_time, my_attack2);
                }

            return true;
        }
    }
}
