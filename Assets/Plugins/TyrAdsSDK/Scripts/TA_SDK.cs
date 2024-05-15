using Plugins.TyrAdsSDK.Scripts.API;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Extra;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.Caching;
using Plugins.TyrAdsSDK.Scripts.DATA;
using Plugins.TyrAdsSDK.Scripts.UI;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using Plugins.TyrAdsSDK.Scripts.UI.Pages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.TyrAdsSDK.Scripts
{
    [AddComponentMenu("")]
    public class TA_SDK : MonoBehaviour
    {
        #region Inspector
        [Space(10)]
        [SerializeField] string apiKey;
        [SerializeField] string apiSecret;

        [Space(10)]
        [SerializeField] Color headerColor;

        [Space(10)]
        public UnityEvent onExit;
        //  public UnityEvent<int> onPlayerGetReward;

        TA_API _api;
        TA_UI _ui;
        TA_DATA _data;
        TA_Campaign[] _real, _newTest;
        /*[Header("TEST")]
        int _testCampaigns;
        int _total;
        bool UseTest => _testCampaigns > 0;*/
        #endregion

        #region Access
        public TA_Campaign[] TargetedCampaignsList => _api.targetedCampaigns.Campaigns;
        public TA_Campaign[] ActiveCampaignsList => _api.activatedCampaigns.Campaigns;
        public bool IsInternet => TA_Web.IsInternetConnection;
        bool IsNewUser => _api.server.initialized && _api.initialize.Response.newRegisteredUser;
        bool AccessRequired => Save.AccessRequired;
        bool UserRequired => Save.UserDataRequired;
        bool NullIdentifier => _api.initialize.PostContent.identifier.Equals(string.Empty);

        public TA_Downloader Downloader => _data.imageDownloader;
        TA_DetailCampaign DetailsData => _api.targetedCampaignDetail.Details;
        TA_SupportTicketCategory[] SupportTicketsList => _api.campaignSupportTickets.Categories;
        TA_PrefsSave Save => _data.save;
        TA_CacheSaver Cache => _data.cache;
        TA_TargetedCampaigns TargetedCampaigns => _api.targetedCampaigns;
        TA_ActivatedCampaigns ActivatedCampaigns => _api.activatedCampaigns;
        TA_CampaignSupportTickets Tickets => _api.campaignSupportTickets;
        TA_SubmitSupportTicket SubmitTicket => _api.submitSupportTickets;
        TA_ActiveACampaign ActivateCampaign => _api.activeACampaign;
        TA_TargetedCampaignDetail Detals => _api.targetedCampaignDetail;


        #endregion

        void Awake()
        {
            FindComponents();
            SetServer();

            _api.initialize.PostContent.deviceData = TyrAds.DeviceData;
            _ui.Header.background.color = headerColor;
            _ui.OnHide += OnHide;

            Init();
            Hide();
            RefreshPublisherUserID("asking API...");
        }

        void Start()
        {
            Invoke(nameof(Initialize), 0.1f);
        }

        void Init()
        {
            var useCache = true;
            var showDebug = true;
            var showDownloadingDebug = false;

            Downloader.Init(Cache, useCache);
            Cache.Init(useCache);
            RefreshAPI();
            _ui.Init(this, Downloader, Cache);

            TyrAds.SetSDK(this);
            TyrAds.SetAPI(_api);
            TyrAds.SetUI(_ui);
            TyrAds.SetIcon(Resources.Load<Texture2D>("TA_points"));
            TA_Debug.disableAllDebug = !showDebug;
            TA_Debug.disableDownloadDebug = !showDownloadingDebug;
        }

        public void SetProfile(string userId, int gender, int yearOfBirth)
        {
            int currentYear = System.DateTime.Now.Year;
            int age = currentYear - yearOfBirth;

            _api.initialize.PostContent.publisherUserId = userId;
            _api.initialize.PostContent.age = age;
            _api.initialize.PostContent.gender = gender;
            Save.UserConfirmed();
        }

        public void SetGAID(string gaid)
        {
            Save.SaveGAID(gaid);
            _api.initialize.SetGAID(gaid);
        }

        //public void InvokeRewardEvent(int currencyAmount)
        //{
        //    onPlayerGetReward.Invoke(currencyAmount);
        //}

        void FindComponents()
        {
            _api = GetComponentInChildren<TA_API>();
            _ui = GetComponentInChildren<TA_UI>();
            _data = GetComponentInChildren<TA_DATA>();
        }

        void SetServer()
        {
            _api.server.address = "api.tyrads.com";
            _api.server.version = "v1";
            _api.server.apiKey = apiKey;
            _api.server.apiSecret = apiSecret;
        }

        void OnHide()
        {
            onExit?.Invoke();
        }

        void RefreshPublisherUserID(string id)
        {
            // publisherUserIdTxt.text = "Publisher User ID (received from API): \n<color=green>" + id + "</color>";
        }



        void RefreshAPI()
            => _api.RefreshData();

        void Preload()
        {
            if (!IsInternet) return;
            StartCoroutine(PreloadCampaigns());
        }

        IEnumerator PreloadCampaigns()
        {
            yield return TA_Web.Request(TargetedCampaigns);
            yield return TA_Web.Request(ActivatedCampaigns);
            _ui.SetTotalActiveOffers(ActiveCampaignsList.Length);

            if (TargetedCampaignsList.Length > 0)
            {
                var campaign = TargetedCampaignsList[0];
                Downloader.DownloadTPointsIcon(campaign.currency.adUnitCurrencyIcon);
                TA_Currency.SetName(campaign.currency.adUnitCurrencyName);
            }
            else if (ActiveCampaignsList.Length > 0)
            {
                var campaign = ActiveCampaignsList[0];
                Downloader.DownloadTPointsIcon(ActiveCampaignsList[0].currency.adUnitCurrencyIcon);
                TA_Currency.SetName(campaign.currency.adUnitCurrencyName);
            }
        }

        public void Show()
        {
            if (!IsInternet)
            {

                StartCoroutine(TA_Web.WaitForInternet());
                return;
            }

            if (IsNewUser && AccessRequired)
            {
                ShowAccess();
                return;
            }

            if (IsNewUser && UserRequired)
            {
                ShowUserEntry();
                return;
            }

            ShowMainPage();
        }


        public void Hide()
        {
            _ui.Hide();
        }

        void ShowMainPage()
        {
            _ui.OnRequestDetails -= ShowDetails;
            _ui.OnRequestOffers -= RequestOffers;
            _ui.OnRequestActiveOffers -= RequestActiveOffers;
            _ui.OnPlayNow -= PlayNow;
            _ui.OnActiveACampaign -= ActiveACampaign;
            _ui.OnRequestTickets -= RequestTickets;
            _ui.OnSubmitTickets -= SubmitTickets;

            _ui.OnRequestDetails += ShowDetails;
            _ui.OnRequestOffers += RequestOffers;
            _ui.OnRequestActiveOffers += RequestActiveOffers;
            _ui.OnPlayNow += PlayNow;
            _ui.OnActiveACampaign += ActiveACampaign;
            _ui.OnRequestTickets += RequestTickets;
            _ui.OnSubmitTickets += SubmitTickets;

            _ui.ShowMainPage();
        }

        void SubmitTickets(List<TA_TicketItem> tickets)
        {
            if (tickets.Count == 0) return;

            var ticket = tickets[0];
            var campaignID = ticket.campaignID;
            var ticketID = ticket.ID;

            SubmitTicket.SetCampaignID(campaignID);
            SubmitTicket.SetTicketID(ticketID);
            SubmitTicket.Request();
        }

        void RequestTickets(TA_DetailCampaign campaign)
            => StartCoroutine(ShowTickets(campaign));

        IEnumerator ShowTickets(TA_DetailCampaign campaign)
        {
            Tickets.SetCampaignID(campaign.campaignId);
            yield return TA_Web.Request(Tickets);
            _ui.RefreshTickets(SupportTicketsList, campaign.campaignId);
        }

        void PlayNow(TA_DetailCampaign campaign)
        {
            OpenGameLink(campaign.tracking.clickUrl);
        }

        void ActiveACampaign(TA_DetailCampaign campaign)
        {
            ActivateCampaign.SetCampaignID(campaign.campaignId);
            ActivateCampaign.Request();
        }

        void OpenGameLink(string url)
        {
            Application.OpenURL(url);
        }

        #region Access & User

        void ShowAccess()
        {
            _ui.ShowAccess();
            _ui.OnAccessGranted += AccessGranted;
        }

        void AccessGranted()
        {
            Save.AccessGranted();
            _ui.HideAccess();
            if (UserRequired) ShowUserEntry();
            else ShowMainPage();
        }

        void ShowUserEntry()
        {
            _ui.ShowUserEntry();
            _ui.OnUserGranted += UserGranted;
        }

        void UserGranted()
        {
            var entry = _ui.UserEntry;
            var newUser = CreateNewUser(entry);
            Save.SaveUser(newUser);
            Save.UserConfirmed();
            _api.initialize.SetUser(newUser);

            _ui.HideUserEntry();
            ShowMainPage();
            Initialize();
        }

        TA_User CreateNewUser(TA_UserEntry entry)
            => new() { age = entry.Age, gender = entry.Gender };

        void Initialize()
            => StartCoroutine(Initialization());

        IEnumerator Initialization()
        {
            if (NullIdentifier)
                SetRandomIdentifier();

            yield return TA_Web.Post(_api.initialize); //TODO: WHAT IF SERVER ERROR???

            var id = _api.initialize.Response.user.publisherUserId;
            _api.server.publisherUserID = id;
            RefreshPublisherUserID(id);

            _api.server.initialized = true;
            RefreshAPI();
            Preload();
        }


        void SetRandomIdentifier()
        {
            string id;
            if (Save.HasIdentifier)
            {
                id = Save.Identifier;
            }
            else
            {
                id = Guid.NewGuid().ToString();
                Save.SaveOtherIdentifier(id);
            }

            _api.initialize.PostContent.identifier = id;
            _api.initialize.PostContent.identifierType = "OTHER";
        }

        #endregion

        void ShowDetails(int campaignID)
        {
            Detals.SetCampaignID(campaignID);
            StartCoroutine(ShowDetails());
        }

        IEnumerator ShowDetails()
        {
            yield return RequestDetails();
            _ui.RefreshDetail(DetailsData);
        }

        Coroutine RequestDetails()
            => StartCoroutine(TA_Web.Details(Detals));

        void RequestOffers()
            => StartCoroutine(TA_Web.Offers(_api.targetedCampaigns, isActive: false));

        void RequestActiveOffers()
            => StartCoroutine(TA_Web.Offers(_api.activatedCampaigns, isActive: true));
    }
}