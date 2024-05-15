using Plugins.TyrAdsSDK.Scripts.API;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Extra;
using Plugins.TyrAdsSDK.Scripts.Caching;
using Plugins.TyrAdsSDK.Scripts.UI;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts
{
    public static class TyrAds
    {

        public static void SetProfile(string userId, int gender, int yearOfBirth)
        {
            if (SdkNull) return;
            SDK.SetProfile(userId, gender, yearOfBirth);
        }

        public static void SetGAID(string gaid)
        {
            if (SdkNull) return;
            SDK.SetGAID(gaid);
        }

        //public static void RewardPlayer(int goldAmount)
        //{
        //    // Your player get currency reward here
        //    // put your execution here
        //    // i.e. Bank.AddGold(currencyAmount);
        //    SDK.InvokeRewardEvent(goldAmount);
        //}

        public static void Show()
        {
            if (SdkNull) return;
            SDK.Show();
        }

        public static void Hide()
        {
            if (SdkNull) return;
            SDK.Hide();
        }

        #region Setup
        public static void SetIcon(Texture2D texture) => TA_Currency.SetIcon(texture);
        public static void SetAPI(TA_API api) => API = api;
        public static void SetUI(TA_UI ui) => UI = ui;
        public static void SetSDK(TA_SDK sdk) => SDK = sdk;
        #endregion

        #region Access
        public static TA_SDK SDK { get; private set; }
        public static TA_API API { get; private set; }
        public static TA_UI UI { get; private set; }
        public static bool Initialized => API.server.initialized;
        public static TA_Campaign[] TargetedCampaignsList => SDK.TargetedCampaignsList;
        public static TA_DeviceData DeviceData => TA_Android.GetDeviceData();
        static bool SdkNull
        {
            get
            {
                if (SDK) return false;
                Debug.LogError("TyrAds SDK has not been initialized." +
                               " Try to call it from Start() method, not from Awake()");
                return true;
            }
        }
        #endregion
    }
}