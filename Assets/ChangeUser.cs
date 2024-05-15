using Plugins.TyrAdsSDK.Scripts;
using TMPro;
using UnityEngine;

public class ChangeUser : MonoBehaviour
{
    public string testUser = "bb517172-d264-444c-ab6e-6c83e96b9019";
    public string abcdUser = "abc-def-ghi";
    public TextMeshProUGUI txt;
    TA_SDK sdk;

    bool isTest;

    public void Change()
    {
        isTest = !isTest;
        if (isTest) TestUser();
        else NewUser();
    }

    void Start()
    {
        isTest = true;
        sdk = FindObjectOfType<TA_SDK>();
        TestUser();
    }

    void TestUser()
    {
        txt.text = "bb517172 User";
        // sdk.SetTestUser(testUser);
        // sdk.Clear();
    }

    void NewUser()
    {
        txt.text = "abc-def-ghi User";
        // sdk.GetUserFromAPI(abcdUser);
        // sdk.Clear();
    }
}