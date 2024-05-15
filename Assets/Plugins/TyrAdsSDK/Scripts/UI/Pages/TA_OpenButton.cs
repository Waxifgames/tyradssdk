using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_OpenButton : MonoBehaviour
    {
        public Button button;
        TA_SDK _sdk;
        void Start() => _sdk = FindObjectOfType<TA_SDK>();
        void OnEnable() => button.onClick.AddListener(ShowSdk);
        void OnDisable() => button.onClick.RemoveListener(ShowSdk);
        void ShowSdk()
        {
            _sdk.Show();
        }
    }
}