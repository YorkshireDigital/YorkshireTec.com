namespace YorkshireDigital.MeetupApi.Requests
{
    using System.ComponentModel;
    using RestSharp;
    using YorkshireDigital.MeetupApi.Requests.Enum;

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
                var value = property.GetValue(this, null);

                if (value == null || string.IsNullOrEmpty(value.ToString())) continue;

                if (value is int && (int)value == 0) continue;
                if (value is System.Enum && (int)value == 0) continue;

                var attributes = property.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (value is EventStatus)
                {
                    restRequest.AddParameter(((DescriptionAttribute)attributes[0]).Description, value.ToString().ToLower());
                }
                else
                {
                    restRequest.AddParameter(((DescriptionAttribute)attributes[0]).Description, value.ToString());
                }
            }

            return restRequest;
        }
    }
}
