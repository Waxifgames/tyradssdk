using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_Access : TA_PageWithConfirmation
    {
        public TA_SwitchButton switcher;

        protected override void Showing()
        {
            DisableConfirm();
            switcher.OnSwitchOn += RequestDeviceAccess;
            switcher.OnSwitchOff += CancelAccess;
        }

        protected override void Hiding()
        {
            switcher.OnSwitchOn -= RequestDeviceAccess;
            switcher.OnSwitchOff -= CancelAccess;
        }

        void CancelAccess()
        {
            DisableConfirm();
        }

        void RequestDeviceAccess()
        {
            AccessGranted();
        }

        void AccessGranted()
        {
            AllowConfirm();
        }
    }
}