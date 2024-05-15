using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_TargetedCampaignDetail : TA_GetServiceWithCampaignID
    {
        [Space(20)]
        [SerializeField] TA_CampaignDetailResponse response;
        public TA_DetailCampaign Details => response.data;

        protected override async void OnRequest()
        {
            try
            {
                var result = await Get();
                response = ReadData<TA_CampaignDetailResponse>(result);
                if (response is { data: { payoutEvents: not null } })
                    response.data.payoutEvents = SortByOrder(response.data.payoutEvents);
            }
            catch (Exception ex)
            {
                // error event to stop refresh page
            }
        }

        static List<TA_PayoutEvent> SortByOrder(List<TA_PayoutEvent> orders)
            => orders.OrderBy(payout => payout.eventOrder).ToList();
    }
}