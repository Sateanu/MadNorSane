using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Krypton;
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
        public long last_used_energy_ball = 0;
        List<EnergyBall> my_energy_balls = new List<EnergyBall>();

        public Mage(World _new_world, ContentManager _new_content,KryptonEngine krypton ,float x_coordinate, float y_coordinate,List<Modifier> modifiers,Texture2D tex=null)
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
            hull = ShadowHull.CreateRectangle(Conversions.to_pixels(new Vector2(width, height)));

            hull.Position.X = Conversions.to_pixels(x_coordinate);
            hull.Position.Y = Conversions.to_pixels(y_coordinate);
            krypton.Hulls.Add(hull);
            my_body.CollisionGroup = -1;
            
            my_body.OnCollision += new OnCollisionEventHandler(VS_OnCollision);
            my_body.UserData = this;
            my_body.CollisionGroup = -1;
            heart = _new_content.Load<Texture2D>(@"Textures\heart");
            heartMP = _new_content.Load<Texture2D>(@"Textures\heartMP");
            if (tex == null)
                set_texture("mage");
            else
                my_texture = tex;
            Color[] data = new Color[my_texture.Width * my_texture.Height];
            my_texture.GetData<Color>(data);
            color = data[0];
            health_color = _new_content.Load<Texture2D>(@"Textures\red");
            tinta = _new_content.Load<Texture2D>(@"Textures\direction");
            
            my_attack1 = new Skill(stat.primaryDamage, 0, 0, 3, 1);
            my_attack2 = new Skill(stat.secondaryDamage, 0, 0, 5, 2);
        }

        public override void Update(GameTime gameTime)
        {
            long time_to_load = (int)(10000000 * stat.reload_time);
            if (gameTime.TotalGameTime.Ticks - last_used_energy_ball > time_to_load)
            {
                if (stat.original_mana_points > stat.mana_points)
                {
                    stat.mana_points++;
                    last_used_energy_ball += time_to_load;
                }
            }
            foreach (var magic in my_energy_balls)
                magic.Update(gameTime);
            my_attack1.update_skill_cool_down(gameTime);

            my_attack2.update_skill_cool_down(gameTime);
            base.Update(gameTime);
        }
        public void update_mage(GameTime _game_time)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                                                        (int)Conversions.to_pixels(width), health_size), Color.Red);
            //animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
            foreach (var arr in my_energy_balls)
            {

                if (arr.my_body.UserData != "energy_ball_used")
                {
                    arr.Draw(spriteBatch);
                }
            }
        }

        private bool atack_with_energy_ball(Vector2 direction, GameTime _game_time, Skill _my_attack,KryptonEngine krypton, Texture2D tex)
        {
            if (MP >= _my_attack.used_mp)
            {
                playSound("laserShot");

                last_used_energy_ball = _game_time.TotalGameTime.Ticks;
                my_attack1.use_skill(_game_time);
                my_energy_balls.Add(new EnergyBall(my_world, _my_content, this, direction, _my_attack.damage,krypton,tex));
                MP -= _my_attack.used_mp;
                //arrows.Add(new Arrow(my_world, _my_content, this, direction, _my_attack.damage));
                //arrownr--;
                return true;
            }
            else
                return false;
        }

        public override void atack(Vector2 direction, int _my_skill, GameTime _game_time, KryptonEngine krypton, Texture2D tex)
        {
            if (_my_skill == 1)
            {
                atack_with_energy_ball(direction, _game_time, my_attack1,krypton,tex);
            }
          


            base.atack(direction, _my_skill, _game_time,krypton,tex);
        }
    }
}
