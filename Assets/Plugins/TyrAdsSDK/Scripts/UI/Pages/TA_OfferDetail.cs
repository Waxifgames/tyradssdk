using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using TA_Currency = Plugins.TyrAdsSDK.Scripts.Caching.TA_Currency;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_OfferDetail : TA_Page
    {
        #region Inspector
        [SerializeField] TA_RefreshPageDetector refresher;
        [SerializeField] RectTransform content;
        [SerializeField] CanvasGroup microCharges;

        [Space(20)]
        [SerializeField] TA_OfferGoal goalPrefab;
        [SerializeField] TA_OfferGoal purchaseGoalPrefab;

        [Space(20)]
        [SerializeField] List<TA_OfferGoal> goals = new();
        [SerializeField] List<TA_OfferGoal> purchaseGoals = new();

        [Space(20)]
        [SerializeField] Transform goalsContainer;
        [SerializeField] Transform purchaseGoalsContainer;

        [Space(20)]
        [SerializeField] TextMeshProUGUI totalMicroChargesPoints;
        [SerializeField] TextMeshProUGUI earnedMicroChargesPoints;
        [SerializeField] TextMeshProUGUI downloadHint;

        [Space(20)]
        public TA_OfferItem offer;
        public TextMeshProUGUI description;

        [Space(20)]
        [SerializeField] float animRotateSpeed = 250;
        [SerializeField] RectTransform animTransform;

        public event Action<TA_DetailCampaign> OnPlayNow = delegate { };
        public event Action<int, string, bool> OnRequestDetails = delegate { };
        public event Action<TA_DetailCampaign> OnMoveToActive = delegate { };
        public event Action<TA_DetailCampaign> OnRequestTickets = delegate { };

        public bool IsRefreshing => refresher._isAnimate;
        bool _isActive;
        bool _isAnimate;
        TA_Downloader _downloader;
        TA_DetailCampaign Data { get; set; }
        #endregion

        public void Init(TA_Downloader downloader)
        {
            refresher.OnRefresh += RequestDetails;
            _downloader = downloader;
            StopAnimation();
        }

        void Update()
        {
            if (!_isAnimate) return;
            var z = animTransform.localRotation.eulerAngles.z;
            z += Time.deltaTime * animRotateSpeed;
            animTransform.localRotation = Quaternion.Euler(0, 0, z);
        }

        public void Refresh(TA_DetailCampaign data)
        {
            content.sizeDelta = new Vector2(0, 2500);
            downloadHint.enabled = true;
            _isActive = false;
            microCharges.alpha = 0;
            Data = data;
            description.text = data.app.shortDescription;

            offer.OnPlay -= PlayButton;
            offer.OnPlay += PlayButton;

            RefreshOffer(data);
            RefreshAllGoals(data);
            DownloadImages(data);
        }

        public void RefreshActive(TA_DetailCampaign data)
        {
            Refresh(data);
            downloadHint.enabled = false;
            content.sizeDelta = new Vector2(0, 3200);
            _isActive = true;
            microCharges.alpha = 1;
        }

        public void RefreshAllGoals(TA_DetailCampaign data)
        {
            StopAnimation();
            ClearGoals();
            RefreshGoals(data);
            RefreshPurchaseGoals(data);
        }

        public void HideCategory()
        {
            offer.category.Hide();
            offer.difficulty.Hide();
        }

        public void Clear()
        {
            description.text = string.Empty;
            downloadHint.enabled = false;
        }

        public void RefreshOffer(TA_DetailCampaign data)
        {
            offer.SetName(data.app.title);
            offer.SetCategory(data.app.storeCategory);
            offer.SetDifficulty(data.targeting.reward.rewardDifficulty);
            offer.SetTotalRewards(data.payoutEvents.Count);
            offer.SetPoints(data.campaignPayout.totalPayoutConverted, string.Empty);
            offer.SetCurrencyName(data.currency.adUnitCurrencyName);
            description.text = data.app.shortDescription;
        }

        void RefreshGoals(TA_DetailCampaign data)
        {
            List<TA_PayoutEvent> notPurchases = new();

            foreach (var payout in data.payoutEvents)
            {
                if (!IsEmpty(payout)) continue;
                notPurchases.Add(payout);
            }

            TA_PayoutEvent current = notPurchases.Count > 0 ? notPurchases[0] : null;

            foreach (var payout in data.payoutEvents)
            {
                if (IsEmpty(payout)) continue;
                if (IsPurchase(payout)) continue;

                var goal = Instantiate(goalPrefab, goalsContainer);
                RefreshPayout(goal, payout, data);

                if (data.isInstalled) goal.Available();
                else goal.NotAvailable();

                goal.SetStatus(payout.conversionStatus, payout == current);

                if (payout.enforceMaxTimeCompletion)
                    goal.SetRemainingTime((int)payout.maxTimeRemainSeconds);

                goals.Add(goal);
            }
        }

        void RefreshPurchaseGoals(TA_DetailCampaign data)
        {
            double earn = 0;
            double possibleEarn = 0;

            foreach (var payout in data.payoutEvents)
            {
                var isComplete = IsComplete(payout.conversionStatus);

                if (IsEmpty(payout)) continue;
                if (NotPurchase(payout)) continue;

                var goal = Instantiate(purchaseGoalPrefab, purchaseGoalsContainer);
                RefreshPayout(goal, payout, data);

                goal.SetComplete(isComplete);
                goal.NoCompleted();

                if (payout.enforceMaxTimeCompletion)
                    goal.SetRemainingTime((int)payout.maxTimeRemainSeconds);

                purchaseGoals.Add(goal);

                if (isComplete) earn += payout.payoutAmountConverted;
                possibleEarn += payout.payoutAmountConverted;
            }

            var type = data.currency.adUnitCurrencyName;
            var earned = TA_Currency.ConvertTPoints(earn);
            var total = TA_Currency.ConvertTPoints(possibleEarn);
            var earnedTxt = "<b>" + earned.amount.ToString(earned.format) + earned.suffix + "</b>" + " " + type;
            var totalTxt = "<b>" + total.amount.ToString(total.format) + total.suffix + "</b>" + " " + type;

            earnedMicroChargesPoints.text = "you earned: <b>" + earnedTxt;
            totalMicroChargesPoints.text = "from total <b>" + totalTxt;
        }

        void RefreshPayout(TA_OfferGoal goal, TA_PayoutEvent payout, TA_DetailCampaign data)
        {
            goal.SetID(payout.id);
            goal.SetName(payout.eventName);
            goal.SetPoints(payout.payoutAmountConverted, data.currency.adUnitCurrencyName);
            goal.SetCategory(payout.eventCategory);
        }


        void DownloadImages(TA_DetailCampaign data)
        {
            _downloader.DownloadIcon(data, offer);
            _downloader.DownloadPreview(data, offer);
        }


        public void PlayAnimation()
        {
            _isAnimate = true;
            animTransform.gameObject.SetActive(true);
        }

        public void StopAnimation()
        {
            _isAnimate = false;
            animTransform.gameObject.SetActive(false);
        }

        public void ClearGoals()
        {
            ClearGoals(goals, goalsContainer);
            ClearGoals(purchaseGoals, purchaseGoalsContainer);
        }

        void ClearGoals(List<TA_OfferGoal> list, Transform container)
        {
            if (list.Count == 0)
                list = container.GetComponentsInChildren<TA_OfferGoal>().ToList();

            foreach (var item in list)
                Destroy(item.gameObject);

            list.Clear();
        }

        void PlayButton(TA_OfferItem item)
        {
            OnPlayNow(Data);

            if (_isActive) return;
            OnMoveToActive(Data);
        }


        public void SetIcon(Texture2D texture) => offer.SetIcon(texture);
        public void SetPreview(Texture2D texture) => offer.SetPreview(texture);
        public void ClearCurrencyName() => offer.SetCurrencyName("");
        public void RequestTickets() => OnRequestTickets(Data);


        bool IsComplete(string status) => status.Equals("approved");
        bool IsPurchase(TA_PayoutEvent payout) => payout.eventCategory.Equals("Purchase");
        bool NotPurchase(TA_PayoutEvent payout) => !IsPurchase(payout);
        bool IsEmpty(TA_PayoutEvent payout) => payout.eventCategory.Equals(string.Empty) || ZeroAmount(payout);
        bool ZeroAmount(TA_PayoutEvent payout) => payout.payoutAmountConverted == 0f;
        void RequestDetails() => OnRequestDetails(Data.campaignId, Data.app.packageName, _isActive);


    }
}