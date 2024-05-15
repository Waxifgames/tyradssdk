using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_PutService : TA_Service
    {
        protected async Task<string> Put()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, URL);
                request.Headers.Add("X-API-Key", APIKey);
                request.Headers.Add("X-API-Secret", APISecret);
                request.Headers.Add("X-User-ID", UserID);

                var webResponse = await client.SendAsync(request);
                webResponse.EnsureSuccessStatusCode();
       

                var responseContent = await webResponse.Content.ReadAsStringAsync();
                Received(webResponse, responseContent);
                return responseContent;
                //TODO: what if not SUCCESS? if(Success) ReadData();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"HTTP request error: {e.Message}");
                return null;
            }
        }

    }
}