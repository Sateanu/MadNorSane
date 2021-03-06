﻿using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Krypton;
using Krypton.Lights;
using MadNorSane.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
    public class EnergyBall : Physics_object
    {
        Player owner;
        int damage = 0;
        Light2D light;
        KryptonEngine kryp;
        public EnergyBall(World _new_world, ContentManager _new_content, Player owner, Vector2 direction, int _damage,KryptonEngine krypton, Texture2D tex)
        {
            kryp = krypton;
            this.owner = owner;
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 0.5f, 0.5f, 1, owner.my_body.Position);
            my_body.UserData = "energy_ball";
            width = 0.5f;
            height = 0.5f;
            my_body.Rotation = (float)Math.Atan2(direction.X, -direction.Y);
            my_body.FixedRotation = true;
            my_body.OnCollision += my_body_OnCollision;
            my_body.BodyType = BodyType.Dynamic;
            my_body.IgnoreGravity = true;
            my_body.Mass = 1f;
            my_body.ApplyLinearImpulse(direction);
            set_texture("energy_ball");
            light = new Light2D()
            {
                Texture = tex,
                Range =70,
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
            this.light.Position = Conversions.to_pixels(my_body.Position);
            base.Update(gameTime);
        }
        bool my_body_OnCollision(Fixture fixA, Fixture fixB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {
                if (fixA.Body.UserData == "energy_ball")
                {
                    if (fixB.Body.UserData == owner)
                        return false;
                    else
                        if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                        {
                            Console.WriteLine("Energy Ball-ul a lovit player");
                            my_body.UserData = "energy_ball_used";
                            fixA.Body.LinearVelocity = Vector2.Zero;
                            fixA.Body.IgnoreGravity = false;
                            fixA.Body.Rotation = 0f;
                            fixA.Body.Dispose();
                            kryp.Lights.Remove(light);
                            
                            fixA.Dispose();
                            Active = false;
                            Player pl = (Player)(fixB.Body.UserData);
                            pl.TakeDamage(damage);
                            return false;
                        }
                        else
                            if (fixB.Body.UserData == "ground" || fixB.Body.UserData == "wall")
                            {
                                my_body.UserData = "energy_ball_used";
                                fixA.Body.LinearVelocity = Vector2.Zero;
                                fixA.Body.IgnoreGravity = false;
                                fixA.Body.Rotation = 0f;
                                fixA.Body.Dispose();
                                kryp.Lights.Remove(light);

                                fixA.Dispose();
                                Active = false;
                                
                                return true;
                            }
                            else if (fixB.Body.UserData == "energy_ball" || fixB.Body.UserData == "energy_ball_used" || fixB.Body.UserData == "arrow" || fixB.Body.UserData == "arrow_dropped")
                                return false;
                }
                

            }
            return true;
        }
    }
}
