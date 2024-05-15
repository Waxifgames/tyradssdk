using System;
using System.Collections.Generic;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_InitializePost
    {
        public string platform;
        public string identifierType;
        public string identifier;
        public string publisherUserId;
        public string email;
        public string phoneNumber;
        public int age;
        public int gender;
        public TA_DeviceData deviceData;
    }

    [Serializable]
    public class TA_InitializeResponse
    {
        public string message;
        public TA_InitializeData data;
    }

    [Serializable]
    public class TA_InitializeData
    {
        public bool newDevice;
        public bool newRegisteredUser;
        public TA_PublisherApp publisherApp;
        public TA_User user;
    }

    [Serializable]
    public class TA_UpdateUserResponse
    {
        public string message;
        public TA_User data;
    }

    [Serializable]
    public class TA_User
    {
        public int id;
        public string publisherUserId;
        public int publisherAppId;
        public int adUnitId;
        public string email;
        public string phoneNumber;
        public string platform;
        public string country;
        public int age;
        public int gender;
        public bool highRisk;
        public string createdAt;
        public string modifiedAt;
    }

    [Serializable]
    public class TA_PublisherApp
    {
        public int publisherId;
        public string platform;
        public string packageName;
        public string baseUrl;
        public string title;
        public string summary;
        public string thumbnail;
        public int rating;
        public object downloadsMin;
        public object downloadsMax;
        public string website;
        public object size;
        public object containsAds;
        public object developer;
        public object userRatingsCount;
        public object iapMin;
        public object iapMax;
        public object privacyPolicyUrl;
        public object contentRating;
        public object releaseDate;
        public object r1;
        public object r2;
        public object r3;
        public object r4;
        public object r5;
        public object catKeys;
        public object description;
        public object similar;
        public object screenshots;
        public object promoVideo;
        public object availableIn;
        public int appStoreCategoryId;
        public List<TA_AdUnit> adUnits;
        public TA_AppStoreCategory appStoreCategory;
    }

    [Serializable]
    public class TA_AdUnit
    {
        public int id;
        public int publisherId;
        public int publisherAppId;
        public string indentifier;
        public string name;
        public int adUnitTypeId;
        public string currencyIcon;
        public string currencyName;
        public int currencyConversion;
        public TA_AdUnitType adUnitType;
    }

    [Serializable]
    public class TA_AdUnitType
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class TA_AppStoreCategory
    {
        public int id;
        public string name;
        public int appCategoryId;
        public string store;
    }
}