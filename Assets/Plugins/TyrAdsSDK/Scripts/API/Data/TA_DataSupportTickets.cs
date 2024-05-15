using System;
using System.Collections.Generic;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_SupportTicketsResponse
    {
        public string message;
        public TA_SupportTicketCategory[] data;
    }

    [System.Serializable]
    public class TA_SupportTicketCategory
    {
        public string category;
        public TA_SupportTicket[] events;
    }

    [Serializable]
    public class TA_SupportTicketData
    {
        public List<TA_SupportTicketsList> list;
    }

    [Serializable]
    public class TA_SupportTicketsList
    {
        public int id;
        public string name;
        public List<TA_SupportTicket> events;
    }

    [Serializable]
    public class TA_SupportTicket
    {
        public int id;
        public string identifier;
        public string eventName;
        public string eventDescription;
        public string currencyName;
        public string currencyIcon;
        public int currencyConversion;
        public int payoutTypeId;
        public string payoutType;
        public double payoutAmount;
        public int payoutAmountConverted;
        public bool isTicketSubmitted;
    }

    [Serializable]
    public class TA_SubmitTicketResponse
    {
        public string message;
        public TA_SubmitTicketData data;
    }

    [Serializable]
    public class TA_SubmitTicketData
    {
        public bool isTicketSubmitted;
    }
}