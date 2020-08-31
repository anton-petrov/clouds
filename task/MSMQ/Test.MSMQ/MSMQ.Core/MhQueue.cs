using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using MSMQ.Core.Common;
using MSMQ.Core.Helpers;

namespace MSMQ.Core
{
    public sealed class MhQueue : IMhQueue
    {
        private const string QueuePrefix = @".\private$\";
        private readonly MessageQueue queue;

        public MhQueue(string name)
        {
            if (!IsQueueExist(name))
                throw new Exception($"The {name} not found");

            queue = new MessageQueue(Path(name));
        }

        public MhQueue(MessageQueue queue)
        {
            if (queue == null || !IsQueueExist(queue.Path))
                throw new Exception("The queue not found");

            this.queue = new MessageQueue(queue.Path);
        }

        public void Send(object message, string label = "")
        {
            var msqm = new Message
            {
                Formatter = new JsonMessageFormatter(),
                Body = message,
                Label = label
            };

            queue.Send(msqm);
        }

        public IMhMessage Receive()
        {
            queue.MessageReadPropertyFilter.SetAll();
            return MhMessage.Convert(queue.Receive());
        }

        public IMhMessage Receive(string id)
        {
            queue.MessageReadPropertyFilter.SetAll();
            return MhMessage.Convert(queue.ReceiveById(id));
        }

        public IMhMessage Receive(TimeSpan timeout)
        {
            queue.MessageReadPropertyFilter.SetAll();
            return MhMessage.Convert(queue.Receive(timeout));
        }

        public IEnumerable<IMhMessage> GetMessages()
        {
            queue.MessageReadPropertyFilter.SetAll();
            return queue.GetAllMessages().Select(MhMessage.Convert);
        }

        public IMhMessage Peek()
        {
            queue.MessageReadPropertyFilter.SetAll();
            return MhMessage.Convert(queue.Peek());
        }

        public IMhMessage Peek(string id)
        {
            queue.MessageReadPropertyFilter.SetAll();
            return MhMessage.Convert(queue.PeekById(id));
        }

        public void Purge()
        {
            queue.Purge();
        }

        public string Name { get; set; }

        public int Count => queue.GetAllMessages().Length;

        public Guid Id => queue.Id;

        public string Label { get; set; }

        private static string Path(string name)
        {
            if (name.Contains(QueuePrefix))
                return name;

            return QueuePrefix + name;
        }

        public static MhQueue Create(string name)
        {
            var queue = MessageQueue.Create(Path(name));
            return new MhQueue(queue);
        }

        public static bool IsQueueExist(string name)
        {
            return MessageQueue.Exists(Path(name));
        }

        public static void Delete(string name)
        {
            MessageQueue.Delete(Path(name));
        }
    }
}
