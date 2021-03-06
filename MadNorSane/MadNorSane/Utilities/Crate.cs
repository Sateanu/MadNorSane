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
    class Crate : Physics_object
    {
        Light2D light;
        KryptonEngine kryp;
        Modifier modifier;
        int type;
        public Crate(World _new_world, ContentManager _new_content, KryptonEngine krypton, Texture2D tex, Vector2 pos)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            kryp = krypton;
            _my_content = _new_content;
            my_world = _new_world;
            my_body = BodyFactory.CreateRectangle(my_world, 2f, 2f, 1, pos);
            my_body.UserData = "crate";
            width = 2f;
            height = 2f;
            my_body.FixedRotation = true;
            my_body.OnCollision += my_body_OnCollision;
            my_body.BodyType = BodyType.Dynamic;
            my_body.IgnoreGravity = true;
            ModifierList li = new ModifierList();
            //my_body.GravityScale = 0.05f;
            modifier = li.getMod();
            my_body.Mass = 1f;
            set_texture("chest");
            Color color;
            double d=r.NextDouble();
            type=1;
            color=Color.White;
            if(d<=0.33f)
            {
                color=Color.LightGreen;
                type=2;
                int hp=r.NextDouble()>0.5f ? 1 : -1;
                int maxArr=r.NextDouble()<0.5f ? 1 : -1;
                modifier = new Modifier()
                {
                    descriere = "Heal",
                    health_points = hp,
                    jump_speed = 0,
                    mana_points = maxArr,
                    maxArrows = maxArr,
                    move_speed = 0,
                    primaryDamage = 0,
                    projectile_speed = 15,
                    reload_time = 1,
                };
            }else
                if(d<=0.66f)
                {
                    color=Color.LightPink;
                    type = 2;
                    int hp = r.NextDouble() > 0.5f ? 2 : -2;
                    int maxArr = r.NextDouble() < 0.5f ? 2 : -2;
                    modifier = new Modifier()
                    {
                        descriere = "Heal",
                        health_points = hp,
                        jump_speed = 0,
                        mana_points = maxArr,
                        maxArrows = maxArr,
                        move_speed = 0,
                        primaryDamage = 0,
                        projectile_speed = 15,
                        reload_time = 1,
                    };
                }

                
            light = new Light2D()
            {
                Texture = tex,
                Range = 160,
                Color = color,
                Intensity = 0.8f,
                X = Conversions.to_pixels(my_body.Position.X),
                Y = Conversions.to_pixels(my_body.Position.Y),
                Angle = -(float)Math.PI * 3 / 2,
                //Fov = (float)Math.PI / 4,

            };
            kryp.Lights.Add(light);

        }
        public override void Update(GameTime gameTime)
        {
            light.Range=(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)*100;
            base.Update(gameTime);
        }
        bool my_body_OnCollision(Fixture fixA, Fixture fixB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Vector2 touched_sides = contact.Manifold.LocalNormal;
            if (contact.IsTouching)
            {

                if (fixA.Body.UserData == "crate")
                {
                    if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                    {
                        Console.WriteLine("Sageata a lovit player");
                        my_body.UserData = "arrow_dropped";
                        fixA.Body.LinearVelocity = Vector2.Zero;
                        fixA.Body.IgnoreGravity = false;
                        fixA.Body.Rotation = 0f;
                        Player pl = (Player)(fixB.Body.UserData);
                        if (type != 3)
                            pl.stat.apply(modifier);
                        kryp.Lights.Remove(light);
                        fixA.Body.Dispose();
                        SoundManager.playSound("crate");
                        return false;
                    }
                    return false;

                }

            }
            return true;
        }
    }
}
