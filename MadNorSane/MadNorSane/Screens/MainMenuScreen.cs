#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using MadNorSane.Screens;
using System.Threading;
#endregion

namespace MadNorSane.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play");
            MenuEntry optionMenuEntry = new MenuEntry("Options");
            MenuEntry exit = new MenuEntry("Exit");
            // Hook up menu event handlers.
            playGameMenuEntry.Selected+=playGameMenuEntry_Selected;
            optionMenuEntry.Selected += OptionsMenuEntrySelected;
            exit.Selected+=exit_Selected;
            // Add entries to the menu.
            //MenuEntries.Add(playGameMenuEntry);
            //MenuEntries.Add(playGameMenuEntry4);
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionMenuEntry);
            MenuEntries.Add(exit);
        }

        
        [STAThread]

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }
        void playGameMenuEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);
        }
        void exit_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();

        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit Project Bot?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}