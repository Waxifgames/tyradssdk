using Plugins.TyrAdsSDK.Scripts;
using TMPro;
using UnityEngine;

public class TestAdvertisingID : MonoBehaviour
{
    void Start()
    {
        SetID();
        ResetPos();
    }
    public TextMeshProUGUI txt;
    void ResetPos()
    {
        var rect = (RectTransform)transform;
        rect.anchoredPosition = Vector2.zero;
    }
    void SetID()
    {
        MiniIT.Utils.AdvertisingIdFetcher.RequestAdvertisingId(
            (advertisingId, trackingEnabled, error)
                =>
            {
                FindObjectOfType<TA_SDK>().SetGAID(advertisingId);
                txt.text = "<color=white>Advertising ID:</color>\n" + advertisingId;
               // Debug.Log("advertisingId " + advertisingId + ", tracking: " + trackingEnabled + " " + error);
            });

 
    }
}
