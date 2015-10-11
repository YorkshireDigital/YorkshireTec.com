using System;

namespace YorkshireDigital.Data.Messages
{
    public class TextMessage : IHandleMessage
    {
        public TextMessage(string input)
        {
            Message = input;
        }

        public string Message { get; set; }

        public void Handle()
        {
            Console.WriteLine(Message);
        }
    }
}
