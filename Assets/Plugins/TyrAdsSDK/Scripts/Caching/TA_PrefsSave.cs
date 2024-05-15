using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.Caching
{
    [AddComponentMenu("")]
    public class TA_PrefsSave : MonoBehaviour
    {
        const string ACCESS = "TyrRewards_AccessGranted";
        const string USER_CONFIRMED = "TyrRewards_UserConfirmed";
        const string INITIALIZATION = "TyrRewards_Initialization";
        const string USER = "TyrRewards_User";
        const string AD_ID = "TyrRewards_AD_ID";
        const string OTHER_IDENTIFIER = "TyrRewards_OtherIdentifier";

        public string Identifier => PlayerPrefs.GetString(OTHER_IDENTIFIER);
        public bool AccessRequired => !HasAccess;
        public bool UserDataRequired => !HasUserInfo;

        public bool HasAccess => HasKey(ACCESS);
        public bool HasUserInfo => HasKey(USER_CONFIRMED);
        public bool HasBeenInitialized => HasKey(INITIALIZATION);
        public bool HasIdentifier => HasKey(OTHER_IDENTIFIER);



        bool HasKey(string key) => PlayerPrefs.HasKey(key);
        void SaveKey(string key) => PlayerPrefs.SetInt(key, 1);

        public void AccessGranted() => SaveKey(ACCESS);
        public void SaveGAID(string id) => PlayerPrefs.SetString(AD_ID, id);
        public void SaveOtherIdentifier(string id) => PlayerPrefs.SetString(OTHER_IDENTIFIER, id);

        TA_User _user;
        public TA_User User => _user;

        public void SaveUser(TA_User userData)
        {
            if (userData == null) return;
            _user.gender = userData.gender;
            _user.age = userData.age;
            _user.publisherUserId = userData.publisherUserId;
            PlayerPrefs.SetString(JsonUtility.ToJson(_user), USER);
        }

        public void SavePublisherUserID(string id)
        {
            _user.publisherUserId = id;
            SaveUser(_user);
        }

        public void UserConfirmed()
        {
            SaveKey(USER_CONFIRMED);
        }

        public void InitializationSuccess()
        {
            SaveKey(INITIALIZATION);
        }
    }
}