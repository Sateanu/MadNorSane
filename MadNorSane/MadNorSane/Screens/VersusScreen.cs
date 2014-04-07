using MadNorSane.Characters;
using MadNorSane.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Screens
{
    public class VersusScreen : GameScreen
    {
        private  ContentManager content;
        private float pauseAlpha;
        Texture2D firstPlayer;
        Texture2D secondPlayer;
        public int p1Wins=0;
        public int p2Wins=0;
        public SpriteFont font;
        Texture2D vs;
        Player p1;
        Player p2;
        Texture2D win1, win2;
        bool p1ready = false;
        bool p2ready = false;
        TimeSpan delay=TimeSpan.FromSeconds(1f);
        public VersusScreen(Player[] players,int p1Win=0,int p2Win=0)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            p1Wins = p1Win;
            Score.p1Score += p1Win;
            Score.p2Score += p2Win;
            p2Wins = p2Win;
            p1 = players[0];
            p2 = players[1];
            p1.score = p1Win;
            p2.score = p2Win;
            delay = TimeSpan.FromSeconds(1f);
        }
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            firstPlayer = content.Load<Texture2D>(@"Textures\archer");
            secondPlayer = content.Load<Texture2D>(@"Textures\mage");
            font = content.Load<SpriteFont>("menufont");
            vs = content.Load<Texture2D>(@"Textures\vs");
            win1 = content.Load<Texture2D>(@"Textures\win1");
            win2 = content.Load<Texture2D>(@"Textures\win2");
            ScreenManager.Game.ResetElapsedTime();
        }
        public override void UnloadContent()
        {
            content.Unload();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            delay -= gameTime.ElapsedGameTime;

            if (IsActive)
            {
                if(p1ready&&p2ready)
                {
                    this.ExitScreen();
                }
            }
        }
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
            PlayerIndex piout;
            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            //if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            //{
             //   this.ExitScreen();
            //}
            if (delay.TotalMilliseconds < 10)
            {
                if (input.CurrentKeyboardStates[0].GetPressedKeys().Count() > 0)
                {
                    p1ready = true;
                }
                var gp = input.CurrentGamePadStates[0];
                if (gp.Triggers.Left > 0 || gp.Triggers.Right > 0)
                {
                    p2ready = true;
                }
                if (!input.GamePadWasConnected[0])
                    p2ready = true;
                foreach (Buttons b in (Buttons[])Enum.GetValues(typeof(Buttons)))
                    if (gp.IsButtonDown(b))
                        p2ready = true;
            }
            
        }
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 scale=new Vector2(0.5f);
            Random r=new Random();
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport vp = ScreenManager.GraphicsDevice.Viewport;
            scale *= new Vector2(vp.Width/800f,vp.Height/480f);
            spriteBatch.Begin();
            spriteBatch.Draw(firstPlayer, new Rectangle(0, 0, vp.Width / 2, vp.Height), Color.White);
            spriteBatch.Draw(secondPlayer, new Rectangle(vp.Width/2, 0, vp.Width , vp.Height), Color.White);
            //spriteBatch.Draw(vs, new Rectangle(vp.Width / 2, vp.Height / 2, 150, 150), null,new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()) , 0f, new Vector2(vs.Width / 2f, vs.Height / 2f), SpriteEffects.None, 0f);
            
            spriteBatch.Draw(vs, new Rectangle(vp.Width / 2, vp.Height / 2, (int)(300*scale.X), (int)(300*scale.Y)), null, Color.DarkRed, 0f, new Vector2(vs.Width / 2f, vs.Height / 2f), SpriteEffects.None, 0f);
            if (p1Wins==1)
            spriteBatch.Draw(win1, new Rectangle(vp.Width / 2, win1.Height/2+100, (int)(win1.Width*0.5f * scale.X), (int)(win2.Height*0.5f * scale.Y)), null, Color.DarkRed, 0f, new Vector2(win1.Width / 2f, win1.Height / 2f), SpriteEffects.None, 0f);
            else
            if(p2Wins==1)
                spriteBatch.Draw(win2, new Rectangle(vp.Width / 2, win2.Height/2+100, (int)(win1.Width *0.5f* scale.X), (int)(win2.Height*0.5f * scale.Y)), null, Color.DarkRed, 0f, new Vector2(win2.Width / 2f, win2.Height / 2f), SpriteEffects.None, 0f);
            string type1 = p1.GetType() == typeof(Archer) ? "Archer" : "Mage";
            spriteBatch.DrawString(font, "Player 1 - "+type1, Vector2.Zero + new Vector2(30, 0), Color.Black,0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if(type1=="Archer")
            {
                spriteBatch.DrawString(font, "Trebuie sa te duci sa iti recuperezi sagetile!", new Vector2(0, 50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None,0f);
            }
            else
                spriteBatch.DrawString(font, "Trebuie sa astepti mana sa se regenereze! ", new Vector2(0, 50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if(!p1ready)
            spriteBatch.DrawString(font, "Press any button when ready", new Vector2(30 * scale.X, vp.Height - font.MeasureString("Press anybutton when ready").Y - 100), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else
            spriteBatch.DrawString(font, "READY!", new Vector2(30 * scale.X, vp.Height - font.MeasureString("READY!").Y - 100), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Wins: " + Score.p1Score.ToString(), new Vector2(30 * scale.X, vp.Height - font.MeasureString("Wins: " + p1Wins.ToString()).Y-50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            float x=5,y=92;
            foreach (var mod in p1.modifiers)
            {
                spriteBatch.DrawString(font, mod.descriere, new Vector2(x, y), Color.Black,0f,Vector2.Zero,scale,SpriteEffects.None,0f);
                y += 32*scale.Y+5;
            }
            string type2 = p2.GetType() == typeof(Archer) ? "Archer" : "Mage";
            if (type2 == "Archer")
            {
                spriteBatch.DrawString(font, "Trebuie sa te duci sa iti recuperezi sagetile!", new Vector2(vp.Width - font.MeasureString("Trebuie sa te duci sa iti recuperezi sagetile!").X * scale.X - 10, 50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
                spriteBatch.DrawString(font, "Trebuie sa astepti mana sa se regenereze!", new Vector2(vp.Width - font.MeasureString("Trebuie sa astepti mana sa se regenereze!").X * scale.X -10, 50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, type2 + " - Player 2", new Vector2(vp.Width - font.MeasureString(type2 + " - Player 2").X*scale.X - 30, 0), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            if (!p2ready)
                spriteBatch.DrawString(font, "Press any button when ready", new Vector2(vp.Width - font.MeasureString("Press any button when ready").X*scale.X - 30,vp.Height - font.MeasureString("Press space when ready").Y - 100), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else
                spriteBatch.DrawString(font, "READY!", new Vector2(vp.Width - font.MeasureString("READY!").X * scale.X - 30, vp.Height - font.MeasureString("READY!").Y - 100), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Wins: " + Score.p2Score.ToString(), new Vector2(vp.Width - font.MeasureString("Wins: " + p1Wins.ToString()).X*scale.X - 30, vp.Height - font.MeasureString("Wins: " + p2Wins.ToString()).Y-50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            x = vp.Width-font.MeasureString("Player 2").X;
            y = 92;
            foreach (var mod in p2.modifiers)
            {
                x = vp.Width - font.MeasureString(mod.descriere).X*scale.X-5;
                spriteBatch.DrawString(font, mod.descriere, new Vector2(x, y), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                y += 32*scale.Y+5;
            }
            
            spriteBatch.End();
            
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        
    }
}
