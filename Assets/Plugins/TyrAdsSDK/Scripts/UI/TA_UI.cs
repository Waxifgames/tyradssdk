using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.Caching;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using Plugins.TyrAdsSDK.Scripts.UI.Pages;
using UnityEngine;
using TA_Currency = Plugins.TyrAdsSDK.Scripts.Caching.TA_Currency;

namespace Plugins.TyrAdsSDK.Scripts.UI
{
    [AddComponentMenu("")]
    public class TA_UI : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] TA_Access access;
        [SerializeField] TA_UserEntry userEntry;

        [Space(10)]
        [SerializeField] TA_Offers offers;
        [SerializeField] TA_OfferDetail offerDetail;

        [Space(10)]
        [SerializeField] TA_SupportTickets supportTickets;
        [SerializeField] TA_Help help;
        [SerializeField] TA_FaqPage faq;
        [SerializeField] TA_SupportTicketSuccess submitTicketSuccess;

        [Space(10)]
        // [SerializeField] Button closeButton;
        [SerializeField] TA_NoInternet noInternet;
        [SerializeField] TA_Header header;
        [SerializeField] TA_Background background;
        public TA_Header Header => header;

        [Space(30)]
        [TA_ReadOnly] TA_Page[] _pages;
        [TA_ReadOnly][SerializeField] TA_Page currentPage;
        [TA_ReadOnly][SerializeField] TA_Page lastPage;
        [TA_ReadOnly][SerializeField] bool _activeMode;
        [TA_ReadOnly][SerializeField] bool _activeOffersMode;
        bool _useCache, _showingDetails;
        TA_CacheSaver _cache;
        TA_Downloader _downloader;
        TA_SDK _sdk;
        public TA_UserEntry UserEntry => userEntry;
        bool IsShowing
        {
            get
            {
                if (_showingDetails) return true;
                _showingDetails = true;
                return false;
            }
        }

        #region Actions

        public event Action OnHide = delegate { };
        //user
        public event Action OnAccessGranted = delegate { };
        public event Action OnUserGranted = delegate { };

        //offer
        public event Action OnRequestOffers = delegate { };
        public event Action OnRequestActiveOffers = delegate { };
        public event Action<int> OnRequestDetails = delegate { };
        public event Action<TA_DetailCampaign> OnActiveACampaign = delegate { };
        public event Action<TA_DetailCampaign> OnPlayNow = delegate { };

        //support
        public event Action<TA_DetailCampaign> OnRequestTickets = delegate { };
        public event Action<List<TA_TicketItem>> OnSubmitTickets = delegate { };
        void SubmitTickets(List<TA_TicketItem> tickets)
        {
            ShowPage(submitTicketSuccess);
            OnSubmitTickets(tickets);
        }

        void RequestTickets(TA_DetailCampaign data) => OnRequestTickets(data);
        void MoveToActive(TA_DetailCampaign obj) => OnActiveACampaign(obj);
        void PlayNow(TA_DetailCampaign obj) => OnPlayNow(obj);

        void RequestOffers()
        {
            _activeOffersMode = false;
            SwitchHeader(offers);
            CheckCache();
            OnRequestOffers();
        }

        void RequestActiveOffers()
        {
            _activeOffersMode = true;
            SwitchHeader(offers);
            CheckActiveCache();
            OnRequestActiveOffers();
        }

        #endregion

        public void Init(TA_SDK controller, TA_Downloader downloader, TA_CacheSaver cache)
        {
            _sdk = controller;
            _cache = cache;
            _downloader = downloader;

            offers.Init(downloader);
            offerDetail.Init(downloader);

            offers.OnShowDetails += ShowDetails;
            offers.OnShowActiveDetails += ShowActiveDetails;
            offers.OnRequestOffers += RequestOffers;
            offers.OnRequestActiveOffers += RequestActiveOffers;

            offerDetail.OnRequestDetails += RefreshDetails;
            offerDetail.OnPlayNow += PlayNow;
            offerDetail.OnMoveToActive += MoveToActive;
            offerDetail.OnRequestTickets += RequestTickets;

            supportTickets.OnSubmit += SubmitTickets;
            submitTicketSuccess.OnBack += BackToDetails;

            header.OnBackButton += Back;
            header.OnHelpButton += Help;
        }



        public void Hide()
        {
            OnHide();
            if (_pages == null || _pages.Length == 0)
                _pages = GetComponentsInChildren<TA_Page>(includeInactive: true);
            foreach (var page in _pages)
                page.Hide();
        }

        public void SetTotalActiveOffers(int amount)
            => offers.SetTotalActiveOffers(amount);

        public void RefreshOffers(TA_Campaign[] data, bool isActive)
        {
            if (_activeOffersMode != isActive) return;

            offers.Refresh(data);
            header.StopRefresh();
            offers.StopRefreshingPage();
        }

        public void RefreshTickets(TA_SupportTicketCategory[] tickets, int campaignID)
        {
            header.StopRefresh();
            supportTickets.Refresh(tickets, campaignID);
        }

        public void RefreshDetail(TA_DetailCampaign data)
        {
            header.StopRefresh();
            _downloader.SetMode(DownloadMode.OfferDetail);

            if (_activeMode)
                offerDetail.RefreshActive(data);
            else
                offerDetail.Refresh(data);

            _cache.Add(data);
        }

        void RefreshDetails(int campaignID, string packageName, bool isActive)
        {
            if (isActive)
                ShowActiveDetails(campaignID, packageName);
            else
                ShowDetails(campaignID, packageName);
        }
        void BackToDetails()
        {

        }

        void ShowDetails(int campaignID, string packageName)
            => Details(campaignID, packageName, isActive: false);

        void ShowActiveDetails(int campaignID, string packageName)
            => Details(campaignID, packageName, isActive: true);

        void Details(int campaignID, string packageName, bool isActive)
        {
            if (IsShowing) return;

            _activeMode = isActive;

            offerDetail.Clear();
            CheckCache(campaignID, packageName);
            OnRequestDetails(campaignID);
            ShowDetailPage();
            // Invoke(nameof(ShowDetailPage), 0.1f);
        }

        void ShowDetailPage()
        {
            _showingDetails = false;
            offers.Hide();
            offerDetail.ClearGoals();
            ShowPage(offerDetail);
        }

        void CheckCache(int campaignID, string packageName)
        {
            var campaigns = _sdk.TargetedCampaignsList;
            var find = campaigns.FirstOrDefault(c => c.campaignId == campaignID);
            if (find != null)
            {
                offerDetail.offer.SetName(find.app.title);
                offerDetail.offer.SetCategory(find.app.storeCategory);
                offerDetail.offer.SetDifficulty(find.targeting.reward.rewardDifficulty);
                offerDetail.description.text = find.app.shortDescription;
                _downloader.DownloadIcon(find, offerDetail.offer);
                _downloader.DownloadPreview(find, offerDetail.offer);
            }

            if (_cache.Has(campaignID))
            {
                var campaign = _cache.Get(campaignID);
                offerDetail.RefreshOffer(campaign);
                offerDetail.RefreshAllGoals(campaign);
            }
            else
            {
                offerDetail.PlayAnimation();
                offerDetail.ClearGoals();
            }

            if (_cache.HasIcon(packageName))
            {
                var texture = _cache.GetIcon(packageName);
                offerDetail.SetIcon(texture);
            }

            if (_cache.HasPreview(packageName))
            {
                var texture = _cache.GetPreview(packageName);
                offerDetail.SetPreview(texture);
            }
        }

        void CheckCache()
        {
            var campaigns = _sdk.TargetedCampaignsList;
            offers.Refresh(campaigns);
        }

        void CheckActiveCache()
        {
            var campaigns = _sdk.ActiveCampaignsList;
            offers.Refresh(campaigns);
        }

        #region Access and UserInfo

        public void ShowAccess()
        {
            access.Show();
            access.OnConfirm += AccessGranted;
        }

        void AccessGranted() => OnAccessGranted();

        public void HideAccess()
        {
            access.OnConfirm -= AccessGranted;
            access.Hide();
        }

        public void ShowUserEntry()
        {
            userEntry.Show();
            userEntry.OnConfirm += UserEntryConfirmed;
        }

        void UserEntryConfirmed() => OnUserGranted();

        public void HideUserEntry()
        {
            userEntry.OnConfirm -= UserEntryConfirmed;
            userEntry.Hide();
        }

        #endregion

        public void ShowInternetMissing()
        {
            noInternet.Show();
        }

        public void HideInternetMissing()
        {
            noInternet.Hide();
        }

        public void ShowMainPage()
        {
            background.Show();
            header.Show();
            CheckCache();
            offers.SwitchToNormalMode();
            ShowPage(offers);
        }

        void ShowPage(TA_Page page)
        {
            if (currentPage)
            {
                lastPage = currentPage;
                currentPage.Hide();
            }

            if (!page) return;
            page.Show();
            page.OnShowPlease += OpenKindly;
            SwitchHeader(page);

            currentPage = page;
        }

        void SwitchHeader(TA_Page page)
        {
            if (page == offers)
            {
                if (!offers.IsRefreshing) header.RefreshAnimation();
                header.SetMode(_activeOffersMode ? TA_HeaderMode.ActiveOffers : TA_HeaderMode.Offers);
            }

            if (page == offerDetail)
            {
                if (!offerDetail.IsRefreshing) header.RefreshAnimation();
                header.SetMode(TA_HeaderMode.OfferDetails);
            }

            if (page == help)
                header.SetMode(TA_HeaderMode.Help);
            if (page == faq)
                header.SetMode(TA_HeaderMode.FAQ);

            if (page == supportTickets)
                header.SetMode(TA_HeaderMode.SelectTicket);
            if (page == submitTicketSuccess)
                header.SetMode(TA_HeaderMode.TicketSubmitted);
        }

        void Back()
        {
            if (header.Mode == TA_HeaderMode.Offers)
            {
                Hide();
            }

            if (header.Mode == TA_HeaderMode.ActiveOffers)
            {
                ShowPage(offers);
                offers.SwitchToNormalMode();
            }

            if (header.Mode == TA_HeaderMode.OfferDetails)
            {
                ShowPage(offers);
                if (_activeOffersMode)
                    offers.SwitchToActiveMode();
                else
                    offers.SwitchToNormalMode();
            }


            if (header.Mode == TA_HeaderMode.Help)
            {
                ShowPage(offers);
                if (_activeOffersMode)
                    offers.SwitchToActiveMode();
                else
                    offers.SwitchToNormalMode();
            }

            if (header.Mode == TA_HeaderMode.FAQ)
            {
                if (lastPage == help)
                {
                    ShowPage(help);
                }
                else
                {
                    ShowPage(offers);
                    if (_activeOffersMode)
                        offers.SwitchToActiveMode();
                    else
                        offers.SwitchToNormalMode();
                }
            }

            if (header.Mode == TA_HeaderMode.SelectTicket)
            {
                ShowPage(help);
            }

            if (header.Mode == TA_HeaderMode.TicketSubmitted)
            {
                ShowPage(offerDetail);
            }
        }

        void Help()
        {
            if (header.Mode == TA_HeaderMode.Offers)
                ShowPage(faq);
            if (header.Mode == TA_HeaderMode.ActiveOffers)
            {
                ShowPage(faq);
            }

            if (header.Mode == TA_HeaderMode.OfferDetails)
            {
                ShowPage(help);
                offerDetail.RequestTickets();
                help.tickets.SetLabel("I didn't receive my " + TA_Currency.Name);
               
            }
            // if (header.Mode == HeaderMode.Help)
            //     ShowPage(help);
            // if (header.Mode == HeaderMode.FAQ)
            //     ShowPage(faq);
            //
            // if (header.Mode == HeaderMode.SelectTicket)
            //     header.SetMode(HeaderMode.SelectTicket);
            // if (header.Mode == HeaderMode.TicketSubmitted)
            //     header.SetMode(HeaderMode.TicketSubmitted);
        }

        void OpenKindly(TA_Page current, TA_Page asked)
        {
            current.OnShowPlease -= OpenKindly;
            current.Hide();
            asked._previousPage = current;
            ShowPage(asked);
        }
    }
}