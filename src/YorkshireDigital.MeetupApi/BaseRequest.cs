namespace YorkshireDigital.MeetupApi
{
    using System.ComponentModel;
    using RestSharp;

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

                if (string.IsNullOrEmpty(value.ToString())) continue;

                var attributes = property.GetCustomAttributes(typeof(DescriptionAttribute), false);
                restRequest.AddParameter(((DescriptionAttribute)attributes[0]).Description, value.ToString());
            }

            return restRequest;
        }
    }
}
