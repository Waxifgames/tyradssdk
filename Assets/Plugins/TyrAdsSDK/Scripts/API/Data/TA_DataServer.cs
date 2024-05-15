using System;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public struct TA_Server
    {
        public string address;
        public string version;
        public string apiKey;
        public string apiSecret;
        public string publisherUserID;
        public string url;
        public bool initialized;
    }

    [Serializable]
    public enum TA_Address
    {
        Initialize,
        CreatePublisherUser,
        UpdateUser,
        TargetedCampaigns,
        TargetedCampaignDetail,
        ActiveACampaign,
        ActivatedCampaigns,
        CampaignSupportTickets,
        SubmitSupportTickets
    }
}