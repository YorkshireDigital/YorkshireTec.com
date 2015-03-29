namespace YorkshireDigital.Data.Domain.Account
{
    using YorkshireDigital.Data.Domain.Account.Enums;

    public class UserRole
    {
        public virtual int Id { get; set; }
        public virtual UserRoles Role { get; set; }
        public virtual string Claims { get; set; }
    }
}
