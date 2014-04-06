#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using MadNorSane.Utilities;
#endregion

namespace MadNorSane.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        MenuEntry fullScreen;
        #endregion
        
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            string s1 = "Random";
            if (Global.p1Type == 1)
                s1 = "Archer";
            else if (Global.p1Type == 2)
                s1 = "Mage";
            string s2 = "Random";
            if (Global.p2Type == 1)
                s2 = "Archer";
            else if (Global.p2Type == 2)
                s2 = "Mage";
            MenuEntry p1Type = new MenuEntry("Player 1: "+s1);
            p1Type.Selected+=p1Type_Selected;
            MenuEntry p2Type = new MenuEntry("Player 2: "+s2);
            p2Type.Selected += p2Type_Selected;
            // Create our menu entries.
            MenuEntries.Add(p1Type);
            MenuEntries.Add(p2Type);

        }
        #endregion
        #region Handle Input
        void p1Type_Selected(object sender, PlayerIndexEventArgs e)
        {
            MenuEntry entry = (MenuEntry)sender;
            if(Global.p1Type==0)
            {
                Global.p1Type = 1;
                entry.Text = "Player 1: Archer";
            }
            else if (Global.p1Type==1)
            {
                Global.p1Type = 2;
                entry.Text = "Player 1: Mage";
            }
            else
            {
                Global.p1Type = 0;
                entry.Text = "Player 1: Random";
            }
        }
        void p2Type_Selected(object sender, PlayerIndexEventArgs e)
        {
            MenuEntry entry = (MenuEntry)sender;
            if (Global.p2Type == 0)
            {
                Global.p2Type = 1;
                entry.Text = "Player 2: Archer";
            }
            else if (Global.p2Type == 1)
            {
                Global.p2Type = 2;
                entry.Text = "Plyer 2: Mage";
            }
            else
            {
                Global.p2Type = 0;
                entry.Text = "Player 2: Random";
            }
        }

        #endregion
    }
}
