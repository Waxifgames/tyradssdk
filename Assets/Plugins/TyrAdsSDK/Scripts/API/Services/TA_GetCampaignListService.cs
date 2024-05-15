using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_GetCampaignListService : TA_GetService
    {
        [Space(20)]
        [SerializeField] TA_CampaignsListResponse response;
        public TA_Campaign[] Campaigns => response.data;

        protected override async void OnRequest()
        {
            try
            {
                var result = await Get();
                response = ReadData<TA_CampaignsListResponse>(result);
            }
            catch (Exception ex)
            {
                // error event to stop refresh page
            }

        }
    }
}