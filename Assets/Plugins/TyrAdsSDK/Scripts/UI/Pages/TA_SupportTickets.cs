using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.TyrAdsSDK.Scripts.UI.Pages
{
    [AddComponentMenu("")]
    public class TA_SupportTickets : TA_PageWithConfirmation
    {
        [SerializeField] [TA_ReadOnly] bool onlyOneTicketCanBeSubmitted = true;

        [Space(20)]
        [SerializeField] Button submitButton;
        [SerializeField] TA_TicketItem prefab;
        [SerializeField] Transform tasksContainer;
        [SerializeField] Transform purchaseContainer;
        [SerializeField] CanvasGroup loading;
        [Space(20)]
        [SerializeField] List<TA_TicketItem> tickets = new();
        [SerializeField] List<TA_TicketItem> purchaseTickets = new();

        [Space(20)]
        [SerializeField] List<TA_TicketItem> submitted = new();

        public event Action<List<TA_TicketItem>> OnSubmit = delegate { };

        protected override void Showing()
        { 
            loading.alpha = 1;
            submitButton.interactable = false;
            submitButton.onClick.AddListener(Submit);
        }

        protected override void Hiding() 
            => submitButton.onClick.RemoveListener(Submit);

        void Submit() 
            => OnSubmit(submitted);

        public void Refresh(TA_SupportTicketCategory[] categories, int campaignID)
        {
            loading.alpha = 0;
            ClearTickets();
            RefreshTasks(categories, campaignID);
            RefreshPurchaseGoals(categories, campaignID);
        }

        void RefreshTasks(TA_SupportTicketCategory[] categories, int campaignID)
        {
            foreach (var payout in categories)
            {
                if (!payout.category.Equals("Task")) continue;

                foreach (var category in payout.events)
                {
                    var item = Instantiate(prefab, tasksContainer);
                    item.SetID(category.id);
                    item.SetCampaignID(campaignID);
                    item.SetName(category.eventName);
                    item.SetPoints(category.payoutAmountConverted);
                    item.SetCompletion(category.isTicketSubmitted);
                    item.AllowSelection();

                    item.OnSelected += Selected;
                    item.OnDeselected += DeSelected;

                    tickets.Add(item);
                }
            }
        }

        void RefreshPurchaseGoals(TA_SupportTicketCategory[] categories, int campaignID)
        {
            foreach (var payout in categories)
            {
                if (!payout.category.Equals("Microcharge")) continue;

                foreach (var category in payout.events)
                {
                    var item = Instantiate(prefab, purchaseContainer);
                    item.SetID(category.id);
                    item.SetCampaignID(campaignID);
                    item.SetName(category.eventName);
                    item.SetPoints(category.payoutAmountConverted);
                    item.SetCompletion(category.isTicketSubmitted);
                    item.AllowSelection();

                    item.OnSelected += Selected;
                    item.OnDeselected += DeSelected;

                    purchaseTickets.Add(item);
                }
            }
        }

        void ClearTickets()
        {
            Clear(tickets, tasksContainer);
            Clear(purchaseTickets, purchaseContainer);
        }

        void Clear(List<TA_TicketItem> list, Transform container)
        {
            if (list.Count == 0)
                list = container.GetComponentsInChildren<TA_TicketItem>().ToList();

            foreach (var item in list)
            {
                item.OnSelected -= Selected;
                item.OnDeselected -= DeSelected;
                Destroy(item.gameObject);
            }

            list.Clear();
        }

        void Selected(TA_TicketItem item)
        {
            if (submitted.Count > 0 && onlyOneTicketCanBeSubmitted) return;
            if (submitted.Contains(item)) return;

            submitButton.interactable = true;
            submitted.Add(item);
            DisableSelection();
            item.AllowSelection();
        }

        void DeSelected(TA_TicketItem item)
        {
            if (!submitted.Contains(item)) return;
            submitted.Remove(item);
            AllowSelection();

            if (submitted.Count <= 0)
                submitButton.interactable = false;
        }

        void DisableSelection()
        {
            foreach (var ticket in tickets)
                ticket.DisableSelection();
            foreach (var ticket in purchaseTickets)
                ticket.DisableSelection();
        }

        void AllowSelection()
        {
            foreach (var ticket in tickets)
                ticket.AllowSelection();
            foreach (var ticket in purchaseTickets)
                ticket.AllowSelection();
        }
    }
}