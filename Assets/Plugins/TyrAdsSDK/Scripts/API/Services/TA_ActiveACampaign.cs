using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_ActiveACampaign : TA_PostServiceWithCampaignID
    {
        [Space(20)]
        [SerializeField] TA_ActiveACampaignResponse response;

        protected override async void OnRequest()
        {
            try
            {
                var result = await Post();
                response = ReadData<TA_ActiveACampaignResponse>(result);
            }
            catch (Exception e)
            {
                // error event to stop refresh page
            }
        }
    }
}