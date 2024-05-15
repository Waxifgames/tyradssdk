using System;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_AdCurrency
    {
        public string name;
        public string symbol;
        public string adUnitName;
        public string adUnitCurrencyName;
        public string adUnitCurrencyIcon;
        public string adUnitCurrencyConversion;
    }

    [Serializable]
    public class TA_PayoutEvent
    {
        public int id;
        public string conversionStatus;
        public string identifier;
        public string eventName;
        public string eventDescription;
        public int eventOrder;
        public string eventCategory;
        public float payoutAmount;
        public float payoutAmountConverted;
        public int payoutTypeId;
        public string payoutType;
        public bool allowDuplicateEvents;
        public int maxTime;
        public string maxTimeMetric;
        public float maxTimeRemainSeconds;
        public bool enforceMaxTimeCompletion;
    }
}