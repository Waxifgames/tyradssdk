using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_CampaignSupportTickets : TA_GetServiceWithCampaignID
    {
        [Space(20)]
        [SerializeField] TA_SupportTicketsResponse response;
        public TA_SupportTicketCategory[] Categories => response.data;

        protected override async void OnRequest()
        {
            try
            {
                var result = await Get();
                response = ReadData<TA_SupportTicketsResponse>(result);
            }
            catch (Exception ex)
            {
                // error event to stop refresh page
            }

        }
    }
}