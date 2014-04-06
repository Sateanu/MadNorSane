using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MadNorSane.Characters;
using FarseerPhysics.Dynamics;
using Krypton;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Krypton.Lights;
using Microsoft.Xna.Framework.Graphics;

namespace MadNorSane.Utilities
{
    public class Arrow : Physics_object
    {
        Player owner;
        int damage = 0;
        Light2D light;
        KryptonEngine kryp;
        public Arrow(World _new_world, ContentManager _new_content,KryptonEngine krypton,Texture2D tex ,Player owner, Vector2 direction, int _damage)
        {
            kryp = krypton;
            this.owner = owner;
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 0.5f, 0.5f, 1, owner.my_body.Position);
            my_body.UserData = "arrow";
            width = 0.5f;
            height = 0.5f;
            my_body.Rotation = (float)Math.Atan2(direction.X, -direction.Y);
            my_body.FixedRotation = true;
            my_body.OnCollision += my_body_OnCollision;
            my_body.BodyType = BodyType.Dynamic;
            my_body.IgnoreGravity = true;
            //my_body.GravityScale = 0.05f;
            my_body.Mass = 1f;
            my_body.ApplyLinearImpulse(direction);
            set_texture("arrow");
            light = new Light2D()
            {
                Texture = tex,
                Range = 70,
                Color = owner.color,
                Intensity = 0.8f,
                X = Conversions.to_pixels(my_body.Position.X),
                Y = Conversions.to_pixels(my_body.Position.Y),
                Angle = -(float)Math.PI * 3 / 2,
                //Fov = (float)Math.PI / 4,

            };
            krypton.Lights.Add(light);
            damage = _damage;
        }
        public override void Update(GameTime gameTime)
        {
            light.Position = Conversions.to_pixels(my_body.Position);
            base.Update(gameTime);
        }
        bool my_body_OnCollision(Fixture fixA, Fixture fixB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {

                if (fixA.Body.UserData == "arrow")
                {
                    if (fixB.Body.UserData == owner)
                        return false;
                    else
                        if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                        {
                            Console.WriteLine("Sageata a lovit player");
                            my_body.UserData = "arrow_dropped";
                            fixA.Body.LinearVelocity = Vector2.Zero;
                            fixA.Body.IgnoreGravity = false;
                            fixA.Body.Rotation = 0f;
                            Player pl = (Player)(fixB.Body.UserData);
                            pl.TakeDamage(damage);
                            return false;
                        }
                        else
                            if (fixB.Body.UserData == "ground" || fixB.Body.UserData == "wall")
                            {
                                my_body.UserData = "arrow_dropped";
                                fixA.Body.LinearVelocity = Vector2.Zero;
                                fixA.Body.IgnoreGravity = false;
                                fixA.Body.Rotation = 0f;
                                return true;
                            }
                            else if (fixB.Body.UserData == "energy_ball" || fixB.Body.UserData == "energy_ball_used" || fixB.Body.UserData == "arrow" || fixB.Body.UserData == "arrow_dropped")
                                    return false;
                }
                else
                    if (fixA.Body.UserData == "arrow_dropped")
                    {
                        if (fixB.Body.UserData == owner)
                        {
                            fixA.Body.Dispose();
                            fixA.Dispose();
                            Active = false;
                            kryp.Lights.Remove(light);

                            Player pl = (Player)(fixB.Body.UserData);
                            pl.stat.arrownr++;
                            SoundManager.playSound("loot");
                        }
                        else
                            if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                            {
                                return false;
                            }
                            else
                                if (fixB.Body.UserData == "ground" || fixB.Body.UserData == "wall")
                                    return true;
                                else if (fixB.Body.UserData == "energy_ball" || fixB.Body.UserData == "energy_ball_used" || fixB.Body.UserData == "arrow" || fixB.Body.UserData == "arrow_dropped")
                                    return false;

                    }

            }
            return true;
        }
    }
}
