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
using FarseerPhysics.DebugView;
using System.Collections.Generic;
#endregion

namespace MadNorSane.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
   public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        World world;
        KryptonEngine krypton;
        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);
        Camera mouseCamera;
        Random random = new Random();

        float pauseAlpha;
        private Texture2D mLightTexture;
        private int mNumLights=20;
        private int mNumHorzontalHulls=10;
        private int mNumVerticalHulls=10;
        Camera camera;

        GameTime _game_time = null;

        Archer player1 = null;
        Mage player2 = null;

        public Player[] playeri = new Player[2];

        private DebugViewXNA debug;
        private bool IsDebug=false;
        Block ground = null;
        private Block ground2;


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

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            camera = new Camera(ScreenManager.GraphicsDevice.Viewport);
            camera.position = new Vector2(0, -5);
        }
        List<Block> blocks = new List<Block>();
        Random r;
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            gameFont = content.Load<SpriteFont>("menufont");
            r = new Random((int)DateTime.Now.Ticks);
            ScreenManager.Game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            this.krypton = new KryptonEngine(ScreenManager.Game, "KryptonEffect");
            krypton.SpriteBatchCompatablityEnabled = true;
            krypton.CullMode = CullMode.None;
            ModifierList list=new ModifierList();
            world = new World(new Vector2(0, 40.8f));
            List<Modifier> modlist = new List<Modifier>();
            modlist.Add(list.getSizeMod());
            modlist.Add(list.getMod());
            player1 = new Archer(world, content, -20, -10, modlist);
            modlist.Clear();
            modlist.Add(list.getSizeMod());
            modlist.Add(list.getMod());
            player2 = new Mage(world, content, 20, -10, modlist);
            playeri[0] = player1;
            playeri[1] = player2;
            
            
            ground = new Block(world,krypton, content, 0, 1,100,1,"ground");
            //ground2 = new Block(world, krypton, content, -3, -3,1,2.5f,"wall");
            ground3 = new Block(world, krypton, content, -30, -10, 2, 21, "wall");
            ground4 = new Block(world, krypton, content, 30, -10, 2, 21, "wall");
            ground5 = new Block(world, krypton, content, 0, -20, 100,1, "wall");
            blocks.Add(ground);
            //blocks.Add(ground2);
            blocks.Add(ground3);
            blocks.Add(ground4);
            blocks.Add(ground5);
            blocks.Add(new Block(world, krypton, content, -15, -3, 15, 1, "wall"));
            blocks.Add(new Block(world, krypton, content, 15, -3, 15, 1, "wall"));
            blocks.Add(new Block(world, krypton, content, 0, -8.5f, 30, 1, "wall"));
            this.krypton.Initialize();
            camera = new Camera(ScreenManager.GraphicsDevice.Viewport);
            mouseCamera = new Camera(ScreenManager.GraphicsDevice.Viewport);
            camera.position += new Vector2(0, -5);
            
            Console.WriteLine(camera.IsFollowing);
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            //Thread.Sleep(1000);
            dirGamepad = new Vector2[4];
            dirGamepad[0] =- Vector2.UnitY;
            dirGamepad[1] = -Vector2.UnitY;
            dirGamepad[2] = -Vector2.UnitY;
            dirGamepad[3] = -Vector2.UnitY;
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            debug = new DebugViewXNA(world);
            debug.LoadContent(ScreenManager.GraphicsDevice, content);
            this.mLightTexture = LightTextureBuilder.CreatePointLight(ScreenManager.GraphicsDevice, 512);
            Light2D light = new Light2D()
            {
                Texture = mLightTexture,
                Range = 750,
                Color = Color.White,
                Intensity = 0.8f,
                X = 0,
                Y = -615,
                Angle=-(float)Math.PI*3/2,
                Fov=(float)Math.PI/4,
               
            };
            krypton.Lights.Add(light);
            krypton.Lights.Add(new Light2D()
            {
                Texture = mLightTexture,
                Range = 750,
                Color = Color.Gold,
                Intensity = 0.8f,
                X = -300,
                Y = -615,
                Angle = -(float)Math.PI * 3 / 2,
                Fov = (float)Math.PI / 4,
            });
            krypton.Lights.Add(new Light2D()
            {
                Texture = mLightTexture,
                Range = 750,
                Color = Color.Gold,
                Intensity = 0.8f,
                X = 300,
                Y = -615,
                Angle = -(float)Math.PI * 3 / 2,
                Fov = (float)Math.PI / 4,
            });
            krypton.Lights.Add(new Light2D()
            {
                Texture = mLightTexture,
                Range = 750,
                Color = Color.Silver,
                Intensity = 0.8f,
                X = -600,
                Y = -615,
                Angle = -(float)Math.PI * 3 / 2,
                Fov = (float)Math.PI / 4,
            });
            krypton.Lights.Add(new Light2D()
            {
                Texture = mLightTexture,
                Range = 750,
                Color = Color.Silver,
                Intensity = 0.8f,
                X = 600,
                Y = -615,
                Angle = -(float)Math.PI * 3 / 2,
                Fov = (float)Math.PI / 4,
            });
            krypton.Lights.Add(new Light2D()
            {
                Texture = mLightTexture,
                Range = 500,
                Color = Color.White,
                Intensity = 0.7f,
                X = 0,
                Y = 0,
                Angle = (float)Math.PI * 3 / 2,
                //Fov = (float)Math.PI / 4,
            });
          
            //for (int i = -20; i <= 20;i++)
                //addObject(i*20, 50, 20, 20);
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
            _game_time = gameTime;
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

                player1.Update(gameTime);
                player2.Update(gameTime);
                
                player1.update_archer(gameTime);
                player2.update_mage(gameTime);

                if (player2.my_body.ContactList == null)
                {
                    player2.can_jump = false;
                }
                if (player1.my_body.ContactList == null)
                {
                    player1.can_jump = false;
                }
                if(player1.HP<=0&&player2.HP>0)
                {
                    this.ExitScreen();
                    GameplayScreen gps = new GameplayScreen();
                    ScreenManager.AddScreen(gps, 0);
                    
                    ScreenManager.AddScreen(new VersusScreen(gps.playeri,0,1), 0);
                }
                else if(player2.HP<=0&&player1.HP>0)
                {
                    this.ExitScreen();
                    GameplayScreen gps = new GameplayScreen();
                    ScreenManager.AddScreen(gps,0);
                    ScreenManager.AddScreen(new VersusScreen(gps.playeri,1,0), 0);
                }

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

            if (player1.can_jump)
            {
                player1.move_on_ground();
            }
            else
            {
                player1.controlAir();
            }

            if (player2.can_jump)
            {
                player2.move_on_ground();
            }
            else
            {
                player2.controlAir();
            }
        }

        Vector2[] dirGamepad;
        private Block ground3;
        private Block ground4;
        private Block ground5;
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
                ScreenManager.AddScreen(new VersusScreen(playeri),0);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;
                Vector2 movement2 = Vector2.Zero;
                PlayerIndex piout;
                //Console.WriteLine((int)playerIndex);

                
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    player1.can_move_left = true;
                    player1.btn_move_left = true;
                }
                else
                {
                    player1.can_move_left = false;
                    player1.btn_move_left = false;
                }
                    //movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    player1.can_move_right = true;
                    player1.btn_move_right = true;
                }
                else
                {
                    player1.can_move_right = false;
                    player1.btn_move_right = false;
                }
                    //movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    player1.btn_jump = true;
                }
                else
                {
                    player1.btn_jump = false;
                }
                    //movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    
                }
                    //movement.Y++;
                if (keyboardState.IsKeyDown(Keys.OemMinus))
                    camera.Scale -= 0.01f;

                if (keyboardState.IsKeyDown(Keys.OemPlus))
                    camera.Scale += 0.01f;

                MoveClass.move_player_and_camera(playeri, camera, ScreenManager.GraphicsDevice.Viewport);
                Vector2 direction;
                    int mx = input.MouseState.X;
                    int my = input.MouseState.Y;
                    float mapX = mx + Conversions.to_pixels(camera.position.X) * camera.Scale - ScreenManager.GraphicsDevice.Viewport.Width/2;
                    float mapY = my + Conversions.to_pixels(camera.position.Y) * camera.Scale - ScreenManager.GraphicsDevice.Viewport.Height / 2;
                    mapX /= camera.Scale;
                    mapY /= camera.Scale;
                    direction = new Vector2(mapX, mapY) - Conversions.to_pixels(player1.my_body.Position);
                    direction.Normalize();
                    float angle = (float)Math.Atan2(direction.X, -direction.Y);
                    player1.setAngle(angle);
                //GameTime _game_time = new GameTime();
                if (input.MouseState.LeftButton == ButtonState.Pressed && input.LastMouseState.LeftButton == ButtonState.Released)
                {
                    
                    
                    Console.WriteLine(direction.ToString()+"="+new Vector2(input.MouseState.X,input.MouseState.Y).ToString()+"-"+Conversions.to_pixels(player1.my_body.Position).ToString()+"+"+camera.Position.ToString()+" camera scale"+camera.Scale.ToString());

                    
                    player1.atack(direction*player1.stat.projectile_speed, 1, _game_time);
                }
               if(input.IsNewKeyPress(Keys.N,PlayerIndex.One,out piout))
               {
                   this.ExitScreen();
                   GameplayScreen gps = new GameplayScreen();
                   ScreenManager.AddScreen(gps,0);
                   ScreenManager.AddScreen(new VersusScreen(gps.playeri), 0);
               }
                    if (input.CurrentKeyboardStates[playerIndex].IsKeyDown(Keys.X) && input.LastKeyboardStates[playerIndex].IsKeyUp(Keys.X))
                    {
                       
                        Console.WriteLine(direction.ToString() + "=" + new Vector2(input.MouseState.X, input.MouseState.Y).ToString() + "-" + Conversions.to_pixels(player2.my_body.Position).ToString() + "+" + camera.Position.ToString() + " camera scale" + camera.Scale.ToString());

                      
                        player2.atack(direction * 15f, 1, _game_time);
                    }

                Vector2 thumbstick = input.CurrentGamePadStates[playerIndex].ThumbSticks.Left;

               
                movement2.X += thumbstick.X;
                if(thumbstick.Y > 0)
                {
                    player2.btn_jump = true;
                }
                else
                {
                    player2.btn_jump = false;
                }
                if(thumbstick.X > 0)
                {
                    player2.btn_move_right = true;
                    player2.can_move_right = true;
                }
                else
                {
                    player2.btn_move_right = false;
                    player2.can_move_right = false;
                }

                if (thumbstick.X < 0)
                {
                    player2.btn_move_left = true;
                    player2.can_move_left = true;
                }
                else
                {
                    player2.btn_move_left = false;
                    player2.can_move_left = false;
                }

                Vector2 dir2 = input.CurrentGamePadStates[playerIndex].ThumbSticks.Right * 100;
                dir2.Y *= -1;

                if (dir2 != Vector2.Zero)
                    dirGamepad[playerIndex] = dir2;
                dirGamepad[playerIndex].Normalize();
                player2.setAngle((float)Math.Atan2(dirGamepad[playerIndex].X, -dirGamepad[playerIndex].Y));
                if (input.CurrentGamePadStates[playerIndex].Triggers.Right!=0 && input.LastGamePadStates[playerIndex].Triggers.Right==0)
                {
                    player2.atack(dirGamepad[playerIndex]* player2.stat.projectile_speed, 1, _game_time);
                }

                //movement2.Y -= thumbstick.Y;
                /*if (input.IsNewButtonPress(Buttons.X, 0, out piout))
                    //my_archer2.my_body.ApplyLinearImpulse(new Vector2(0, -10));

                if (movement.Length() > 1)
                    movement.Normalize();
                if (movement2.Length() > 1)
                    movement2.Normalize();

                if (movement != Vector2.Zero)
                {
                    //my_archer.my_body.LinearVelocity = (movement*5);
                    //my_archer.my_body.LinearVelocity *= 0.8f;
                }
               // my_archer2.my_body.ApplyLinearImpulse(movement2);*/
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
            
           // camera.position = new Vector2(20, 20);
           
            this.krypton.Matrix = camera.View;
            this.krypton.Bluriness = 3;
            //krypton.AmbientColor = Color.White;
            this.krypton.LightMapPrepare();

            // Make sure we clear the backbuffer *after* Krypton is done pre-rendering
            ScreenManager.GraphicsDevice.Clear(Color.White);

            // ----- DRAW STUFF HERE ----- //
            // By drawing here, you ensure that your scene is properly lit by krypton.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.View);

            player1.Draw(spriteBatch);
            foreach (var b in blocks)
                b.Draw(spriteBatch);
            //my_archer.animation.Draw(spriteBatch,Vector2.Zero,30,30);
            player2.Draw(spriteBatch);
            spriteBatch.End();
            // Drawing after KryptonEngine.Draw will cause you objects to be drawn on top of the lightmap (can be useful, fyi)
            // ----- DRAW STUFF HERE ----- //

            // Draw krypton (This can be omited if krypton is in the Component list. It will simply draw krypton when base.Draw is called
            this.krypton.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.View);
            player1.DrawTinta(spriteBatch);
            player2.DrawTinta(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            player1.DrawUI(player1, spriteBatch, 0, ScreenManager.GraphicsDevice.Viewport);
            player2.DrawUI(player2, spriteBatch, 1, ScreenManager.GraphicsDevice.Viewport);
            spriteBatch.End();
            // Draw the shadow hulls as-is (no shadow stretching) in pure white on top of the shadows
            // You can omit this line if you want to see what the light-map looks like :)
            if (IsDebug)
            {
                debug.RenderDebugData(ref camera.Projection, ref camera.DebugView);
            }
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
