using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_OfferTag : MonoBehaviour
    {
        public CanvasGroup group;
        public TextMeshProUGUI txt;
        public Image background;

        public Color easyColor;
        public Color mediumColor;
        public Color hardColor;
        public Color categoryColor;

        public void Hide()
        {
            group.alpha = 0;
        }

        public void Set(string msg)
        {
            group.alpha = NotEmpty(msg) ? 1 : 0;
            txt.text = msg;
            background.color = GetColor(msg);
        }

        bool NotEmpty(string msg)
            => msg is not null
               && !msg.Equals(string.Empty);

        Color GetColor(string msg)
            => msg switch
            {
                "Easy" => easyColor,
                "Medium" => mediumColor,
                "Hard" => hardColor,
                _ => categoryColor
            };
    }
}