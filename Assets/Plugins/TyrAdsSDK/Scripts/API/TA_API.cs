using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Extra;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API
{
    [AddComponentMenu("")]
    public class TA_API : MonoBehaviour
    {
        [TA_ReadOnly] public TA_Server server;

        [Header("Initialization")]
        public TA_Initialize initialize;
        // public TA_CreatePublisherUser createPublisherUser;
        // public TA_StoreUserDevice storeUserDevice;
        public TA_UpdateUser updateUser;

        [Header("Campaigns")]
        public TA_TargetedCampaigns targetedCampaigns;
        public TA_TargetedCampaignDetail targetedCampaignDetail;
        public TA_ActiveACampaign activeACampaign;
        public TA_ActivatedCampaigns activatedCampaigns;

        [Header("Support")]
        public TA_CampaignSupportTickets campaignSupportTickets;
        public TA_SubmitSupportTicket submitSupportTickets;

        public void RefreshData()
        {
            TA_URL.SetServer(server.address);
            TA_URL.SetVersion(server.version);

            initialize.SetData(server);

            // createPublisherUser.SetData(server);
            // storeUserDevice.SetData(server);
            updateUser.SetData(server);

            targetedCampaigns.SetData(server);
            targetedCampaignDetail.SetData(server);

            activeACampaign.SetData(server);
            activatedCampaigns.SetData(server);

            campaignSupportTickets.SetData(server);
            submitSupportTickets.SetData(server);
        }
    }
}