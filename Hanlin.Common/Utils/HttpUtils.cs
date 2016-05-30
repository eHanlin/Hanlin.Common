using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
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

        private static T BuildJsonQuery<T>(string url, object request, Func<HttpClient, StringContent, HttpResponseMessage> exec)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var client = BuildJsonClient();
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = exec(client, httpContent);

            response.EnsureSuccessStatusCode();

            var respJson = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static T GetJson<T>(string url)
        {
            var client = BuildJsonClient();
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var respJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static T PostJson<T>(string url, object request)
        {
            return BuildJsonQuery<T>(url, request, (client, httpContent) => client.PostAsync(url, httpContent).Result);
        }

        public static T PutJson<T>(string url, object request)
        {
            return BuildJsonQuery<T>(url, request, (client, httpContent) => client.PutAsync(url, httpContent).Result);
        }
    }
}