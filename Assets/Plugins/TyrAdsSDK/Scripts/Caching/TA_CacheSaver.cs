using System.Collections.Generic;
using System.IO;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.Caching
{
    [AddComponentMenu("")]
    public class TA_CacheSaver : MonoBehaviour
    {
        [SerializeField] Vector2 iconResolution;
        [SerializeField] Vector2 previewResolution;

        [Space(20)]
        [SerializeField] [TA_ReadOnly] string iconSize;
        [SerializeField] [TA_ReadOnly] string previewSize;
        [SerializeField] [TA_ReadOnly] string myAssumingsInTotal;

        [Space(20)]
        [SerializeField] TA_Cache cache;
        [SerializeField] [TA_ReadOnly] List<Texture2D> loaded = new();
        bool _useCache;

        const string PREFIX = "";
        const string SUFFIX = ".jpg";
        const string ICON = "icon";
        const string TPOINTS_ICON = "tIcon";
        const string PREVIEW = "preview";
        const string SAVE = "TA_Cache_SaveKey";
        public bool HasIcon(string appID) => _useCache && cache.HasIcon(appID);
        public bool HasPreview(string creativeName) => _useCache && cache.HasPreview(creativeName);

        const float SIZE_PER_100_PIXELS_PREVIEW = 4.5f;
        const float SIZE_PER_100_PIXELS_ICON = 2.794f;

        void OnValidate()
        {
            var y = (int)(previewResolution.x / 1.923f);
            previewResolution = new Vector2(previewResolution.x, y);
            iconResolution = new Vector2(iconResolution.x, iconResolution.x);

            var sizeKB = previewResolution.x / 100f * SIZE_PER_100_PIXELS_PREVIEW;
            var totalSize = sizeKB * 3 / 10f;
            previewSize = sizeKB.ToString(("0.0")) + " KB";

            var sizeKB_icon = iconResolution.x / 100f * SIZE_PER_100_PIXELS_ICON;
            iconSize = sizeKB_icon.ToString(("0.0")) + " KB";

            myAssumingsInTotal = totalSize.ToString(("0.0")) + " MB";
        }

        public void Init(bool useCache)
        {
            _useCache = useCache;
            if (useCache) Load();
        }

        void Save()
            => PlayerPrefs.SetString(SAVE, JsonUtility.ToJson(cache));

        void Load() =>
            cache = PlayerPrefs.HasKey(SAVE)
                ? JsonUtility.FromJson<TA_Cache>(PlayerPrefs.GetString(SAVE, string.Empty))
                : new TA_Cache();

        public bool Has(int campaignID) => cache.HasCampaign(campaignID);

        public void Add(TA_DetailCampaign detail)
        {
            if (!Has(detail.campaignId))
                cache.AddCampaign(detail);
            Save();
        }

        public TA_DetailCampaign Get(int campaignID)
            => cache.GetCampaign(campaignID);

        public void SaveIcon(string appName, string id, Texture2D texture)
        {
            var file = FileName(ICON, id, appName);

            var filePath = CreateFile(file, texture, ICON);
            cache.SaveIcon(id, filePath);
            Save();
        }

        public void SavePreview(string id, Texture2D texture)
        {
            var file = FileName(PREVIEW, id);
            var filePath = CreateFile(file, texture, PREVIEW);
            cache.SavePreview(id, filePath);
            Save();
        }

        public Texture2D GetIcon(string id)
        {
            var filePath = cache.GetIconPath(id);
            return ReadTexture(filePath, (int)iconResolution.x, (int)iconResolution.y);
        }

        public Texture2D GetPreview(string id)
        {
            var filePath = cache.GetPreviewPath(id);
            return ReadTexture(filePath, (int)previewResolution.x, (int)previewResolution.y);
        }

        string FileName(string type, string id, string appName = "")
            => $"{PREFIX}{id}_{type}{SUFFIX}";

        string CreateFile(string file, Texture2D texture, string type)
        {
            var targetWidth = type == ICON ? iconResolution.x : type == PREVIEW ? previewResolution.x : 50;
            var targetHeight = type == ICON ? iconResolution.y : type == PREVIEW ? previewResolution.y : 50;
            var resizedTexture = ResizeTexture(texture, (int)targetWidth, (int)targetHeight);
            var bytes = resizedTexture.EncodeToJPG();
            var path = Path.Combine(Application.persistentDataPath, file);

            File.WriteAllBytes(path, bytes);
            return path;
        }

        Texture2D ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
        {
            var rt = new RenderTexture(targetWidth, targetHeight, 24);
            RenderTexture.active = rt;
            Graphics.Blit(sourceTexture, rt);

            var resizedTexture = new Texture2D(targetWidth, targetHeight);
            resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = null;
            Destroy(rt);

            return resizedTexture;
        }

        Texture2D ReadTexture(string filePath, int width, int height)
        {
            var imageData = File.ReadAllBytes(filePath);
            var texture = new Texture2D(width, height);
            texture.LoadImage(imageData);
            loaded.Add(texture);
            return texture;
        }
    }
}