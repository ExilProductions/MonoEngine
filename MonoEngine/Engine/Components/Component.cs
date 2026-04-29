using Microsoft.Xna.Framework;
using MonoGameTemplate.Managers;

namespace MonoGameTemplate.Engine.Components
{
    public abstract class Component
    {
        public Entity Entity { get; internal set; }
        public Transform Transform => Entity?.Transform;
        public bool IsEnabled { get; set; } = true;

        public virtual void OnAdded()
        {
        }

        public virtual void OnRemoved()
        {
        }

        public virtual void OnUpdate(GameTime gameTime)
        {
        }
    }
}
