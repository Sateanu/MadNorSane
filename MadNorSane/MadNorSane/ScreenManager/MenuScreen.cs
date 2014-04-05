#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MadNorSane
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields
        public float MenuInputDelay { get; set; }
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;
        MouseState current;
        MouseState prev;
        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Initialization

        bool noCancel = false;
        /// <summary>
        /// Constructor.
        /// </summary>
      /*  public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        public MenuScreen(string menuTitle,bool noCancel)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.noCancel = noCancel;
        }*/
        float staticInputDelay;
        bool startResetDelay = false;
        int ticksReset;
        int nrTicks=30;
        public MenuScreen(string menuTitle, bool noCancel=false, bool centerMenuEntry=false)
        {
            this.menuTitle = menuTitle;
            ticksReset = nrTicks;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.noCancel = noCancel;
            this.centerMenuEntry = centerMenuEntry;
            MenuInputDelay = staticInputDelay = 0.3f;
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            prev=current;
            current=Mouse.GetState();
            if (startResetDelay)
            {
                ticksReset--;
                if (ticksReset < 0)
                {
                    ticksReset = 60;
                    MenuInputDelay = staticInputDelay;
                    startResetDelay = false;
                }
            }
            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer, MenuInputDelay) && menuEntries.Count > 0)
            {
                startResetDelay = false;
                ticksReset = nrTicks;
                MenuInputDelay -= 0.01f;
                MenuInputDelay = MathHelper.Clamp(MenuInputDelay, 0.1f, staticInputDelay);
                selectedEntry--;
                AddOffSet(-1);
                if (selectedEntry < 0)
                {
                    selectedEntry = menuEntries.Count - 1;
                    AddOffSet(menuEntries.Count);
                }
            } else
                if (input.IsMenuDown(ControllingPlayer, MenuInputDelay) && menuEntries.Count > 0)
                {
                    startResetDelay = false;
                    ticksReset = nrTicks; 
                    selectedEntry++;
                    MenuInputDelay = MathHelper.Clamp(MenuInputDelay, 0.1f, staticInputDelay);
                    AddOffSet(1);
                    MenuInputDelay -= 0.01f;
                    if (selectedEntry >= menuEntries.Count)
                    {
                        selectedEntry = 0;
                        AddOffSet(-menuEntries.Count);
                    }
                }
                else { startResetDelay = true; }

            if (input.IsScroolDown())
            {
                AddOffSet(1);
            }
            else if (input.IsScroolUp())
            {
                AddOffSet(-1);
            }
            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex) && menuEntries.Count>0)
            {
                if(selectedEntry<menuEntries.Count)
                OnSelectEntry(selectedEntry, playerIndex);
            }
            if (input.IsMenuLeft(ControllingPlayer, MenuInputDelay) && menuEntries.Count > 0)
            {
                if (selectedEntry < menuEntries.Count)
                {
                    startResetDelay = false;
                    ticksReset = nrTicks;
                    MenuInputDelay -= 0.01f;
                    MenuInputDelay = MathHelper.Clamp(MenuInputDelay, 0.1f, staticInputDelay);
                    OnSelectEntryLeft(selectedEntry, playerIndex);
                }
            }
            if (input.IsMenuRight(ControllingPlayer, MenuInputDelay) && menuEntries.Count > 0)
            {
                if (selectedEntry < menuEntries.Count)
                {
                    startResetDelay = false;
                    ticksReset = nrTicks;
                    MenuInputDelay -= 0.01f;
                    MenuInputDelay = MathHelper.Clamp(MenuInputDelay, 0.1f, staticInputDelay);
                    OnSelectEntryRight(selectedEntry, playerIndex);
                }
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex) && !noCancel)
            {
                OnCancel(playerIndex);
            }
            for (int i = 0; i < menuEntries.Count; i++)
            {
                if (prev.X != current.X || prev.Y != current.Y)
                    if (menuEntries[i].Hover(current, ScreenManager.Font))
                    selectedEntry = i;
                if (menuEntries[i].Click(current, prev, ScreenManager.Font))
                {
                    selectedEntry = i;
                    OnSelectEntry(i, PlayerIndex.One);
                }
            }

        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }
        protected virtual void OnSelectEntryLeft(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntryLeft(playerIndex);
        }
        protected virtual void OnSelectEntryRight(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntryRight(playerIndex);
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


        Vector2 offSet = Vector2.Zero;
        void AddOffSet(int nr)
        {
            if (MenuEntries.Count > 0)
            {
                offSet.Y += nr * menuEntries[0].GetHeight(this);
                offSet.Y = MathHelper.Clamp(offSet.Y, 0, menuEntries.Count * MenuEntries[0].GetHeight(this) - ScreenManager.GraphicsDevice.Viewport.Height/2f);
            }
        }
        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(0f, 150f);
            
            if(centerMenuEntry)
                position = new Vector2(0f, 150f);
            if (centerMenuEntry && menuEntries.Count > 0)
                position.Y -= offSet.Y; //PUNE JOS IN FOR SI FA CATEVA CENTRATE CU ALPHA COLOR
            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                //position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;
                position.X = 50;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }


        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

                spriteBatch.Begin();
            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                if(menuEntry.Position.Y>100)
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 1.5f, 40);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion

        public bool centerMenuEntry { get; set; }
    }
}
