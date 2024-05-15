using System;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_CreatePublisherUser : TA_PostService
    {
        [Space(20)]
        public TA_PublisherUserPost post = new();
        public TA_PublisherUserResponse response;

        public TA_PublisherUserID UserResponse => response.data;

        protected override async void OnRequest()
        {
            try
            {
                var result = await PostContent(post);
                response = ReadData<TA_PublisherUserResponse>(result);
            }
            catch (Exception e)
            {
                // error event to stop refresh page
            }
        }
    }
}