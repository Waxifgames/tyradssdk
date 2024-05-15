using System;
using System.Collections.Generic;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_ActiveACampaignResponse
    {
        public string message;
        public TA_ActiveACampaignData data;
    }

    [Serializable]
    public class TA_ActiveACampaignData
    {
        public bool isCampaignActivated;
    }

    [Serializable]
    public class TA_CampaignsListResponse
    {
        public string message;
        public TA_Campaign[] data;
    }

 

    [Serializable]
    public class TA_Campaign
    {
        public int campaignId;
        public string campaignName;
        public string campaignDescription;
        public string status;
        public TA_App app;
        public TA_AdCurrency currency;
        public TA_Payout campaignPayout;
        public TA_Track tracking;
        public TA_Targeting targeting;
        public TA_Creatives creative;
    }

    [Serializable]
    public class TA_Payout
    {
        public float totalEvents;
        public float totalPayout;
        public float totalPayoutConverted;
    }

    [Serializable]
    public class TA_CampaignDetailResponse
    {
        public string message;
        public TA_DetailCampaign data;
    }

    [Serializable]
    public class TA_DetailCampaign : TA_Campaign
    {
        public bool isInstalled;
        public List<TA_PayoutEvent> payoutEvents;
    }

    [Serializable]
    public class TA_Track
    {
        public string clickUrl;
        public string impressionUrl;
    }
}