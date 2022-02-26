using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUtility.Interfaces
{
    internal interface IMessageService
    {
        void Send<TMessage>(TMessage message);
        void Subscribe<TMessage>(object Subscriber, Action<object> action);
        void UnSubscribe<TMessage>(object Subscriber);
    }
}
