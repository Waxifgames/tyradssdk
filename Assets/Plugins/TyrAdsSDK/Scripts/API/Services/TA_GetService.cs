using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_GetService : TA_Service
    {
        protected async Task<string> Get()
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, URL);
            request.Headers.Add("X-API-Key", APIKey);
            request.Headers.Add("X-API-Secret", APISecret);
            request.Headers.Add("X-User-ID", UserID);

            Sending(request, URL);

            var http = await client.SendAsync(request);
            if (!http.IsSuccessStatusCode)
            {
                switch (http.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        Debug.Log("Error Message: " + await http.Content.ReadAsStringAsync());
                        break;
                    default:
                        TA_Debug.LogError(this, URL +
                                                "\n<color=white>" +
                                                "HTTP Error Status: " + http.StatusCode +
                                                "</color>");
                        break;
                }
            }

            http.EnsureSuccessStatusCode();

            var httpContent = await http.Content.ReadAsStringAsync();
            Received(http, httpContent);
            return httpContent;
        }
    }
}