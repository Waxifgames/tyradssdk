using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_HelpButton : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public CanvasGroup group;
        public Button button;
        public event Action OnClick = delegate { };

        void Click() => OnClick();

        public void Hide()
        {
            group.alpha = 0;
            button.onClick.RemoveListener(Click);
        }

        public void Show()
        {
            group.alpha = 1;
            button.onClick.AddListener(Click);
        }

        public void ShowTitle() => title.gameObject.SetActive(true);
        public void HideTitle() => title.gameObject.SetActive(false);
    }
}