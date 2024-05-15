using System.Linq;
using System.Net;
using Plugins.TyrAdsSDK.Scripts.API.Data;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.API.Extra
{
    public static class TA_Android
    {
        public static TA_DeviceData GetDeviceData()
        {
            TA_DeviceData device = new()
            {
                device = SystemInfo.deviceName,
                brand = GetFirstWord(SystemInfo.deviceModel),
                model = GetLastWord(SystemInfo.deviceModel),

                manufacturer = GetFirstWord(SystemInfo.deviceModel),
                product = "",

                host = GetPublicIPv4(),

                hardware = UnityEngine.Android.AndroidDevice.hardwareType.ToString(),
                serial_number = "unknown",
                android_id = SystemInfo.deviceUniqueIdentifier,
                device_age = "",

                display = "",
                height_inches = "",
                width_inches = "",
                height_px = Screen.height.ToString(),
                width_px = Screen.width.ToString(),
                xdpi = "",
                ydpi = "",

                baseOs = SystemInfo.operatingSystem,
                codename = SystemInfo.operatingSystemFamily.ToString(),
                type = "",
                tags = "",

                build = Application.version,
                build_sign = "unknown",

                version = "",
                sdk_version = GetSDKVersion(),
                release_version = GetReleaseVersion(),

                package = Application.identifier,
                installer_store = Application.installerName,

                lang = Application.systemLanguage.ToString(),
                os_lang = GetLanguageAbbreviation(),
                rooted = IsDeviceRooted(),
                @virtual = IsDeviceVirtual()
            };
            return device;
        }

        static string GetSDKVersion()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using var buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
                return buildVersion.GetStatic<int>("SDK_INT").ToString();
            }

            return "Not running on an Android device.";
        }

        static string GetReleaseVersion()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using var buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
                return buildVersion.GetStatic<string>("RELEASE");
            }

            return "Not running on an Android device.";
        }

        static string GetLanguageAbbreviation() =>
            Application.systemLanguage switch
            {
                SystemLanguage.Afrikaans => "af",
                SystemLanguage.Arabic => "ar",
                SystemLanguage.Basque => "eu",
                SystemLanguage.Belarusian => "be",
                SystemLanguage.Bulgarian => "bg",
                SystemLanguage.Catalan => "ca",
                SystemLanguage.Chinese => "zh",
                SystemLanguage.Czech => "cs",
                SystemLanguage.Danish => "da",
                SystemLanguage.Dutch => "nl",
                SystemLanguage.English => "en",
                SystemLanguage.Estonian => "et",
                SystemLanguage.Faroese => "fo",
                SystemLanguage.Finnish => "fi",
                SystemLanguage.French => "fr",
                SystemLanguage.German => "de",
                SystemLanguage.Greek => "el",
                SystemLanguage.Hebrew => "he",
                SystemLanguage.Icelandic => "is",
                SystemLanguage.Indonesian => "id",
                SystemLanguage.Italian => "it",
                SystemLanguage.Japanese => "ja",
                SystemLanguage.Korean => "ko",
                SystemLanguage.Latvian => "lv",
                SystemLanguage.Lithuanian => "lt",
                SystemLanguage.Norwegian => "no",
                SystemLanguage.Polish => "pl",
                SystemLanguage.Portuguese => "pt",
                SystemLanguage.Romanian => "ro",
                SystemLanguage.Russian => "ru",
                SystemLanguage.SerboCroatian => "hr",
                SystemLanguage.Slovak => "sk",
                SystemLanguage.Slovenian => "sl",
                SystemLanguage.Spanish => "es",
                SystemLanguage.Swedish => "sv",
                SystemLanguage.Thai => "th",
                SystemLanguage.Turkish => "tr",
                SystemLanguage.Ukrainian => "uk",
                SystemLanguage.Unknown => "",
                SystemLanguage.Vietnamese => "vi",
                _ => ""
            };

        static string GetLastWord(string input)
        {
            var words = input.Split(' ');
            var lastWord = words[^1];
            return lastWord;
        }

        static string GetFirstWord(string input)
        {
            var spaceIndex = input.IndexOf(' ');
            return spaceIndex >= 0 ? input[..spaceIndex] : input;
        }

        static string GetPublicIPv4()
        {
            var ipString = new WebClient().DownloadString("http://icanhazip.com");
            return ipString.Replace("\\r\\n", "").Replace("\\n", "").Trim();
        }

        static bool IsDeviceRooted()
        {
            string[] rootIndicators = { "/system/app/Superuser.apk", "/system/xbin/su", "/system/bin/su", "/sbin/su", "/system/su", "/system/bin/.ext", "/system/app/SuperSU", "/system/app/Kinguser" };
            return rootIndicators.Any(System.IO.File.Exists);
        }

        static bool IsDeviceVirtual()
        {
            string[] virtualIndicators = { "generic", "unknown", "google_sdk", "emulator", "sdk", "vbox", "goldfish" };
            var model = SystemInfo.deviceModel.ToLower();
            var product = SystemInfo.deviceModel.ToLower();
            var manufacturer = SystemInfo.deviceModel.ToLower();
            return virtualIndicators.Any(indicator => model.Contains(indicator) || product.Contains(indicator) || manufacturer.Contains(indicator));
        }
    }
}