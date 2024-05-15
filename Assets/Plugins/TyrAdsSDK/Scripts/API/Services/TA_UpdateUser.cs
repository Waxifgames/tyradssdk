using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_UpdateUser : TA_PutService
    {
        [Space(20)]
        [SerializeField] TA_UpdateUserResponse response;
        public TA_User UserResponse => response.data;

        protected override async void OnRequest()
        {
            var result = await Put();
            response = ReadData<TA_UpdateUserResponse>(result);
        }
    }
}