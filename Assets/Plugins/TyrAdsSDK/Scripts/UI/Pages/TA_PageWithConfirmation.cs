using System;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_PageWithConfirmation : TA_Page
    {
        public Button confirmButton;
        public event Action OnConfirm = delegate { };

        protected void DisableConfirm()
            => confirmButton.interactable = false;

        protected void AllowConfirm()
        {
            confirmButton.interactable = true;
            confirmButton.onClick.AddListener(Confirm);
        }

        void Confirm()
        {
            confirmButton.onClick.RemoveListener(Confirm);
            OnConfirm();
        }
    }
}