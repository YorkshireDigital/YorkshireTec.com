namespace YorkshireDigital.Data.Exceptions
{
    using System;

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
            
        }

        public UserNotFoundException(string message) : base(message)
        {
            
        }
    }
}
