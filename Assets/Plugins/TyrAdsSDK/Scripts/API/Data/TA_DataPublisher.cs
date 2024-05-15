namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [System.Serializable]
    public class TA_PublisherUserResponse
    {
        public string message;
        public TA_PublisherUserID data;
    }

    [System.Serializable]
    public class TA_PublisherUserID
    {
        public string publisherUserId;
    }


    [System.Serializable]
    public class TA_PublisherUserPost
    {
        public string platform;
        public string identifierType;
        public string identifier;
    }
}