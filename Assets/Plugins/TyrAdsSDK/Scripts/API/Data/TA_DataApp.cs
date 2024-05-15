using System;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_App
    {
        public int id;
        public string title;
        public string packageName;
        public double rating;
        public string shortDescription;
        public string store;
        public string storeCategory;
        public string previewUrl;
        public string thumbnail;
 
        
    }
}