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
        }
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            firstPlayer = content.Load<Texture2D>(@"Textures\archer");
            secondPlayer = content.Load<Texture2D>(@"Textures\mage");
            font = content.Load<SpriteFont>("menufont");
            vs = content.Load<Texture2D>(@"Textures\vs");
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

            if (IsActive)
            {

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

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                this.ExitScreen();
            }
            else
            {
                
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
            spriteBatch.DrawString(font, "Player 1", Vector2.Zero + new Vector2(30, 0), Color.Black,0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Wins: " + Score.p1Score.ToString(), new Vector2(30 * scale.X, vp.Height - font.MeasureString("Wins: " + p1Wins.ToString()).Y-50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            float x=5,y=62;
            foreach (var mod in p1.modifiers)
            {
                spriteBatch.DrawString(font, mod.descriere, new Vector2(x, y), Color.Black,0f,Vector2.Zero,scale,SpriteEffects.None,0f);
                y += 32*scale.Y+5;
            }
            spriteBatch.DrawString(font, "Player 2", new Vector2(vp.Width - font.MeasureString("Player 2").X - 30, 0), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Wins: " + Score.p2Score.ToString(), new Vector2(vp.Width - font.MeasureString("Wins: " + p1Wins.ToString()).X*scale.X - 30, vp.Height - font.MeasureString("Wins: " + p2Wins.ToString()).Y-50), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            x = vp.Width-font.MeasureString("Player 2").X;
            y = 62;
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
