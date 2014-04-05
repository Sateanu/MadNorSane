#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Krypton;
using Krypton.Lights;
using FarseerPhysics.Factories;
using MadNorSane.Utilities;
using MadNorSane.Characters;
#endregion

namespace MadNorSane.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        World world;
        KryptonEngine krypton;
        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);
        
        Random random = new Random();

        float pauseAlpha;
        private Texture2D mLightTexture;
        private int mNumLights=20;
        private int mNumHorzontalHulls=10;
        private int mNumVerticalHulls=10;
        Camera camera;
        Archer my_archer = null;
        Archer my_archer2 = null;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            gameFont = content.Load<SpriteFont>("menufont");
            this.krypton = new KryptonEngine(ScreenManager.Game, "KryptonEffect");
            krypton.SpriteBatchCompatablityEnabled = true;
            krypton.CullMode = CullMode.None;

            world = new World(new Vector2(0, 9.8f));
            my_archer = new Archer(world, content, 0, -10);
            my_archer2 = new Archer(world, content, -3, -20);
            this.krypton.Initialize();
            camera = new Camera(ScreenManager.GraphicsDevice.Viewport);
            camera.Follow(my_archer.my_body);
            Console.WriteLine(camera.IsFollowing);
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.

            this.mLightTexture = LightTextureBuilder.CreatePointLight(ScreenManager.GraphicsDevice, 512);
            Light2D light = new Light2D()
            {
                Texture = mLightTexture,
                Range = 1000f,
                Color = Color.White,
                Intensity = 0.8f,
                X = 0,
                Y = -350,
                Angle=1f,
                Fov = MathHelper.PiOver2 ,
            };
            krypton.Lights.Add(light);
            for (int i = -20; i <= 20;i++)
                addObject(i*20, 50, 10, 10);
            // Create some lights and hulls
          //  this.CreateLights(mLightTexture, this.mNumLights);
           // this.CreateHulls(this.mNumHorzontalHulls, this.mNumVerticalHulls);
           
            ScreenManager.Game.ResetElapsedTime();
        }
        void addObject(float x, float y, float width, float height)
        {
            var body= BodyFactory.CreateRectangle(world,Conversions.to_meters(width),Conversions.to_meters(height),1f,new Vector2(Conversions.to_meters(x),Conversions.to_meters(y)));
            body.BodyType = BodyType.Static;
             var hull = ShadowHull.CreateRectangle(new Vector2(width,height));
             hull = ShadowHull.CreateRectangle(new Vector2(width, height));
             hull.Position.X = x;
             hull.Position.Y = y;

                    krypton.Hulls.Add(hull);
        }
        private void CreateLights(Texture2D texture, int count)
        {
            // Make some random lights!
            for (int i = 0; i < count; i++)
            {
                byte r = (byte)(this.random.Next(255 - 64) + 64);
                byte g = (byte)(this.random.Next(255 - 64) + 64);
                byte b = (byte)(this.random.Next(255 - 64) + 64);

                Light2D light = new Light2D()
                {
                    Texture = texture,
                    Range = (float)(this.random.NextDouble() * 50 + 1),
                    Color = new Color(r, g, b),
                    //Intensity = (float)(this.mRandom.NextDouble() * 0.25 + 0.75),
                    Intensity = 1f,
                    Angle = MathHelper.TwoPi * (float)this.random.NextDouble(),
                    X = (float)(this.random.NextDouble() * 150 - 25),
                    Y = (float)(this.random.NextDouble() * 150 - 25),
                };

                // Here we set the light's field of view
                if (i % 2 == 0)
                {
                    light.Fov = MathHelper.PiOver2 * (float)(this.random.NextDouble() * 0.75 + 0.25);
                }

                this.krypton.Lights.Add(light);
            }
        }

        private void CreateHulls(int x, int y)
        {
            float w = 50;
            float h = 50;

            // Make lines of lines of hulls!
            for (int j = 0; j < y; j++)
            {
                // Make lines of hulls!
                for (int i = 0; i < x; i++)
                {
                    var posX = ((i * w) / x) - w / 2 + (j % 2 == 0 ? w / x / 2 : 0) * 10;
                    var posY = ((j * h) / y) - h / 2 + (i % 2 == 0 ? h / y / 4 : 0) * 10;

                    var hull = ShadowHull.CreateRectangle(Vector2.One);
                    hull.Position.X = posX;
                    hull.Position.Y = posY;
                    hull.Scale.X = (float)(this.random.NextDouble() * 10f + 0.25f);
                    hull.Scale.Y = (float)(this.random.NextDouble() * 10f + 0.25f);

                    krypton.Hulls.Add(hull);
                }
            }
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
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
               
                camera.Update(gameTime);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    ScreenManager.Game.Exit();

                world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                var t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                var speed = 5;

                // Allow for randomization of lights and hulls, to demonstrait that each hull and light is individually rendered
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    // randomize lights
                    foreach (Light2D light in this.krypton.Lights)
                    {
                        light.Position += Vector2.UnitY * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                        light.Position += Vector2.UnitX * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                        light.Angle -= MathHelper.TwoPi * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                    }

                    // randomize hulls
                    foreach (var hull in this.krypton.Hulls)
                    {
                        hull.Position += Vector2.UnitY * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                        hull.Position += Vector2.UnitX * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                        hull.Angle -= MathHelper.TwoPi * (float)(this.random.NextDouble() * 2 - 1) * t * speed;
                    }
                }
            }
            my_archer.move_on_ground();
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
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
                ScreenManager.AddScreen(new OptionsMenuScreen(),0);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;
                Vector2 movement2 = Vector2.Zero;
                PlayerIndex piout;
                Console.WriteLine((int)playerIndex);
                if (input.IsKeyPress(Keys.Left, ControllingPlayer.Value, out piout))
                    movement.X--;

                if (input.IsKeyPress(Keys.Right, ControllingPlayer.Value, out piout))
                    movement.X++;

                if (input.IsKeyPress(Keys.Up, ControllingPlayer.Value, out piout))
                    movement.Y--;

                if (input.IsKeyPress(Keys.Down, ControllingPlayer.Value, out piout))
                    movement.Y++;



                Vector2 thumbstick = input.CurrentGamePadStates[playerIndex].ThumbSticks.Left;

                movement2.X += thumbstick.X;
                movement2.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();
                if (movement2.Length() > 1)
                    movement2.Normalize();

                my_archer.my_body.ApplyLinearImpulse(movement);
                my_archer2.my_body.ApplyLinearImpulse(movement2);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix view = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Matrix.CreateTranslation(new Vector3(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2, 0));
           // camera.position = new Vector2(20, 20);
           
            this.krypton.Matrix = camera.View;
            this.krypton.Bluriness = 3;
            this.krypton.LightMapPrepare();

            // Make sure we clear the backbuffer *after* Krypton is done pre-rendering
            ScreenManager.GraphicsDevice.Clear(Color.White);

            // ----- DRAW STUFF HERE ----- //
            // By drawing here, you ensure that your scene is properly lit by krypton.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.View);
            my_archer.Draw(spriteBatch);
            my_archer2.Draw(spriteBatch);
            spriteBatch.End();
            // Drawing after KryptonEngine.Draw will cause you objects to be drawn on top of the lightmap (can be useful, fyi)
            // ----- DRAW STUFF HERE ----- //

            // Draw krypton (This can be omited if krypton is in the Component list. It will simply draw krypton when base.Draw is called
            this.krypton.Draw(gameTime);

            // Draw the shadow hulls as-is (no shadow stretching) in pure white on top of the shadows
            // You can omit this line if you want to see what the light-map looks like :)
            
            this.DebugDraw();
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        private void DebugDraw()
        {
            this.krypton.RenderHelper.Effect.CurrentTechnique = this.krypton.RenderHelper.Effect.Techniques["DebugDraw"];
            ScreenManager.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.WireFrame,
            };
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                // Clear the helpers vertices
                this.krypton.RenderHelper.ShadowHullVertices.Clear();
                this.krypton.RenderHelper.ShadowHullIndicies.Clear();

                foreach (var hull in krypton.Hulls)
                {
                    this.krypton.RenderHelper.BufferAddShadowHull(hull);
                }


                foreach (var effectPass in krypton.RenderHelper.Effect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    this.krypton.RenderHelper.BufferDraw();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                this.krypton.RenderHelper.ShadowHullVertices.Clear();
                this.krypton.RenderHelper.ShadowHullIndicies.Clear();

                foreach (Light2D light in krypton.Lights)
                {
                    this.krypton.RenderHelper.BufferAddBoundOutline(light.Bounds);
                }

                foreach (var effectPass in krypton.RenderHelper.Effect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    this.krypton.RenderHelper.BufferDraw();
                }
            }
        }


        #endregion
    }
}
