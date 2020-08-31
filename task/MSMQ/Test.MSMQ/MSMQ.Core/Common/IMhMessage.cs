using System;

namespace MSMQ.Core.Common
{
    public interface IMhMessage
    {
        object Body { get; set; }
        string Id { get; set; }
        string Label { get; set; }
        DateTime ArrivedTime { get; set; }
        DateTime SentTime { get; set; }
    }
}

