using System;

namespace YorkshireDigital.MessageQueue.Messages
{
    public class TextMessage : IHandleMessage
    {
        public string Text { get; set; }

        public void Handle()
        {
            Console.WriteLine(Text);
        }
    }
}
