using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hanlin.Common.Utils
{
    public class HttpSetting
    {
        public TimeSpan Timeout { set; get; } = TimeSpan.FromMinutes(2);
    }

    public class HttpUtils
    {
        private static HttpClient BuildJsonClient(HttpSetting setting = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (setting != null)
            {
                client.Timeout = setting.Timeout;
            }
            return client;
        }

        private static string BuildJsonQuery(object request, Func<HttpClient, StringContent, HttpResponseMessage> exec, bool defaultCase = false, HttpSetting setting = null)
        {
            var requestJson = defaultCase ? JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            }): JsonConvert.SerializeObject(request);

            var client = BuildJsonClient();
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = exec(client, httpContent);

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        public static string Get(string url, HttpSetting setting = null)
        {
            var client = BuildJsonClient(setting);
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public static T GetJson<T>(string url, HttpSetting setting = null)
        {
            return JsonConvert.DeserializeObject<T>(Get(url, setting));
        }

        public static string PostJson(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PostAsync(url, httpContent).Result, defaultCase, setting);
        }

        public static T PostJson<T>(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            var respJson = PostJson(url, request, defaultCase, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }
        
        public static string PutJson(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            return BuildJsonQuery(request, (client, httpContent) => client.PutAsync(url, httpContent).Result, defaultCase, setting);
        }

        public static T PutJson<T>(string url, object request, bool defaultCase = false, HttpSetting setting = null)
        {
            var respJson = PutJson(url, request, defaultCase, setting);
            return JsonConvert.DeserializeObject<T>(respJson);
        }
        
    }
}