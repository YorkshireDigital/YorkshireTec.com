namespace YorkshireDigital.MeetupApi
{
    using RestSharp;
    using YorkshireDigital.MeetupApi.Helpers;

    public class BaseRequest
    {
        private readonly string endpoint;
        private readonly Method method;

        public BaseRequest(string endpoint, Method method)
        {
            this.endpoint = endpoint;
            this.method = method;
        }

        public RestRequest ToRestRequest()
        {
            var type = GetType();
            var properties = type.GetProperties();

            var restRequest = new RestRequest(endpoint, method);

            foreach (var property in properties)
            {
                if (!string.IsNullOrEmpty(property.GetValue(this, null).ToString()))
                {
                    restRequest.AddParameter(this.GetDescriptionValue(property.Name), property.GetValue(this, null).ToString());
                }
            }

            return restRequest;
        }
    }
}
