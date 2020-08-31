using System;
using System.Messaging;
using MSMQ.Core.Common;
using MSMQ.Core.Helpers;

namespace MSMQ.Core
{
    [Serializable]
    public sealed class MhMessage : IMhMessage
    {
        public string Label { get; set; }
        public DateTime ArrivedTime { get; set; }
        public DateTime SentTime { get; set; }
        public string Id { get; set; }
        public object Body { get; set; }
        MhMessage() { }
        public MhMessage(Message msg)
        {
            Convert(msg);
        }
       
        public static IMhMessage Convert(Message msg)
        {
            msg.Formatter = new JsonMessageFormatter();

            var mhMessage = new MhMessage
            {
                Label = msg.Label,
                Body = msg.Body,
                ArrivedTime = msg.ArrivedTime,
                SentTime = msg.SentTime,
                Id = msg.Id
            };

            return mhMessage;
        }
    }
}
