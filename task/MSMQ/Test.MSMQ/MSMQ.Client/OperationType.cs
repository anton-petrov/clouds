namespace MSMQ.Client
{
    public enum OperationType
    {
        CreateQueue = 1,
        DeleteQueue,
        SendMessage,
        ReceiveMessage,
        PeekMessage,
        PeekAll,
        DeleteAllMessages
    }
}