using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTemplate.Engine.Components;

namespace MonoGameTemplate.Managers
{
    public class RenderManager : ManagerBase
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        // we keep separate lists so camera lookups are O(1) for the common single-camera case
        private readonly List<Camera> _cameras = new();
        private readonly List<MeshRenderer> _renderers = new();

        public Color ClearColor { get; set; } = Color.CornflowerBlue;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        // First registered camera is the active one; future priority system can reorder the list
        public Camera ActiveCamera => _cameras.Count > 0 ? _cameras[0] : null;

        public override void OnInit()
        {
            // OnInit runs after LoadContent in MonoGame's pipeline; GraphicsDevice is already set by here
            _graphicsDevice = MainGame.Instance.GraphicsDevice
                ?? throw new System.InvalidOperationException("GraphicsDevice is not available.");
        }

        public override void OnContentLoad()
        {
            // base.Initialize() calls LoadContent before our OnInit, so we resolve GraphicsDevice here directly
            _graphicsDevice = MainGame.Instance.GraphicsDevice
                ?? throw new System.InvalidOperationException("GraphicsDevice is not available during content load.");
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public override void OnDraw(GameTime gameTime)
        {
            _graphicsDevice.Clear(ClearColor);

            Camera camera = ActiveCamera;
            if (camera == null)
                return;

            // Reset render state - SpriteBatch or prior 2D passes may have stomped these
            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
            _graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            _graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            for (int i = 0; i < _renderers.Count; i++)
            {
                var renderer = _renderers[i];
                if (renderer.IsEnabled && renderer.Entity != null && renderer.Entity.IsActive)
                    renderer.Draw(camera);
            }
        }

        public void RegisterCamera(Camera camera)
        {
            if (camera != null && !_cameras.Contains(camera))
                _cameras.Add(camera);
        }

        public void UnregisterCamera(Camera camera)
        {
            _cameras.Remove(camera);
        }

        public void RegisterRenderer(MeshRenderer renderer)
        {
            if (renderer != null && !_renderers.Contains(renderer))
                _renderers.Add(renderer);
        }

        public void UnregisterRenderer(MeshRenderer renderer)
        {
            _renderers.Remove(renderer);
        }
    }
}
