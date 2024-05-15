using Plugins.TyrAdsSDK.Scripts.API.Data;
using System;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    [AddComponentMenu("")]
    public class TA_Initialize : TA_PostService
    {
        [Space(20)]
        [SerializeField] TA_InitializePost post;
        [SerializeField] TA_InitializeResponse response;

        public TA_InitializePost PostContent => post;
        public TA_InitializeData Response => response.data;
        public TA_PublisherApp PublisherResponse => Response.publisherApp;
        public TA_User UserResponse => Response.user;

        public void SetGAID(string gaid)
        {
            post.publisherUserId = string.Empty;
            post.identifier = gaid;
            post.identifierType = "GAID";


            if (Application.platform == RuntimePlatform.Android)
            {
                post.platform = "Android";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                post.platform = "iOS";
            }
            else
            {
                post.platform = "Unknown";
            }
        }

        public void SetUser(TA_User user)
        {
            post.email = user.email;
            post.phoneNumber = user.phoneNumber;
            post.age = user.age;
            post.gender = user.gender;
        }

        protected override async void OnRequest()
        {
            try
            {
                var result = await PostContent(post);
                response = ReadData<TA_InitializeResponse>(result);
            }
            catch (Exception e)
            {
                // error event to stop refresh page
            }
        }
    }
}