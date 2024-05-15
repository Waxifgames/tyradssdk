using TMPro;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_HelpCategory : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI label;

        public void SetLabel(string msg)
        {
            label.text = msg;
        }
    }
}
