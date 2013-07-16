using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public class MessageBroker : IMessageBroker
    {
        private IDictionary<string, IList<Action<object>>> subscribers;

        public MessageBroker()
        {
            subscribers = new Dictionary<string, IList<Action<object>>>();
        }

        public void Subscribe(string name, Action<object> callback)
        {
            if (callback == null)
                return;

            if (!subscribers.ContainsKey(name))
                subscribers[name] = new List<Action<object>>();

            subscribers[name].Add(callback);
        }

        public void Publish(string name, object parameter)
        {
            if (!subscribers.ContainsKey(name))
                return;

            var callbacks = subscribers[name];
            for (int i = 0; i < callbacks.Count; i++)
                callbacks[i](parameter);
        }


        public void Unsubscribe(string name, Action<object> callback)
        {
            if (!subscribers.ContainsKey(name))
                return;

            subscribers[name].Remove(callback);
        }
    }
}
