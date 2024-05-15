using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_GetServiceWithCampaignID : TA_GetService
    {
        [TA_ReadOnly] public string campaignID;

        public void SetCampaignID(int id)
        {
            campaignID = id.ToString();
            Data.url = GetURL() + AddCampaignToURL(id);
        }
    }
}