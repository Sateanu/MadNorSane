#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
#endregion

namespace MadNorSane
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 4;

        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public  MouseState MouseState;
        public  MouseState LastMouseState;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        GameTime gameTime;
        TimeSpan timeFromPrev;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }
        }


        public bool IsScroolUp()
        {
            return LastMouseState.ScrollWheelValue < MouseState.ScrollWheelValue;
        }
        public bool IsScroolDown()
        {
            return LastMouseState.ScrollWheelValue > MouseState.ScrollWheelValue;
        }

        public bool IsKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                        out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;
                return (CurrentKeyboardStates[i].IsKeyDown(key));
            }
            else
                return (IsKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyPress(key, PlayerIndex.Four, out playerIndex));
        }
        public bool IsKeyPressTime(Keys key, PlayerIndex? controllingPlayer,
                                        out PlayerIndex playerIndex,float seconds)
        {
            
            if (controllingPlayer.HasValue)
            {
                
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;
                if (gameTime.TotalGameTime - timeFromPrev < TimeSpan.FromSeconds(seconds))
                {
                    return false;
                }
                if (CurrentKeyboardStates[i].IsKeyDown(key))
                {
                    timeFromPrev = gameTime.TotalGameTime;
                    return true;
                }
                else
                    return false;
            }
            else
                return (IsKeyPressTime(key, PlayerIndex.One, out playerIndex,seconds) ||
                        IsKeyPressTime(key, PlayerIndex.Two, out playerIndex,seconds) ||
                        IsKeyPressTime(key, PlayerIndex.Three, out playerIndex,seconds) ||
                        IsKeyPressTime(key, PlayerIndex.Four, out playerIndex,seconds));
        }
        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }
        
        public bool IsButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }
        public bool IsButtonPressTime(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex,float seconds)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;
                if (gameTime.TotalGameTime - timeFromPrev < TimeSpan.FromSeconds(seconds))
                {
                    return false;
                }
                else
                {
                    if (CurrentGamePadStates[i].IsButtonDown(button))
                    {
                        timeFromPrev = gameTime.TotalGameTime;
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressTime(button, PlayerIndex.One, out playerIndex,seconds) ||
                        IsButtonPressTime(button, PlayerIndex.Two, out playerIndex,seconds) ||
                        IsButtonPressTime(button, PlayerIndex.Three, out playerIndex,seconds) ||
                        IsButtonPressTime(button, PlayerIndex.Four, out playerIndex,seconds));
            }
        }
        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer, float seconds)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                   IsButtonPressTime(Buttons.DPadUp, controllingPlayer, out playerIndex,seconds) ||
                   IsButtonPressTime(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex,seconds);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer, float seconds)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                   IsButtonPressTime(Buttons.DPadDown, controllingPlayer, out playerIndex,seconds) ||
                   IsButtonPressTime(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex,seconds);
        }

        public bool IsMenuLeft(PlayerIndex? controllingPlayer, float seconds )
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Left, controllingPlayer, out playerIndex) ||
                   IsButtonPressTime(Buttons.DPadLeft, controllingPlayer, out playerIndex,seconds) ||
                   IsButtonPressTime(Buttons.LeftThumbstickLeft, controllingPlayer, out playerIndex,seconds);
        }

        public bool IsMenuRight(PlayerIndex? controllingPlayer, float seconds)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Right, controllingPlayer, out playerIndex) ||
                   IsButtonPressTime(Buttons.DPadRight, controllingPlayer, out playerIndex,seconds) ||
                   IsButtonPressTime(Buttons.LeftThumbstickRight, controllingPlayer, out playerIndex,seconds);
        }

        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        #endregion

        
    }
}
