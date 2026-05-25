using System;
using Microsoft.Xna.Framework;
using MonoGameTemplate.Managers;

namespace MonoGameTemplate.Engine.Components
{
    public class Camera : Component
    {
        private RenderManager _renderManager;

        public float FieldOfView { get; set; } = MathHelper.PiOver4;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 1000f;

        // View matrix derived from the entity's world transform
        public Matrix ViewMatrix
        {
            get
            {
                Matrix world = Entity.GetWorldMatrix();
                Vector3 position = world.Translation;
                Vector3 forward = Vector3.Transform(Vector3.Forward, Entity.Transform.Rotation);
                Vector3 up = Vector3.Transform(Vector3.Up, Entity.Transform.Rotation);
                return Matrix.CreateLookAt(position, position + forward, up);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                var viewport = MainGame.Instance.GraphicsDevice.Viewport;
                float aspect = (float)viewport.Width / viewport.Height;
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, aspect, NearPlane, FarPlane);
            }
        }

        public override void OnAdded()
        {
            _renderManager = MainGame.Instance?.GetManager<RenderManager>()
                ?? throw new InvalidOperationException("RenderManager must be registered before using Camera.");
            _renderManager.RegisterCamera(this);
        }

        public override void OnRemoved()
        {
            _renderManager?.UnregisterCamera(this);
            _renderManager = null;
        }
    }
}
