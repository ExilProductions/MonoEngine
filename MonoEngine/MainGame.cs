using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTemplate.Managers;

namespace MonoGameTemplate
{
    public class MainGame : Game
    {
        public static MainGame Instance { get; private set; }

        private GraphicsDeviceManager _graphics;
        private readonly List<ManagerBase> _managers = new();

        public GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
        }

        public MainGame()
        {
            if (Instance == null)
                Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public T RegisterManager<T>() where T : ManagerBase, new()
        {
            var manager = new T();
            _managers.Add(manager);
            return manager;
        }

        public T GetManager<T>() where T : ManagerBase
        {
            for (int i = 0; i < _managers.Count; i++)
            {
                if (_managers[i] is T typed)
                    return typed;
            }

            return null;
        }

        protected override void Initialize()
        {
            // we need the graphics device and platform init first otherwise render state in managers can hit a null GL context
            base.Initialize();

            foreach (var manager in _managers)
            {
                manager.OnInit();
            }
        }

        protected override void LoadContent()
        {
            foreach(var manager in _managers)
            {
                manager.OnContentLoad();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            foreach(var manager in _managers)
            {
                manager.OnUpdate(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            foreach(var manager in _managers)
            {
                manager.OnDraw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
