using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TA_BannerAndIconDownloader : MonoBehaviour
{
    [SerializeField] string[] bannerUrls;
    [SerializeField] string[] iconUrls;
    [SerializeField] float preloadTimeout = 5f;

    private Dictionary<string, Texture2D> bannerCache = new Dictionary<string, Texture2D>();
    private Dictionary<string, Texture2D> iconCache = new Dictionary<string, Texture2D>();

    void Start()
    {
        StartCoroutine(PreloadAssets());
    }

    private IEnumerator PreloadAssets()
    {
        List<IEnumerator> downloadJobs = new List<IEnumerator>();

        foreach (var url in bannerUrls)
        {
            if (!bannerCache.ContainsKey(url))
            {
                downloadJobs.Add(DownloadTexture(url, bannerCache));
            }
        }

        foreach (var url in iconUrls)
        {
            if (!iconCache.ContainsKey(url))
            {
                downloadJobs.Add(DownloadTexture(url, iconCache));
            }
        }

        yield return RunParallelDownloads(downloadJobs);
    }

    private IEnumerator DownloadTexture(string url, Dictionary<string, Texture2D> cache)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            uwr.timeout = (int)preloadTimeout;
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                cache[url] = texture;
            }
            else
            {
                Debug.LogError($"Failed to download texture from {url}: {uwr.error}");
            }
        }
    }

    private IEnumerator RunParallelDownloads(List<IEnumerator> downloadJobs)
    {
        List<Coroutine> coroutines = new List<Coroutine>();

        foreach (var job in downloadJobs)
        {
            coroutines.Add(StartCoroutine(job));
        }

        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }
}
