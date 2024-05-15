using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Elements
{
    [AddComponentMenu("")]
    public class TA_GenderButton : MonoBehaviour
    {
        public Color activeColor;
        public Color passiveColor;
        public Image icon;
        public Image background;
        public Button button;
        public TextMeshProUGUI txt;
        public event Action<TA_GenderButton> OnClick = delegate { };

        void OnEnable()
        {
            button.onClick.AddListener(Click);
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(Click);
        }

        void Click() => OnClick(this);

        public void Active()
        {
            background.color = activeColor;
            icon.color = Color.white;
            txt.color = Color.white;
        }

        public void Passive()
        {
            background.color = passiveColor;
            icon.color = Color.gray;
            txt.color = Color.gray;
        }
    }
}