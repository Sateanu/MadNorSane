using System;
using System.Collections.Generic;
using System.Linq;
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
namespace MadNorSane
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        ScreenManager screenManager;

        private Texture2D mLightTexture;
        private int mNumLights = 4;
        private int mNumHorzontalHulls = 20;
        private int mNumVerticalHulls = 20;


        Random mRandom = new Random();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            screenManager = new ScreenManager(this, graphics);
            Components.Add(screenManager);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

       
        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
            
        }

        
        protected override void UnloadContent()
        {
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
