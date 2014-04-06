using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Krypton;
using Krypton.Lights;
using MadNorSane.Screens;
using MadNorSane.Utilities;
namespace MadNorSane
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        ScreenManager screenManager;
        public static SoundManager soundManager;
        private Texture2D mLightTexture;
        private int mNumLights = 4;
        private int mNumHorzontalHulls = 20;
        private int mNumVerticalHulls = 20;


        Random mRandom = new Random();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            
            screenManager = new ScreenManager(this, graphics);
            Components.Add(screenManager);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

       
        protected override void Initialize()
        {

            base.Initialize();
            soundManager = new SoundManager(Content);
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
            try
            {
                string line;
                StreamReader reader = new StreamReader("settings.cfg");
                line=reader.ReadLine();
                Global.p1Type=int.Parse(line);
                line=reader.ReadLine();
                Global.p2Type=int.Parse(line);
                reader.Close();
                reader.Dispose();
            }
            catch(Exception e)
            {
                Global.p1Type=0;
                Global.p2Type=0;
            }
            
        }

        
        protected override void UnloadContent()
        {
            try
            {
                StreamWriter writer = new StreamWriter("settings.cfg");
                writer.WriteLine(Global.p1Type.ToString());
                writer.WriteLine(Global.p2Type.ToString());
                writer.Close();
                writer.Dispose();
            }
            catch(Exception e)
            {

            }
            base.UnloadContent();
        }

        
        protected override void Update(GameTime gameTime)
        {
           
            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            

            base.Draw(gameTime);
        }
        
    }
}
