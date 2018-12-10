using System;
using System.Collections.Generic;

using DeenGames.AbdullahTheAlp.Ecs;

namespace DeenGames.AbdullahTheAlp.Ecs
{
    /// <summary>
    /// Just a simple collection of entities. You can't have dupes, though.
    /// Insert is O(1), retrieval by type is O(1).
    public class Entity
    {
        private IDictionary<Type, AbstractComponent> components = new Dictionary<Type, AbstractComponent>();

        public void Set(AbstractComponent component)
        {
            var key = component.GetType();
            this.components[key] = component;
        }

        public AbstractComponent Get(Type componentType)
        {
            return this.components[componentType];
        }
    }
}