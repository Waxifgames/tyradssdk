using System.Net.Http;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Extra;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_Service : MonoBehaviour
    {
        [SerializeField] [TA_ReadOnly] protected TA_Server Data;
        [SerializeField] [TA_ReadOnly] protected TA_Address type;
        [SerializeField] [TextArea] protected string debug;

        public bool WaitingResponse { get; protected set; }
        public bool Success { get; protected set; }
        public bool Error => !Success;
        public bool DataHasBeenRead { get; protected set; }
        public bool ReadingData => !DataHasBeenRead;

        protected string APIKey => Data.apiKey;
        protected string APISecret => Data.apiSecret;
        protected string UserID => Data.publisherUserID;
        protected string URL => Data.url;

        protected T ReadData<T>(string msg)
        {
            var result = JsonUtility.FromJson<T>(msg);
            DataHasBeenRead = true;
            return result;
        }
        
        public void Request()
        {
            DataHasBeenRead = false;
            WaitingResponse = true;
            OnRequest();
        }

        protected void Received(HttpResponseMessage res, string msg)
        {
            Success = res.IsSuccessStatusCode;
            WaitingResponse = false;

            res.LogResult(this, msg);
        }

        protected void Sending(HttpRequestMessage request, string url)
        {
            request.LogBegin(name, url);
        }

        protected virtual async void OnRequest()
        {
            Debug.LogError("NICE");
        }

        public void SetData(TA_Server data)
        {
            Data = data;
            Data.url = GetURL();
        }

        protected string GetURL() => TA_URL.GetURL(type);
        protected string AddCampaignToURL(int campaignID) => TA_URL.AddCampaignToURL(type, campaignID);
        public void SetLog(string msg) => debug = msg;
    }
}