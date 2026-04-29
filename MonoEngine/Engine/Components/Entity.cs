using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameTemplate.Engine.Components
{
    public class Entity
    {
        private readonly List<Component> _components = new();
        private readonly List<Entity> _children = new();

        public Entity(string name = "Entity")
        {
            Name = name;
            Transform = new Transform();
        }

        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public Entity Parent { get; private set; }
        public Transform Transform { get; }
        public IReadOnlyList<Component> Components => _components;
        public IReadOnlyList<Entity> Children => _children;

        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T();
            AddComponent(component);
            return component;
        }

        public void AddComponent(Component component)
        {
            if (component == null)
                throw new System.ArgumentNullException(nameof(component));
            if (component.Entity != null)
                throw new System.InvalidOperationException("Component is already attached to an entity.");

            component.Entity = this;
            _components.Add(component);
            component.OnAdded();
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T typed)
                    return typed;
            }

            return null;
        }

        public bool RemoveComponent(Component component)
        {
            if (component == null)
                return false;

            if (!_components.Remove(component))
                return false;

            component.OnRemoved();
            component.Entity = null;
            return true;
        }

        public void AddChild(Entity child)
        {
            if (child == null)
                throw new System.ArgumentNullException(nameof(child));
            if (ReferenceEquals(child, this))
                throw new System.InvalidOperationException("Entity cannot be parent of itself.");
            if (child.Parent != null)
                throw new System.InvalidOperationException("Entity already has a parent.");

            child.Parent = this;
            _children.Add(child);
        }

        public bool RemoveChild(Entity child)
        {
            if (child == null)
                return false;

            if (!_children.Remove(child))
                return false;

            child.Parent = null;
            return true;
        }

        public Matrix GetWorldMatrix()
        {
            Matrix world = Transform.LocalMatrix;
            Entity current = Parent;

            while (current != null)
            {
                world *= current.Transform.LocalMatrix;
                current = current.Parent;
            }

            return world;
        }
    }
}
