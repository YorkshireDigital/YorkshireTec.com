namespace YorkshireDigital.MeetupApi.Models
{
    public class ApiResponse<T>
    {
        public T Results { get; set; }
        public Meta Meta { get; set; }
    }
}
