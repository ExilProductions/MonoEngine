using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTemplate.Managers;

namespace MonoGameTemplate.Engine.Components
{
    public class MeshRenderer : Component
    {
        private RenderManager _renderManager;
        private Model _model;
        private string _modelAssetName;

        public Color Color { get; set; } = Color.White;
        public Model Model => _model;

        // Can be called before or after AddComponent - both orderings are handled
        public void SetModel(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName))
                throw new ArgumentException("Asset name cannot be null or empty.", nameof(assetName));

            _modelAssetName = assetName;

            if (_renderManager != null)
                LoadModel();
        }

        public override void OnAdded()
        {
            _renderManager = MainGame.Instance?.GetManager<RenderManager>()
                ?? throw new InvalidOperationException("RenderManager must be registered before using MeshRenderer.");

            if (!string.IsNullOrWhiteSpace(_modelAssetName))
                LoadModel();

            _renderManager.RegisterRenderer(this);
        }

        public override void OnRemoved()
        {
            _renderManager?.UnregisterRenderer(this);
            _renderManager = null;
        }

        // Called by RenderManager each frame
        internal void Draw(Camera camera)
        {
            if (_model == null || camera == null)
                return;

            Matrix world = Entity.GetWorldMatrix();
            bool tinted = Color != Color.White;

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                    effect.EnableDefaultLighting();

                    // only override DiffuseColor when the user explicitly wants a tint
                    if (tinted)
                        effect.DiffuseColor = Color.ToVector3();
                }

                mesh.Draw();
            }
        }

        private void LoadModel()
        {
            _model = MainGame.Instance.Content.Load<Model>(_modelAssetName);
            
            var gd = MainGame.Instance.GraphicsDevice;
            var textureAssetName = _model.Tag as string;
            Texture2D texture = textureAssetName != null
                ? MainGame.Instance.Content.Load<Texture2D>(textureAssetName)
                : null;

            foreach (ModelMesh mesh in _model.Meshes)
                Console.WriteLine($"[MeshRenderer] '{mesh.Name}' BS_radius={mesh.BoundingSphere.Radius:F3} parentBone_scale={mesh.ParentBone.Transform.M11:F4}");

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    var effect = new BasicEffect(gd);
                    if (texture != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = texture;
                    }
                    part.Effect = effect;
                }
            }
        }
    }
}
