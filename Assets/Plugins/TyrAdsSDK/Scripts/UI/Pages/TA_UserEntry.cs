using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_UserEntry : TA_PageWithConfirmation
    {
        public TA_GenderButton male;
        public TA_GenderButton female;
        public TA_ScrollAge agePicker;
        public int Gender { get; private set; }
        public int Age => agePicker.chosenAge;

        protected override void Showing()
        {
            DisableConfirm();
            male.OnClick += ChangeGender;
            female.OnClick += ChangeGender;
        }

        protected override void Hiding()
        {
            male.OnClick -= ChangeGender;
            female.OnClick -= ChangeGender;
        }

        void ChangeGender(TA_GenderButton button)
        {
            if (button == male) Male();
            if (button == female) Female();
        }

        void Male()
        {
            Gender = 1;
            male.Active();
            female.Passive();

            AllowConfirm();
        }

        void Female()
        {
            Gender = 2;
            male.Passive();
            female.Active();

            AllowConfirm();
        }
    }
}