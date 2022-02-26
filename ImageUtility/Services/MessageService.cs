using ImageUtility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUtility.Services
{
    internal class MessageService : IMessageService
    {
        public void Send<TMessage>(TMessage message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TMessage>(object Subscriber, Action<object> action)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe<TMessage>(object Subscriber)
        {
            throw new NotImplementedException();
        }
    }
}
