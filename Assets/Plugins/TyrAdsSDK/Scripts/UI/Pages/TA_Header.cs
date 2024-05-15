using System;
using System.Collections;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_Header : TA_Page
    {
        public Image background;
        [SerializeField] RectTransform animTransform;
        [SerializeField] float animRotateSpeed = 150;
        public TA_HeaderModeSettings[] settings;
        public TextMeshProUGUI title;
        public Button backButton;
        public event Action OnBackButton = delegate { };
        public event Action OnHelpButton = delegate { };
        public TA_TPoints points;
        public TA_HelpButton helpButton;
        [field: SerializeField] public TA_HeaderMode Mode { get; private set; }
        bool _isAnimate;

        void Update()
        {
            if (!_isAnimate) return;
            var z = animTransform.localRotation.eulerAngles.z;
            z += Time.deltaTime * animRotateSpeed;
            animTransform.localRotation = Quaternion.Euler(0, 0, z);
        }

        public void SetMode(TA_HeaderMode mode)
        {
            Mode = mode;
            var find = settings.FirstOrDefault(s => s.mode == mode);
            if (find == null) return;

            SetTitle(find.title);
            if (find.helpMode == TA_HelpButtonMode.Disabled)
            {
                HideHelp();
            }
            else
            {
                var showHelpTxt = find.helpMode == TA_HelpButtonMode.Help;
                ShowHelp(showHelpTxt);
            }
        }

        protected override void Showing()
        {
            StopRefresh();
            backButton.interactable = true;
            backButton.onClick.AddListener(Back);
            helpButton.OnClick += OnHelpClick;
        }

        protected override void Hiding()
        {
            backButton.onClick.RemoveListener(Back);
            helpButton.OnClick -= OnHelpClick;
        }

        public void RefreshAnimation()
        {
            _isAnimate = true;
            animTransform.gameObject.SetActive(true);
        }

        public void StopRefresh()
        {
            _isAnimate = false;
            animTransform.gameObject.SetActive(false);
        }

        void OnHelpClick() => OnHelpButton();

        void Back()
        {
            StartCoroutine(Shit());
            OnBackButton();
        }

        IEnumerator Shit()
        {
            backButton.interactable = false;
            yield return new WaitForSeconds(0.1f);
            backButton.interactable = true;
        }

        public void SetTitle(string msg)
            => title.text = msg;

        public void SetPoints(double value, string type)
            => points.SetPoints(value, type);

        public void ShowPoints()
        {
            points.Show();
        }

        public void HidePoints()
        {
            points.Hide();
        }

        public void ShowHelp(bool showText = false)
        {
            helpButton.Show();

            if (showText) helpButton.ShowTitle();
            else helpButton.HideTitle();
        }

        public void HideHelp()
        {
            helpButton.Hide();
        }
    }

    [Serializable]
    public class TA_HeaderModeSettings
    {
        public TA_HeaderMode mode;
        public string title;
        public TA_HelpButtonMode helpMode;
    }

    public enum TA_HelpButtonMode
    {
        Disabled,
        Help,
        FAQ
    }

    public enum TA_HeaderMode
    {
        Offers,
        ActiveOffers,
        OfferDetails,
        Help,
        SelectTicket,
        TicketSubmitted,
        FAQ,
    }
}