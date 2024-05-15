using System.Collections;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.Caching;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Extra
{
    public static class TA_Web
    {
        public static IEnumerator Offers(TA_GetCampaignListService service, bool isActive)
        {
            while (!TyrAds.Initialized)
                yield return null;

            yield return Request(service);

            if (isActive)
                TyrAds.UI.SetTotalActiveOffers(service.Campaigns.Length);
            else
            {
                /*
                if (UseTest)
                {
                    var a = TargetedCampaignsList[0];

                    _real = TargetedCampaignsList;
                    var realAmount = TargetedCampaignsList.Length;
                    _total = _real.Length + _testCampaigns;
                    _newTest = new TA_Campaign[_total];

                    yield return null;

                    for (var i = 0; i < _total; i++)
                    {
                        if (i < realAmount)
                            _newTest[i] = _real[i];
                        else
                            _newTest[i] = a;
                    }
                }
                */

                if (TyrAds.TargetedCampaignsList.Length > 0)
                    yield return TyrAds.SDK.Downloader.DownloadTPointsIcon(TyrAds.TargetedCampaignsList[0].currency.adUnitCurrencyIcon);
            }

            // _ui.RefreshOffers(UseTest && !isActive ? _newTest : service.Campaigns, isActive);
            TyrAds.UI.RefreshOffers(service.Campaigns, isActive);
        }

        public static IEnumerator Details(TA_TargetedCampaignDetail service)
        {
            yield return Request(service);
        }

        public static IEnumerator Request(TA_Service service)
        {
            service.Request();
            yield return TrackService(service);
        }

        public static IEnumerator Post(TA_Service service)
        {
            service.Request();
            yield return TrackService(service);
        }


        static IEnumerator TrackService(TA_Service service)
        {
            while (service.WaitingResponse)
                yield return null;

            if (service.Error) yield break;
            //TODO: WHAT IF SERVER ERROR???

            while (service.ReadingData)
                yield return null;
        }
        public static IEnumerator WaitForInternet()
        {
            TyrAds.UI.ShowInternetMissing();

            while (!IsInternetConnection)
                yield return new WaitForSeconds(0.5f);

            TyrAds.UI.HideInternetMissing();
            TyrAds.Show();
        }

        public static bool IsInternetConnection =>
         Application.internetReachability switch
         {
             NetworkReachability.NotReachable => false,
             NetworkReachability.ReachableViaCarrierDataNetwork => true,
             NetworkReachability.ReachableViaLocalAreaNetwork => true,
             _ => false
         };


    }
}