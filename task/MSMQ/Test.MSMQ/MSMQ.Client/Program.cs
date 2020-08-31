using System;
using System.Collections.Generic;
using MSMQ.Core;
using MSMQ.Core.Common;

namespace MSMQ.Client
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                if (!Menu())
                    break;
            }
        }

        private static bool Menu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Create queue");
            Console.WriteLine("2. Delete queue");
            Console.WriteLine("3. Send Message");
            Console.WriteLine("4. Receive message");
            Console.WriteLine("5. Peek message");
            Console.WriteLine("6. Peek all messages");
            Console.WriteLine("7. Delete all messages");
            Console.WriteLine("Press any other key to exit");

            char symbol = Console.ReadKey(false).KeyChar;
            OperationType type = (OperationType)Char.GetNumericValue(symbol);
            Console.Clear();

            switch (type)
            {
                case OperationType.CreateQueue:
                    CreateQueue();
                    break;
                case OperationType.DeleteQueue:
                    DeleteQueue();
                    break;
                case OperationType.SendMessage:
                    SendMessage();
                    break;
                case OperationType.ReceiveMessage:
                    ReceiveMessage();
                    break;
                case OperationType.PeekMessage:
                    PeekMessage();
                    break;
                case OperationType.PeekAll:
                    PeekAll();
                    break;
                case OperationType.DeleteAllMessages:
                    DeleteAllMessages();
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static void CreateQueue()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");

                string name = Console.ReadLine();
                MhQueue.Create(name);

                Console.WriteLine($"The {name} was created succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void DeleteQueue()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");

                string name = Console.ReadLine();
                MhQueue.Delete(name);

                Console.WriteLine($"The {name} was deleted succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void SendMessage()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");
                string name = Console.ReadLine();
                Console.WriteLine("\n\tEnter message:");
                string message = Console.ReadLine();
                Console.WriteLine("\n\tEnter label:");
                string label = Console.ReadLine();

                MhQueue queue = new MhQueue(name);
                queue.Send(message, label);

                Console.WriteLine($"The {message} was sent succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ReceiveMessage()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");
                string name = Console.ReadLine();

                MhQueue queue = new MhQueue(name);
                IMhMessage message = queue.Receive();

                Console.WriteLine($"Message: {message.Body}    SentTime: {message.SentTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void PeekMessage()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");
                string name = Console.ReadLine();

                MhQueue queue = new MhQueue(name);
                var message = queue.Peek();

                Console.WriteLine($"Message: {message.Body}    SentTime: {message.SentTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void PeekAll()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");
                string name = Console.ReadLine();

                MhQueue queue = new MhQueue(name);
                IEnumerable<IMhMessage> messages = queue.GetMessages();

                foreach (var message in messages)
                {
                    Console.WriteLine($"Message: {message.Body}    SentTime: {message.SentTime}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void DeleteAllMessages()
        {
            try
            {
                Console.WriteLine("\n\tEnter name of queue:");
                string name = Console.ReadLine();

                MhQueue queue = new MhQueue(name);
                queue.Purge();

                Console.WriteLine("Messages were deleted succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
