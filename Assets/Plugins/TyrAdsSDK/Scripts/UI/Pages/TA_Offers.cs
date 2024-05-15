using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_Offers : TA_Page
    {
        [Space(10)]
        [SerializeField] TextMeshProUGUI activeOffersAmount;
        [SerializeField] TA_TPoints userPoints;
        [SerializeField] TA_RefreshPageDetector refresher;

        [Space(20)]
        [SerializeField] Transform container;
        [SerializeField] TA_ScrollOffers scroll;
        [SerializeField] Button hideActiveOffers;
        [SerializeField] Button showActiveOffersButton;

        [Space(20)]
        [SerializeField] [TA_ReadOnly] List<TA_OfferItem> list = new();
        [SerializeField] [TA_ReadOnly] List<TA_Campaign> showList = new();
        [SerializeField] [TA_ReadOnly] bool activeMode;

        public TA_Downloader Downloader { get; private set; }
        public event Action<int, string> OnShowDetails = delegate { };
        public event Action OnRequestOffers = delegate { };
        public event Action OnRequestActiveOffers = delegate { };
        public event Action<int, string> OnShowActiveDetails = delegate { };

        bool _buttonsDisabled;

        public void Init(TA_Downloader downloader)
        {
            Downloader = downloader;
            scroll.Init();
            refresher.OnRefresh += RequestOffers;
        }

        void RequestOffers()
        {
            if (activeMode)
                OnRequestActiveOffers();
            else
                OnRequestOffers();
        }

        public void StopRefreshingPage()
        {
            refresher.StopRefresh();
        }

        public bool IsRefreshing => refresher._isAnimate;

        public void Refresh(TA_Campaign[] data)
        {
            scroll.Refresh(data);
        }

        public void SetTotalActiveOffers(int a)
            => activeOffersAmount.text = "+" + a + " Active offers!";

        public void ShowDetails(TA_OfferItem item)
        {
            if (activeMode)
                OnShowActiveDetails(item.campaignID, item.packageName);
            else
                OnShowDetails(item.campaignID, item.packageName);
        }

        protected override void Showing()
        {
            SubscribeButtons();
        }

        protected override void Hiding()
        {
            UnSubscribeButtons();
        }

        public void SwitchToNormalMode()
        {
            if (_buttonsDisabled) return;

            Clear();
            OnRequestOffers();
            NormalMode();

            DisableButtonsForWhile();
        }

        public void SwitchToActiveMode()
        {
            if (_buttonsDisabled) return;

            Clear();
            OnRequestActiveOffers();
            ActiveMode();

            DisableButtonsForWhile();
        }

        void DisableButtonsForWhile()
        {
            if (gameObject.activeSelf)
            {
                DisableButtons();
                Invoke(nameof(EnableButtons), 0.1f);
            }
        }

        void DisableButtons()
        {
            _buttonsDisabled = true;
        }

        void EnableButtons()
        {
            _buttonsDisabled = false;
        }

        public void NormalMode()
        {
            activeMode = false;
            showActiveOffersButton.gameObject.SetActive(true);
            hideActiveOffers.gameObject.SetActive(false);
        }

        public void ActiveMode()
        {
            activeMode = true;
            showActiveOffersButton.gameObject.SetActive(false);
            hideActiveOffers.gameObject.SetActive(true);
        }

        void Clear()
        {
            if (list.Count == 0)
                list = container.GetComponentsInChildren<TA_OfferItem>().ToList();

            for (var i = 0; i < list.Count; i++)
            {
                if (i < 2) continue;
                var item = list[i];
                item.Hide();
            }

            showList.Clear();
        }

        void SubscribeButtons()
        {
            showActiveOffersButton.onClick.AddListener(SwitchToActiveMode);
            hideActiveOffers.onClick.AddListener(SwitchToNormalMode);
        }

        void UnSubscribeButtons()
        {
            showActiveOffersButton.onClick.RemoveListener(SwitchToActiveMode);
            hideActiveOffers.onClick.RemoveListener(SwitchToNormalMode);
        }
    }
}