namespace YorkshireDigital.MeetupApi.Models
{
    public class ApiResponse<T>
    {
        public T Results { get; set; }
        public Meta Meta { get; set; }

        public int RateLimit { get; set; }
        public int RateLimitRemaining { get; set; }
        public int RateLimitReset { get; set; }

    }
}
