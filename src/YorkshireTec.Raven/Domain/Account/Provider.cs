namespace YorkshireTec.Raven.Domain.Account
{
    using System;

    public class Provider
    {
        public string Name { get; set; }
        public string PublicToken { get; set; }
        public string SecretToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Username { get; set; }
    }
}
