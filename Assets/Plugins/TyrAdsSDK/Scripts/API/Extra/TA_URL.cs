using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Extra
{
    public static class TA_URL
    {
        static string _server;
        static string _version;
        public static void SetServer(string server) => _server = server;
        public static void SetVersion(string version) => _version = version;

        public static string GetURL(TA_Address type)
        {
            var address = type switch
            {
                TA_Address.Initialize => "initialize",
                TA_Address.CreatePublisherUser => "publisher-user",
                TA_Address.UpdateUser => "update-user",
                TA_Address.TargetedCampaigns => "campaigns",
                TA_Address.TargetedCampaignDetail => "campaigns",
                TA_Address.ActiveACampaign => "campaigns/active",
                TA_Address.ActivatedCampaigns => "activated-campaigns",
                TA_Address.CampaignSupportTickets => "campaigns",
                TA_Address.SubmitSupportTickets => "campaigns",
                _ => string.Empty
            };
            if (address.Equals(string.Empty)) Debug.LogWarning("TyrRewards: Missing address!");
            return "https://" + _server + "/" + _version + "/" + address;
        }

        public static string AddCampaignToURL(TA_Address type, int campaignID) => type switch
        {
            TA_Address.ActiveACampaign => "/" + campaignID,
            TA_Address.TargetedCampaignDetail => "/" + campaignID, // + "/detail",
            TA_Address.CampaignSupportTickets => "/" + campaignID + "/tickets",
            TA_Address.SubmitSupportTickets => "/" + campaignID + "/tickets",
            _ => string.Empty
        };
    }
}