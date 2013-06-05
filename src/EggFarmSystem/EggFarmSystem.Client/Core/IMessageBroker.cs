using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public interface IMessageBroker
    {
        void Subscribe(string name, Action<object> callback);

        void Publish(string name, object parameter);
    }
}
