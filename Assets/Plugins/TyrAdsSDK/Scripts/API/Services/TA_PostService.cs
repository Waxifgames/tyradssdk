using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_PostService : TA_Service
    {
        protected async Task<string> Post()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, URL);
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
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        protected async Task<string> PostContent<T>(T content, bool platfromAndVersionHeader = false)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, URL);
                request.Headers.Add("X-API-Key", APIKey);
                request.Headers.Add("X-API-Secret", APISecret);
                if (platfromAndVersionHeader)
                {
                    request.Headers.Add("X-SDK-Platform", "Unity");
                    request.Headers.Add("X-SDK-Version", "v0.1.0");
                }

                var jsonContent = JsonUtility.ToJson(content);
                var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Content = requestContent;

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
            catch (HttpRequestException e)
            {
                return null;
            }
        }
    }
}