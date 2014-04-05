using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Utilities
{
   public class Animation
    {
         Texture2D texture;
        int height;
        int width;
        int bigHeight;
        int bigWidth;
        bool repeat = false;
        bool Active = false;
        int indexX;
        int indexY;
        int maxIndex;
        TimeSpan time;
        TimeSpan lastTime;
        private float angle;
        int cX, cY;
        public bool Finished=false;
        public Animation(Texture2D tex, int bW, int bH, int countX,int countY, float t, bool repeat,float angle=0f)
        {
            texture = tex;
            height = bH/countY;
            width = bW / countX;
            bigHeight = bH;
            bigWidth = bW;
            time =TimeSpan.FromMilliseconds(t);
            this.repeat = repeat;
            indexX = 0;
            indexY = 0;
            maxIndex = countX+countY-1;
            cX = countX;
            cY = countY;

        }
        public Animation(Texture2D tex, int countX, float t, bool repeat, float angle = 0f)
        {
            texture = tex;
            height = tex.Height;
            width = tex.Width / countX;
            bigHeight = tex.Height;
            bigWidth = tex.Width;
            time = TimeSpan.FromMilliseconds(t);
            this.repeat = repeat;
            indexX = 0;
            indexY = 0;
            maxIndex = countX-1;
            cX = countX;

        }
       public void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (gameTime.TotalGameTime - lastTime > time)
                {
                    lastTime = gameTime.TotalGameTime;
                    indexX++;
                        if (indexX > cX)
                        {
                            if (repeat)
                            {
                                indexY = 0;
                                indexX = 0;
                            }
                            else
                            { Active = false; Finished = true; }
                        }
                }
            }
        }
       public void Activate()
       {
           Active = true;
           indexX = 0;
           indexY = 0;
       }
       internal void Draw(SpriteBatch spriteBatch, Vector2 position, int widthH, int heightH)
       {
           if (Active)
           {
               spriteBatch.Draw(texture, new Rectangle((int)position.X,(int)position.Y,widthH,heightH) ,
                   new Rectangle(indexX * width, indexY*height, widthH, heightH),
                   Color.White,0f, new Vector2(widthH / 2, heightH / 2),SpriteEffects.None, 0f);
           }
       }
    }
}
