using Plugins.TyrAdsSDK.Scripts.Caching;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_TPoints : MonoBehaviour
    {
        public CanvasGroup group;
        public TextMeshProUGUI txt;
        public RawImage icon;

        public void SetPoints(double value, string type)
        {
            icon.texture = TA_Currency.Icon;
            var data = TA_Currency.ConvertTPoints(value);
            txt.text = "<b>" + data.amount.ToString(data.format)
                             + data.suffix + "</b>"
                             + " " + type;
        }

        public void Hide()
        {
            if (!group) return;
            group.alpha = 0;
        }

        public void Show()
        {
            if (!group) return;
            group.alpha = 1;
        }
    }
}