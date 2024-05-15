using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_SubmitSupportTicket : TA_PostServiceWithCampaignID
    {
        [Space(20)]
        [SerializeField] TA_SubmitTicketResponse response;
        public TA_SubmitTicketData Ticket => response.data;

        public void SetTicketID(int id)
        {
            Data.url += "/" + id;
            Debug.LogError("new " + Data.url);
        }

        protected override async void OnRequest()
        {
            try
            {
                var result = await Post();
                response = ReadData<TA_SubmitTicketResponse>(result);
            }
            catch (Exception e)
            {
                // error event to stop refresh page
            }
        }
    }
}