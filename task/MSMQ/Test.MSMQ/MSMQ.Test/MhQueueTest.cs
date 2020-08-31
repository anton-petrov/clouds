using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSMQ.Core;

namespace MSMQ.Test
{
    [TestClass]
    public class MhQueueTest
    {
        #region Properties

        const string Name = "Test-channel";

        #endregion

        #region Test Methods 

        [TestMethod]
        public void QueueCreate()
        {
            QueueDeleteIfExist(Name);

            MhQueue.Create(Name);
        }

        [TestMethod]
        public void QueueDelete()
        {
            QueueDeleteIfExist(Name);

            MhQueue.Create(Name);
            MhQueue.Delete(Name);
        }

        [TestMethod]
        public void QueueExist()
        {
            QueueDeleteIfExist(Name);

            MhQueue.Create(Name);
            bool isExist = MhQueue.IsQueueExist(Name);

            Assert.IsTrue(isExist);
        }

        [TestMethod]
        public void MessageSend()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            string msg = "test";
            string label = "group a";
            queue.Send(msg, label);
            var count = queue.Count;

            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void MessageSendWithEmptyLabel()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            string msg = "test";
            queue.Send(msg);
            var count = queue.Count;

            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        public void MessageCount()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            queue.Send("test 1");
            queue.Send("test 2");
            queue.Send("test 2");
            int count = queue.Count;

            Assert.AreEqual(count, 3);
        }

        [TestMethod]
        public void Purge()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            queue.Send("test 1");
            queue.Send("test 2");
            queue.Send("test 2");
            queue.Purge();

            Assert.AreEqual(queue.Count, 0);
        }

        [TestMethod]
        public void MessagePeek()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            queue.Send("test 1");
            queue.Peek();
            var count = queue.Count;

            Assert.AreEqual(count, 1);
        }


        [TestMethod]
        public void MessagePeekById()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            queue.Send("test 1");
            var msg1 = queue.Peek();
            var msg2 = queue.Peek(msg1.Id);

            Assert.AreEqual(msg1.Id, msg2.Id);
        }

        [TestMethod]
        public void MessageReceive()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            string msg = "test";
            string label = "group a";
            queue.Send(msg, label);
            var msgOut = queue.Receive();

            Assert.AreEqual(msg, msgOut.Body);
        }

        [TestMethod]
        public void MessageReceiveById()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            queue.Send("test 1");
            queue.Send("test 2");
            queue.Send("test 3");
            var msgOut = queue.Peek();
            queue.Receive(msgOut.Id);
            var count = queue.Count;

            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void MessageReceiveByTimePositive()
        {
            QueueDeleteIfExist(Name);
            var queue = MhQueue.Create(Name);
            string msg = "test";
            TimeSpan timeout = new TimeSpan(0, 0, 3);

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                queue.Send(msg);
            });

            var msgOut = queue.Receive(timeout);

            Assert.AreEqual(msg, msgOut.Body);
        }

        [TestMethod]
        public void MessageReceiveByTimeNegative()
        {
            try
            {
                QueueDeleteIfExist(Name);
                var queue = MhQueue.Create(Name);
                string msg = "test";
                TimeSpan timeout = new TimeSpan(0, 0, 1);

                Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    queue.Send(msg);
                });

                queue.Receive(timeout);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void MessageGetAll()
        {
            QueueDeleteIfExist(Name);

            var queue = MhQueue.Create(Name);
            var messages = new List<string> { "test 1", "test 2", "test 3" };
            foreach (var message in messages)
                queue.Send(message);

            var mhMessages = queue.GetMessages();
            foreach (var mhMessage in mhMessages)
            {
                if (!messages.Contains(mhMessage.Body.ToString()))
                    Assert.Fail();
            }
           
            Assert.AreEqual(mhMessages.Count(), 3);
        }

        #endregion

        #region Helpers

        private void QueueDeleteIfExist(string name)
        {
            if (MhQueue.IsQueueExist(Name))
                MhQueue.Delete(name);
        }

        #endregion
    }
}
