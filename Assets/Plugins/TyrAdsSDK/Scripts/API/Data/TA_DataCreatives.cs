using System;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_Creatives
    {
        public string creativeUrl;
        public TA_CreativePack[] creativePacks;
    }

    [Serializable]
    public class TA_CreativePack
    {
        public string creativePackName;
        public string languageName;
        public string languageCode;
        public TA_Creative[] creatives;
    }

    [Serializable]
    public class TA_Creative
    {
        public string creativeName;
        public string callToAction;
        public string text;
        public string byteSize;
        public string fileUrl;
        public string duration;
        public TA_CreativeType creativeType;
    }

    [Serializable]
    public class TA_CreativeType
    {
        public string name;
        public string type;
        public string width;
        public string height;
        public string creativeCategoryType;
    }
}