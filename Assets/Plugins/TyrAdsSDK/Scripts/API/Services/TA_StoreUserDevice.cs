using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_StoreUserDevice : TA_PostService
    {
        [Space(20)]
        public TA_DeviceData post = new();
        public TA_StoreUserDeviceResponse response;

        public TA_StoreUserDeviceData ResponseData => response.data;

        protected override async void OnRequest()
        {
            try
            {
                var result = await PostContent(post);
                response = ReadData<TA_StoreUserDeviceResponse>(result);
            }
            catch (Exception e)
            {
                // error event to stop refresh page
            }
        }
    }
}