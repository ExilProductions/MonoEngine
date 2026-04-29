using MonoGameTemplate.Engine.Components;

namespace MonoGameTemplate.Engine.SceneManagment
{
    public class SceneBase
    {
        public Entity RootEntity { get; internal set; }

        public virtual void OnSceneLoaded(Entity rootEntity)
        {
        }
    }
}
