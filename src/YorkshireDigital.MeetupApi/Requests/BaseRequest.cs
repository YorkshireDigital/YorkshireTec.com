namespace YorkshireDigital.MeetupApi.Requests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using RestSharp;

    public class BaseRequest
    {
        private readonly string endpoint;

        public BaseRequest(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public RestRequest ToRestRequest(Method method, string apiKey)
        {
            var requestProperties = new Dictionary<string, string> {{"key", apiKey}};

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

                switch (value.GetType().ToString())
                {
                    case "YorkshireDigital.MeetupApi.Requests.Enum.EventStatus":
                        requestProperties.Add(((DescriptionAttribute)attributes[0]).Description, value.ToString().ToLower());
                        break;
                    case "System.Collections.Generic.Dictionary`2[System.Int32,System.String]":
                        var dictionary = (Dictionary<int, string>)value;
                        var description = ((DescriptionAttribute) attributes[0]).Description;

                        foreach (var entry in dictionary)
                        {
                            requestProperties.Add(string.Format(description, entry.Key), entry.Value);
                        }

                        break;
                    default:
                        requestProperties.Add(((DescriptionAttribute)attributes[0]).Description, value.ToString());
                        break;
                }

            }

            switch (method)
            {
                case Method.POST:
                    restRequest.AddJsonBody(requestProperties);
                    break;
                case Method.GET:
                default:
                    foreach (var requestProperty in requestProperties)
                    {
                        restRequest.AddParameter(requestProperty.Key, requestProperty.Value);
                    }
                    break;
            }

            return restRequest;
        }
    }
}
