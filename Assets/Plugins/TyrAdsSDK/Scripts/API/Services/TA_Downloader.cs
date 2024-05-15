using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using Plugins.TyrAdsSDK.Scripts.Caching;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEngine;
using UnityEngine.Networking;
using static Plugins.TyrAdsSDK.Scripts.API.Services.TA_Debug;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    public enum DownloadMode
    {
        Offers,
        ActiveOffers,
        OfferDetail
    }

    [AddComponentMenu("")]
    public class TA_Downloader : MonoBehaviour
    {
        [TA_ReadOnly] public TA_CacheSaver _cache;
        bool _useCache;
        DownloadMode _mode;

        public void Init(TA_CacheSaver cache, bool useCache)
        {
            _useCache = useCache;
            _cache = cache;
        }

        public void DownloadIcons(IReadOnlyList<TA_Campaign> data, List<TA_OfferItem> list)
        {
            for (var i = 0; i < data.Count; i++)
                DownloadIcon(data[i], list[i]);
        }

        public void DownloadPreviews(IReadOnlyList<TA_Campaign> data, List<TA_OfferItem> list)
        {
            for (var i = 0; i < data.Count; i++)
                DownloadPreview(data[i], list[i]);
        }




        readonly List<IEnumerator> coroutineSequence = new List<IEnumerator>();
 
         bool isSequenceRunning = false;

        IEnumerator ExecuteCoroutineSequence()
        {
            isSequenceRunning = true;

            while(coroutineSequence.Count > 0) 
            {

                var c = coroutineSequence[0];
                yield return StartCoroutine(c);
                coroutineSequence.RemoveAt(0);
                yield return null;
            }
 
            isSequenceRunning = false;
        }

      

        public void DownloadIcon(TA_Campaign data, TA_OfferItem offer)
        {
            StartDownload(LoadIcon(data, offer));
            //StartCoroutine(LoadIcon(data, offer));
        }
        void StartDownload(IEnumerator corutine)
        {
            coroutineSequence.Add((corutine));
            if (!isSequenceRunning)
                StartCoroutine(ExecuteCoroutineSequence());
        }
        public void DownloadPreview(TA_Campaign data, TA_OfferItem offer)
        {
            StartDownload(LoadPreview(data, offer));
           // StartCoroutine(LoadPreview(data, offer));
        }

        public Texture2D GetCachedIcon(string packageName) => _cache.GetIcon(packageName);
        public Texture2D GetCachedPreview(string packageName) => _cache.GetPreview(packageName);

        public bool CachedIcon(string packageName) => _useCache && _cache.HasIcon(packageName);
        public bool CachedPreview(string packageName) => _useCache && _cache.HasPreview(packageName);

        public void StopAllDownloads()
        {
            // Debug.LogError("STOPED");
            // StopAllCoroutines();
        }

        public void SetMode(DownloadMode mode)
        {
            _mode = mode;
        }

        IEnumerator LoadIcon(TA_Campaign data, TA_OfferItem offer)
        {
            var app = data.app;
            var packageName = app.packageName;
            var url = app.thumbnail;
            DebugDownload(app.title, "icon", url);

            if (CachedIcon(packageName))
            {
                var texture = GetCachedIcon(packageName);
                offer.SetIcon(texture);
                DebugCache(app.title, "icon", url);
            }
            else
            {
                using var iconURL = UnityWebRequestTexture.GetTexture(url, false);
                yield return iconURL.SendWebRequest();
                if (iconURL.result == UnityWebRequest.Result.Success)
                {
                    var texture = ((DownloadHandlerTexture)iconURL.downloadHandler).texture;
                    offer.SetIcon(texture);
                    DebugDownloadSuccess(data.app.title, "icon", url);

                    if (_useCache)
                    {
                        _cache.SaveIcon(app.title, packageName, texture);
                    }
                }
                else
                {
                    DebugDownloadFail(data.app.title, "icon", url, iconURL.error);
                }
            }
        }

        IEnumerator LoadPreview(TA_Campaign data, TA_OfferItem offer)
        {
            var packs = data.creative.creativePacks;
            if (packs.Length <= 0)
            {
                offer.SetPreview(null);
                yield break;
            }

            var creatives = packs[0].creatives;
            if (creatives.Length <= 0)
            {
                offer.SetPreview(null);
                yield break;
            }

            var creative = creatives[0];
            var cName = data.app.packageName;
            var url = creative.fileUrl;
            DebugDownload(data.app.title, "preview", url);

            if (_useCache && _cache.HasPreview(cName))
            {
                var texture = _cache.GetPreview(cName);
                offer.SetPreview(texture);
                DebugCache(data.app.title, "preview", url);
            }
            else
            {
                using var iconURL = UnityWebRequestTexture.GetTexture(url, false);
                yield return iconURL.SendWebRequest();

                if (iconURL.result == UnityWebRequest.Result.Success)
                {
                    // if (startedFor != _mode) yield break;

                    var texture = ((DownloadHandlerTexture)iconURL.downloadHandler).texture;
                    offer.SetPreview(texture);
                    DebugDownloadSuccess(data.app.title, "preview", url);

                    if (_useCache && texture)
                    {
                        _cache.SavePreview(cName, texture);
                    }
                }
                else
                {
                    offer.SetPreview(null);
                    DebugDownloadFail(data.app.title, "preview", url, iconURL.error);
                }
            }
        }

        public Coroutine DownloadTPointsIcon(string url)
            => StartCoroutine(LoadTPointsIcon(url));

        IEnumerator LoadTPointsIcon(string url)
        {
            var title = "TPOINTS";
            DebugDownload(title, "icon", url);

            using var iconURL = UnityWebRequestTexture.GetTexture(url, false);
            yield return iconURL.SendWebRequest();
            if (iconURL.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)iconURL.downloadHandler).texture;
                TA_Currency.SetIcon(texture);
                DebugDownloadSuccess(title, "icon", url);
            }
            else
            {
                DebugDownloadFail(title, "icon", url, iconURL.error);
            }
        }
    
    }
}