namespace YorkshireDigital.Data.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RestSharp.Extensions.MonoHttp;

    public interface IDiscourseHelper
    {
        bool ValidatePayloadSignature(string payload, string signature);
        string GetHash(string payload);
        string GetRedirectUrl(string discourseDomain, string name, string externalId, string email, string username, string nonce);
        string GetNonceFromPayload(string payload64);
    }

    public class DiscourseHelper : IDiscourseHelper
    {
        private readonly string secret;

        public DiscourseHelper(string secret)
        {
            this.secret = secret;
        }

        public bool ValidatePayloadSignature(string payload, string signature)
        {
            return signature == GetHash(payload);
        }

        public string GetHash(string payload)
        {
            var encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(secret);

            var hasher = new System.Security.Cryptography.HMACSHA256(keyBytes);

            var bytes = encoding.GetBytes(payload);
            var hash = hasher.ComputeHash(bytes);

            return hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));
        }

        public string GetRedirectUrl(string discourseDomain, string name, string externalId, string email, string username, string nonce)
        {
            var payload = string.Format("nonce={0}&name={1}&username={2}&email={3}&external_id={4}", HttpUtility.UrlEncode(nonce), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(username), HttpUtility.UrlEncode(email), HttpUtility.UrlEncode(externalId));
            var payload64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            var payloadChunked = String.Join("\n", ChunksUpto(payload64, 60)) + "\n";
            var payloadUrl = HttpUtility.UrlEncode(payloadChunked);

            var hash = GetHash(payloadChunked);

            return string.Format("{0}/session/sso_login?sso={1}&sig={2}", discourseDomain, payloadUrl, hash);
        }

        public string GetNonceFromPayload(string payload64)
        {
            var payloadBytes = Convert.FromBase64String(payload64);
            var decodedPayload = Encoding.UTF8.GetString(payloadBytes);
            var payloadKvp = HttpUtility.ParseQueryString(decodedPayload);
            var nonce = payloadKvp["nonce"];
            return nonce;
        }

        private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (var i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
    }
}
