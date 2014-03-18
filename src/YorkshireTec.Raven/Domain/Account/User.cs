namespace YorkshireTec.Raven.Domain.Account
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Locale { get; set; }
        public string Picture { get; set; }
        public IList<Provider> Providers { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
