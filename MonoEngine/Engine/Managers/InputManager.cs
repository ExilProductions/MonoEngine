using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTemplate.Managers
{
    public class InputManager : ManagerBase
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private readonly GamePadState[] _currentGamePadStates = new GamePadState[4];
        private readonly GamePadState[] _previousGamePadStates = new GamePadState[4];

        public override void OnInit()
        {
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = _currentKeyboardState;
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;

            for (int i = 0; i < _currentGamePadStates.Length; i++)
            {
                _currentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                _previousGamePadStates[i] = _currentGamePadStates[i];
            }
        }

        public override void OnUpdate(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            for (int i = 0; i < _currentGamePadStates.Length; i++)
            {
                _previousGamePadStates[i] = _currentGamePadStates[i];
                _currentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        public bool GetKey(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool GetKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool GetKeyUp(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public bool GetMouseButton(int button)
        {
            return GetCurrentMouseButtonState(button) == ButtonState.Pressed;
        }

        public bool GetMouseButtonDown(int button)
        {
            return GetCurrentMouseButtonState(button) == ButtonState.Pressed &&
                   GetPreviousMouseButtonState(button) == ButtonState.Released;
        }

        public bool GetMouseButtonUp(int button)
        {
            return GetCurrentMouseButtonState(button) == ButtonState.Released &&
                   GetPreviousMouseButtonState(button) == ButtonState.Pressed;
        }

        public bool GetButton(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
        {
            int index = ToPlayerIndex(playerIndex);
            return _currentGamePadStates[index].IsButtonDown(button);
        }

        public bool GetButtonDown(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
        {
            int index = ToPlayerIndex(playerIndex);
            return _currentGamePadStates[index].IsButtonDown(button) &&
                   _previousGamePadStates[index].IsButtonUp(button);
        }

        public bool GetButtonUp(Buttons button, PlayerIndex playerIndex = PlayerIndex.One)
        {
            int index = ToPlayerIndex(playerIndex);
            return _currentGamePadStates[index].IsButtonUp(button) &&
                   _previousGamePadStates[index].IsButtonDown(button);
        }

        private static int ToPlayerIndex(PlayerIndex playerIndex)
        {
            return (int)playerIndex;
        }

        private ButtonState GetCurrentMouseButtonState(int button)
        {
            return button switch
            {
                0 => _currentMouseState.LeftButton,
                1 => _currentMouseState.RightButton,
                2 => _currentMouseState.MiddleButton,
                _ => throw new System.ArgumentOutOfRangeException(nameof(button), "Mouse button must be 0 (left), 1 (right), or 2 (middle).")
            };
        }

        private ButtonState GetPreviousMouseButtonState(int button)
        {
            return button switch
            {
                0 => _previousMouseState.LeftButton,
                1 => _previousMouseState.RightButton,
                2 => _previousMouseState.MiddleButton,
                _ => throw new System.ArgumentOutOfRangeException(nameof(button), "Mouse button must be 0 (left), 1 (right), or 2 (middle).")
            };
        }
    }
}
