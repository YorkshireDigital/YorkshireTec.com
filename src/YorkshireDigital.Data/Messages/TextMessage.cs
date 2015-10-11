using System;

namespace YorkshireDigital.Data.Messages
{
    public class TextMessage : IHandleMessage
    {
        private string input;

        public TextMessage(string input)
        {
            this.input = input;
        }

        public string Message { get; set; }

        public void Handle()
        {
            Console.WriteLine(Message);
        }
    }
}
