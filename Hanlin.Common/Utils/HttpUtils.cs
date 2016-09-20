using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Hanlin.Common.Utils
{
    public class HttpUtils
    {
        private static HttpClient BuildJsonClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static string BuildJsonQuery(object request, Func<HttpClient, StringContent, HttpResponseMessage> exec)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var client = BuildJsonClient();
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = exec(client, httpContent);

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        public static string Get(string url)
        {
            var client = BuildJsonClient();
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public static T GetJson<T>(string url)
        {
            return JsonConvert.DeserializeObject<T>(Get(url));
        }

        public static string PostJson(string url, object request)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PostAsync(url, httpContent).Result);
        }

        public static T PostJson<T>(string url, object request)
        {
            var respJson = PostJson(url, request);
            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static string PutJson(string url, object request)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PutAsync(url, httpContent).Result);
        }

        public static T PutJson<T>(string url, object request)
        {
            var respJson = PutJson(url, request);
            return JsonConvert.DeserializeObject<T>(respJson);
        }
    }
}