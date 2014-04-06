#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MadNorSane.Screens
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
       public enum BackgroundType
        {
            Simple,
            Tile,
            Full
        }


        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        BackgroundType type;
        Vector2 position;
        private string backgroundName;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen(string backgroundName,BackgroundType type,Vector2 pos)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.type = type;
            this.backgroundName = backgroundName;
            if (pos != null&&type==BackgroundType.Simple)
                position = pos;
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>(backgroundName);
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
                this.ExitScreen();
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            ScreenManager.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            switch (type)
            {
                case BackgroundType.Full:
                    spriteBatch.Draw(backgroundTexture, fullscreen,
                                     new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                    break;
                case BackgroundType.Tile:
                    int x = viewport.Width / backgroundTexture.Width + 1;
                    int y = viewport.Height / backgroundTexture.Height + 1;
                    for (int i = 0; i < x; i++)
                        for (int j = 0; j < y; j++)
                        {
                            spriteBatch.Draw(backgroundTexture, new Vector2(i * backgroundTexture.Width, j * backgroundTexture.Height),
                                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                        }
                    break;
                case BackgroundType.Simple:
                    spriteBatch.Draw(backgroundTexture, position,
                        new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
                    break;
            }
            spriteBatch.End();
        }


        #endregion
    }
}
