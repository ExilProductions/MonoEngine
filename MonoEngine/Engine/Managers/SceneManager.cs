using Microsoft.Xna.Framework;
using MonoGameTemplate.Engine.Components;
using MonoGameTemplate.Engine.SceneManagment;

namespace MonoGameTemplate.Managers
{
    public class SceneManager : ManagerBase
    {
        private MainGame _game;
        private System.Type _pendingSceneType;
        private bool _contentLoadCompleted;

        public SceneBase ActiveScene { get; private set; }

        public override void OnInit()
        {
            _game = MainGame.Instance ?? throw new System.InvalidOperationException("MainGame.Instance is not initialized.");
        }

        public override void OnContentLoad()
        {
            // we run after other managers that we need (e.g. RenderManager default effect) because of Program registration order
            _contentLoadCompleted = true;
            TryApplyPendingScene();
        }

        public TScene LoadScene<TScene>() where TScene : SceneBase, new()
        {
            LoadScene(typeof(TScene));
            return ActiveScene as TScene;
        }

        public SceneBase LoadScene(System.Type sceneType)
        {
            if (sceneType == null)
                throw new System.ArgumentNullException(nameof(sceneType));
            if (!typeof(SceneBase).IsAssignableFrom(sceneType))
                throw new System.ArgumentException("Scene type must inherit from SceneBase.", nameof(sceneType));

            _pendingSceneType = sceneType;
            TryApplyPendingScene();
            return ActiveScene;
        }

        public SceneBase ReloadActiveScene()
        {
            if (ActiveScene == null)
                throw new System.InvalidOperationException("No active scene loaded.");

            return LoadScene(ActiveScene.GetType());
        }

        private void TryApplyPendingScene()
        {
            if (_pendingSceneType == null)
                return;
            if (!_contentLoadCompleted)
                return;

            var scene = (SceneBase)System.Activator.CreateInstance(_pendingSceneType);
            InitializeScene(scene);
            _pendingSceneType = null;
        }

        private void InitializeScene(SceneBase scene)
        {
            ActiveScene = scene ?? throw new System.ArgumentNullException(nameof(scene));

            var rootEntity = new Entity("Root");
            ActiveScene.RootEntity = rootEntity;
            ActiveScene.OnSceneLoaded(rootEntity);
        }

        public Entity CreateEntity(string name = "Entity", Entity parent = null)
        {
            if (ActiveScene == null)
                throw new System.InvalidOperationException("No active scene loaded.");

            var entity = new Entity(name);
            var targetParent = parent ?? ActiveScene.RootEntity;

            if (targetParent == null)
                throw new System.InvalidOperationException("Active scene root entity is missing.");

            targetParent.AddChild(entity);
            return entity;
        }

        public bool RemoveEntity(Entity entity)
        {
            if (entity == null || ActiveScene?.RootEntity == null || ReferenceEquals(entity, ActiveScene.RootEntity))
                return false;

            if (entity.Parent != null)
                return entity.Parent.RemoveChild(entity);

            return ActiveScene.RootEntity.RemoveChild(entity);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            if (ActiveScene?.RootEntity == null)
                return;

            UpdateEntity(ActiveScene.RootEntity, gameTime);
        }

        private void UpdateEntity(Entity entity, GameTime gameTime)
        {
            if (!entity.IsActive)
                return;

            for (int i = 0; i < entity.Components.Count; i++)
            {
                var component = entity.Components[i];
                if (component.IsEnabled)
                    component.OnUpdate(gameTime);
            }

            for (int i = 0; i < entity.Children.Count; i++)
                UpdateEntity(entity.Children[i], gameTime);
        }
    }
}
