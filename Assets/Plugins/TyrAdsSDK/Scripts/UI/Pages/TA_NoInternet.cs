using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_NoInternet : TA_Page
    {
        [SerializeField] float animRotateSpeed = 150;
        [SerializeField] RectTransform animTransform;

        protected override void Showing()
        {
            enabled = true;
        }

        protected override void Hiding()
        {
            enabled = false;
        }

        void Update()
        {
            var z = animTransform.localRotation.eulerAngles.z;
            z += Time.deltaTime * animRotateSpeed;
            animTransform.localRotation = Quaternion.Euler(0, 0, z);
        }
    }
}