using System.Collections.Generic;
using System.Linq;
using Plugins.TyrAdsSDK.Scripts.API.Data;

namespace Plugins.TyrAdsSDK.Scripts.Caching
{
    [System.Serializable]
    public class TA_Cache
    {
        public List<TA_DetailCampaign> campaignDetails = new();
        public List<CacheData> previews = new();
        public List<CacheData> icons = new();

        public bool HasCampaign(int id) => campaignDetails.Any(detail => detail.campaignId == id);
        public void AddCampaign(TA_DetailCampaign detail) => campaignDetails.Add(detail);

        public TA_DetailCampaign GetCampaign(int campaignID)
            => campaignDetails.Find(detail => detail.campaignId == campaignID);

        public bool HasIcon(string id) => Has(id, icons);
        public string GetIconPath(string id) => Get(id, icons);
        public CacheData SaveIcon(string id, string filePath) => Save(id, filePath, icons);

        public bool HasPreview(string id) => Has(id, previews);
        public string GetPreviewPath(string id) => Get(id, previews);
        public CacheData SavePreview(string id, string filePath) => Save(id, filePath, previews);

        public bool Has(string id, List<CacheData> list) => list.Any(data => data.id == id);
        public string Get(string id, List<CacheData> list) => list.Find(data => data.id == id).filePath;
        public CacheData Save(string id, string filePath, List<CacheData> list) => Create(id, filePath).Please(list.Add);

        CacheData Create(string id, string filePath) => new() { id = id, filePath = filePath };
    }

    [System.Serializable]
    public class CacheData
    {
        public string id;
        public string filePath;
    }
}