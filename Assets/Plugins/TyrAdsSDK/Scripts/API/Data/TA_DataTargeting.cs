using System;

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_Targeting
    {
        public string os;
        public string targetingType;
        public TA_Reward reward;
 
    }

    [Serializable]
    public class TA_Reward
    {
        public string rewardDifficulty;
        public string incentRewardDescription;
    }
}