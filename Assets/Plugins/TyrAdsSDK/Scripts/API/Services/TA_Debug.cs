using System.Net.Http;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Services
{
    public static class TA_Debug
    {
        public static bool disableAllDebug;
        public static bool disableDownloadDebug;

        #region Colors

        static string GreenColor(string msg) => $"<color=#2CB388>{msg}</color>";
        static string WhiteColor(string msg) => $"<color=white>{msg}</color>";
        static string YellowColor(string msg) => $"<color=yellow>{msg}</color>";
        static string RedColor(string msg) => $"<color=#FF4828>{msg}</color>";

        #endregion

        static string Clickable(string url)
            => Regex.Replace(url, @"(https?://\S+)", "<a href=\"$1\">$1</a>");

        public static void LogBegin(this HttpRequestMessage self, string msg, string url)
        {
            if (disableAllDebug) return;
            Debug.Log($"{YellowColor(msg)} requested...\n{url}");
        }

        public static void LogResult(this HttpResponseMessage self, TA_Service service, string msg)
        {
            if (disableAllDebug) return;

            var name = service.name;
            var log = self.IsSuccessStatusCode
                ? $"{GreenColor(name)} received \n{msg}"
                : $"{RedColor(name)} error: \n{self.ReasonPhrase}";

            service.SetLog(log);
            Debug.Log(log);
        }
        public static void LogError(  TA_Service service, string msg)
        {
            if (disableAllDebug) return;

            var name = service.name;
            var log =
                $"{RedColor(name)} error: \n{msg}";

            service.SetLog(log);
            Debug.Log(log);
        }
        public static void DebugDownload(string gameTitle, string type, string url)
        {
            if (disableAllDebug) return;
            if (disableDownloadDebug) return;

            Debug.Log($"{WhiteColor(gameTitle)}{YellowColor(" downloading ")}{type}...\n\nUrl: {Clickable(url)}");
        }

        public static void DebugDownloadFail(string gameTitle, string type, string url, string errorType)
        {
            if (disableAllDebug) return;
            if (disableDownloadDebug) return;

            Debug.Log($"{WhiteColor(gameTitle)}{RedColor(" download fail ")}{type}!\n\nUrl: {Clickable(url)}\nError: {errorType}");
        }

        public static void DebugDownloadSuccess(string gameTitle, string type, string url)
        {
            if (disableAllDebug) return;
            if (disableDownloadDebug) return;

            Debug.Log($"{WhiteColor(gameTitle)}{GreenColor(" downloaded ")}{type}!\n\nUrl: {Clickable(url)}");
        }

        public static void DebugCache(string gameTitle, string type, string url)
        {
            if (disableAllDebug) return;
            if (disableDownloadDebug) return;

            Debug.Log($"{WhiteColor(gameTitle)}{GreenColor(" loaded from cache ")}{type}!\n\nUrl: {Clickable(url)}");
        }
    }
}