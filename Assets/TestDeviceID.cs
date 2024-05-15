using TMPro;
using UnityEngine;

public class TestDeviceID : MonoBehaviour
{
    public TextMeshProUGUI txt;
    void Start()
    {
        ResetPos();
        SetID();
    }

    void ResetPos()
    {
        var rect = (RectTransform)transform;
        rect.anchoredPosition = Vector2.zero;
    }

    void SetID()
    {
        txt.text = SystemInfo.deviceUniqueIdentifier;
    }
    
}
