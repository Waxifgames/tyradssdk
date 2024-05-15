using System.Collections.Generic;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.UI.Pages;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_ScrollOffers : MonoBehaviour
    {
        [SerializeField] TA_Offers page;
        [SerializeField] TA_ScrollVertical scroll;
        [SerializeField] [TA_ReadOnly] TA_Campaign[] _data;

        [TA_ReadOnly] public List<TA_OfferItem> list = new();
        [TA_ReadOnly] public List<TA_Campaign> showList = new();
        void ShowDetails(TA_OfferItem item) => page.ShowDetails(item);

        public void Init()
        {
            scroll.OnScrollUp += RefreshBottom;
            scroll.OnScrollDown += RefreshFirst;
            scroll.FindItems();
        }

        public void HideAll()
        {
            foreach (var offer in scroll.items)
                offer.Hide();
        }

        public void Refresh(TA_Campaign[] data)
        {
           // page.Downloader.StopAllDownloads();

            foreach (var offer in scroll.items)
                offer.Hide();

            list.Clear();
            showList.Clear();
            scroll.Init(data.Length);
            _data = data;

            for (var i = 0; i < scroll.items.Count; i++)
            {
                var offer = scroll.items[i];
                if (i >= data.Length)
                {
                    offer.Hide();
                    continue;
                }

                var campaign = data[i];
                if (campaign.status.Equals("paused")) continue;

                RefreshOffer(offer, campaign, i);

                offer.OnPlay -= ShowDetails;
                offer.OnPlay += ShowDetails;

                list.Add(offer);
                showList.Add(campaign);
            }
        }

        void RefreshOffer(TA_OfferItem offer, TA_Campaign campaign, int id)
        {
            offer.ID = id;

            offer.SetIcon(null);
            offer.SetPreview(null);
            offer.SetName(campaign.app.title);
            offer.SetPoints(campaign.campaignPayout.totalPayoutConverted, campaign.currency.adUnitCurrencyName);
            offer.SetPackage(campaign.app.packageName);
            offer.SetCategory(campaign.app.storeCategory);
            offer.SetDifficulty(campaign.targeting.reward.rewardDifficulty);
            offer.SetCampaignID(campaign.campaignId);
            offer.SetTotalRewards((int)campaign.campaignPayout.totalEvents);
            offer.Show();

            page.Downloader.DownloadIcon(campaign, offer);
            page.Downloader.DownloadPreview(campaign, offer);
        }

        void RefreshBottom(TA_OfferItem offer)
        {
            var previous = scroll.items[^2].GetComponent<TA_OfferItem>();
            var nextID = previous.ID + 1;
            var data = _data[nextID];
            // Debug.Log("SwapToBot  ID: " + nextID + ", Campaign: " + data.campaignId, offer);
            RefreshOffer(offer, data, nextID);
        }

        void RefreshFirst(TA_OfferItem offer)
        {
            var nextAge = scroll.items[1].GetComponent<TA_OfferItem>();
            var nextID = nextAge.ID - 1;
            var data = _data[nextID];
            // Debug.Log("SwapToTop  ID: " + nextID + ", Campaign: " + data.campaignId, offer);
            RefreshOffer(offer, data, nextID);
        }
    }
}