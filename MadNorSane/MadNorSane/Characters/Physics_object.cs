using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
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
    public abstract class Physics_object
    {


        public bool Active = true;
       public float width = 1, height = 1;
       public float x_coordinate = 0, y_coordinate = 0;
       public float turnMultiplier = 1;
       public float speed_float = 0.05f;
       public float angle = 0f;
       
        public Animation animation;
        

        public Vector2 velocity = Vector2.Zero;

    

        public World my_world = null;
        public Body my_body = null;
        public Texture2D my_texture = null;
        public ContentManager _my_content = null;

        public void set_texture(String name)
        {
            try
            {
                my_texture = _my_content.Load<Texture2D>(@"Textures\" + name);
                //animation = new Animation(my_texture, 18, 50, true);
                //animation.Activate();
            }
            catch
            {
                my_texture = _my_content.Load<Texture2D>(@"Textures\place_holder");
               // animation = new Animation(my_texture, 1, 10, true);
              //  animation.Activate();
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

        
        
        

        
        


        
        public virtual void Update(GameTime gameTime)
        {
            if (Active)
            {
                //animation.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                //animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
                spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y),
                                                             (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height)), null, Color.White,my_body.Rotation, origin, SpriteEffects.None, 0f);
            }
        }
        public void Draw(SpriteBatch spriteBatch,Color color)
        {
            if (Active)
            {
                //animation.Draw(spriteBatch, new Vector2((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y)), (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height));
                spriteBatch.Draw(my_texture, new Rectangle((int)Conversions.to_pixels(my_body.Position.X), (int)Conversions.to_pixels(my_body.Position.Y),
                                                             (int)Conversions.to_pixels(Width), (int)Conversions.to_pixels(Height)), null, color, my_body.Rotation, origin, SpriteEffects.None, 0f);
            }
        }

        public Vector2 origin { get { return new Vector2(my_texture.Width / 2f, my_texture.Height / 2f); } }
    }
}
