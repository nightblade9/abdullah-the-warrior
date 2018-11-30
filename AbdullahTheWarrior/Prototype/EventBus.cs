using System;
using System.Collections;
using System.Collections.Generic;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    public class EventBus
    {
        private Dictionary<string, List<Action<object>>> eventListeners = new Dictionary<string, List<Action<object>>>();

        public static EventBus Instance { get; private set; } = new EventBus();

        private EventBus()
        {
            EventBus.Instance = this;
        }

        public void AddListener(string eventName, Action<object> listener)
        {
            if (!this.eventListeners.ContainsKey(eventName))
            {
                this.eventListeners[eventName] = new List<Action<object>>();
            }

            this.eventListeners[eventName].Add(listener);
        }

        public void Broadcast(string eventName, object data)
        {
            if (this.eventListeners.ContainsKey(eventName))
            {
                foreach (var listener in this.eventListeners[eventName])
                {
                    listener.Invoke(data);
                }
            }
        }
    }
}