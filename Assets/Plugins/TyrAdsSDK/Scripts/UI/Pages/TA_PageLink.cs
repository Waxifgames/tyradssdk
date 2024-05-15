using System;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [System.Serializable]
    
    public class TA_PageLink
    {
        public Button openButton;
        public TA_Page page;
        public event Action<TA_Page> OnShow = delegate { };

        public void Subscribe()
        {
            openButton.onClick.AddListener(Show);
        }

        public void UnSubscribe()
        {
            openButton.onClick.RemoveListener(Show);
        }

        void Show()
        {
            OnShow(page);
        }
    }
}