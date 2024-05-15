using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_SupportTicketSuccess : TA_Page
    {
        [SerializeField] Button backButton;
        public event Action OnBack = delegate { };
        protected override void Showing() => backButton.onClick.AddListener(Back);
        protected override void Hiding() => backButton.onClick.RemoveListener(Back);
        void Back() => OnBack();
    }
}