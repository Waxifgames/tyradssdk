using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GetDeviceData : MonoBehaviour
{
    public TextMeshProUGUI txt;

    void Start()
    {
        Refresh();
    }

    string None => "<color=red>none</color>";
    string NotSure => " <color=yellow>(not sure)</color>";
    string Space = "";

    string msg;

    // public int GetSDKLevel() {
    //     var clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
    //     var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
    //     var sdkLevel = AndroidJNI.GetStaticIntField(clazz, fieldID);
    //     return sdkLevel;
    // }
    void Refresh()
    {
        msg = string.Empty;

        // string BASE_OS = AndroidVersion.BASE_OS;
        // string CODENAME = AndroidVersion.CODENAME;
        // string INCREMENTAL = AndroidVersion.INCREMENTAL;
        // string RELEASE = AndroidVersion.RELEASE;
        // string SDK = AndroidVersion.SDK;
        // int SDK_INT = AndroidVersion.SDK_INT;
        // string SECURITY_PATCH = AndroidVersion.SECURITY_PATCH;
        // string ALL_VERSION = AndroidVersion.ALL_VERSION;


        Join(Space);
        Join("device", SystemInfo.deviceName);
        Join("brand", GetFirstWord(SystemInfo.deviceModel));
        Join("model", GetLastWord(SystemInfo.deviceModel));
        Join("manufacturer", GetFirstWord(SystemInfo.deviceModel));
        Join("product", None);
        Join("host", GetPublicIPv4() + NotSure);
        Join("hardware", UnityEngine.Android.AndroidDevice.hardwareType.ToString());
        Join("serialNumber", Serial());

        Join(Space);
        Join("androidId (md5)", SystemInfo.deviceUniqueIdentifier);
        Join("deviceAge", None);

        Join(Space);
        Join("display", GetDisplayModel());
        Join("heightInches", None);
        Join("widthInches", None);
        Join("heightPx", Screen.height.ToString());
        Join("widthPx", Screen.width.ToString());
        Join("xdpi", None);
        Join("ydpi", None);

        Join(Space);
        Join("baseOs", SystemInfo.operatingSystem);
        Join("codename", SystemInfo.operatingSystemFamily.ToString());
        Join("type", None);
        Join("tags", None);

        Join(Space);
        Join("build", Application.version);
        Join("buildSign", BuildSignature());
        Join("version", None); //PlayerSettings.Android.targetSdkVersion.ToString()

        Join("package", Application.identifier);
        Join("installerStore", GetInstallerPackageName());

        Join(Space);
        Join("lang", Application.systemLanguage.ToString());
        Join("osLang", GetLanguageAbbreviation());
        Join("rooted", IsDeviceRooted() + NotSure);
        Join("virtual", IsDeviceVirtual() + NotSure);

        txt.text = msg;
    }

    int GetAndroidTargetSDKAPI()
    {
        try
        {
            // Access Android's Build.VERSION_CODES class to get the target SDK API level
            var buildVersionCodesClass = new AndroidJavaClass("android.os.Build$VERSION_CODES");
            var targetSDKAPI = buildVersionCodesClass.GetStatic<int>("LOLLIPOP_MR1"); // Adjust according to your target SDK version

            return targetSDKAPI;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving Android target SDK API: " + e);
            return -1;
        }
    }
    #region Device Data
    string GetLastWord(string input)
    {
        var words = input.Split(' ');
        var lastWord = words[^1];
        return lastWord;
    }

    string GetFirstWord(string input)
    {
        var spaceIndex = input.IndexOf(' ');
        return spaceIndex >= 0 ? input[..spaceIndex] : input;
    }

    public static string GetPublicIPv4()
    {
        var ipString = new WebClient().DownloadString("http://icanhazip.com");
        return ipString.Replace("\\r\\n", "").Replace("\\n", "").Trim();
    }

    string IsDeviceRooted()
    {
        string[] rootIndicators = { "/system/app/Superuser.apk", "/system/xbin/su", "/system/bin/su", "/sbin/su", "/system/su", "/system/bin/.ext", "/system/app/SuperSU", "/system/app/Kinguser" };
        return rootIndicators.Any(System.IO.File.Exists) ? "true" : "false";
    }

    string IsDeviceVirtual()
    {
        string[] virtualIndicators = { "generic", "unknown", "google_sdk", "emulator", "sdk", "vbox", "goldfish" };
        var model = SystemInfo.deviceModel.ToLower();
        var product = SystemInfo.deviceModel.ToLower();
        var manufacturer = SystemInfo.deviceModel.ToLower();
        return virtualIndicators.Any(indicator => model.Contains(indicator) || product.Contains(indicator) || manufacturer.Contains(indicator)) ? "true" : "false";
    }
    #endregion
    int GetAndroidSDKVersion()
    {
        try
        {
            // Accessing Android's Build class to get the SDK version
            var buildClass = new AndroidJavaClass("android.os.Build");
            var release = buildClass.GetStatic<string>("VERSION.RELEASE");

            // Convert the release string to an integer (e.g., "8" from "8.0.0")
            var sdkVersion = int.Parse(release);

            return sdkVersion;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving Android SDK version: " + e);
            return -1; // Return a default value or handle the error as needed
        }
    }

    string Serial()
    {
        return "unknown";
        return None;
        var jo = new AndroidJavaObject("android.os.Build$VERSION");
        var serial = jo.GetStatic<string>("SERIAL");
        return serial;
    }

    void Join(string header, string info = "")
    {
        if (header.Equals("")) msg += "\n\n";
        else
            msg += $"\n<color=white>{header}:</color> {info}";
    }

  

    string BuildSignature()
    {
        return None;
        var contextClass = new AndroidJavaClass("android.content.Context");
        var context = contextClass.CallStatic<AndroidJavaObject>("getApplicationContext");

        var packageManager = context.Call<AndroidJavaObject>("getPackageManager");
        var packageName = context.Call<string>("getPackageName");
        var flags = 0x00000040; // PackageManager.GET_SIGNATURES
        var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, flags);

        var signatures = packageInfo.Get<AndroidJavaObject>("signatures");
        var signature = signatures.Call<AndroidJavaObject>("get", 0);
        var signatureBytes = signature.Call<byte[]>("toByteArray");
        var buildSignature = System.Convert.ToBase64String(signatureBytes);

        return buildSignature;
    }

    string Build()
    {
        return Application.version;
        return None;
        var buildClass = new AndroidJavaClass("android.os.Build");
        var buildVersion = buildClass.GetStatic<string>("VERSION.RELEASE");
        return buildVersion;
    }


    string GetInstallerPackageName()
    {
        return Application.installerName;
        return None;
        try
        {
            var contextClass = new AndroidJavaClass("android.content.Context");
            var context = contextClass.CallStatic<AndroidJavaObject>("getApplicationContext");
            var packageName = context.Call<string>("getPackageName");

            var packageManager = context.Call<AndroidJavaObject>("getPackageManager");
            var packageInfo = packageManager.Call<AndroidJavaObject>("getInstallerPackageName", packageName);

            return packageInfo != null ? packageInfo.Call<string>("toString") : "Unknown";
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving installer package name: " + e);
            return "Unknown";
        }
    }

    string GetDisplayModel()
    {
        return None;
        try
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var windowManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "window");
            var display = windowManager.Call<AndroidJavaObject>("getDefaultDisplay");
            var displayModel = display.Call<string>("getName");
            return displayModel;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving display model: " + e);
            return "Unknown";
        }
    }

    string GetLanguageAbbreviation()
    {
        // Get the system language enum
        var language = Application.systemLanguage;

        // Convert the enum value to its corresponding abbreviation
        var languageAbbreviation = "";

        switch (language)
        {
            case SystemLanguage.Afrikaans:
                languageAbbreviation = "af";
                break;
            case SystemLanguage.Arabic:
                languageAbbreviation = "ar";
                break;
            case SystemLanguage.Basque:
                languageAbbreviation = "eu";
                break;
            case SystemLanguage.Belarusian:
                languageAbbreviation = "be";
                break;
            case SystemLanguage.Bulgarian:
                languageAbbreviation = "bg";
                break;
            case SystemLanguage.Catalan:
                languageAbbreviation = "ca";
                break;
            case SystemLanguage.Chinese:
                languageAbbreviation = "zh";
                break;
            case SystemLanguage.Czech:
                languageAbbreviation = "cs";
                break;
            case SystemLanguage.Danish:
                languageAbbreviation = "da";
                break;
            case SystemLanguage.Dutch:
                languageAbbreviation = "nl";
                break;
            case SystemLanguage.English:
                languageAbbreviation = "en";
                break;
            case SystemLanguage.Estonian:
                languageAbbreviation = "et";
                break;
            case SystemLanguage.Faroese:
                languageAbbreviation = "fo";
                break;
            case SystemLanguage.Finnish:
                languageAbbreviation = "fi";
                break;
            case SystemLanguage.French:
                languageAbbreviation = "fr";
                break;
            case SystemLanguage.German:
                languageAbbreviation = "de";
                break;
            case SystemLanguage.Greek:
                languageAbbreviation = "el";
                break;
            case SystemLanguage.Hebrew:
                languageAbbreviation = "he";
                break;
            case SystemLanguage.Icelandic:
                languageAbbreviation = "is";
                break;
            case SystemLanguage.Indonesian:
                languageAbbreviation = "id";
                break;
            case SystemLanguage.Italian:
                languageAbbreviation = "it";
                break;
            case SystemLanguage.Japanese:
                languageAbbreviation = "ja";
                break;
            case SystemLanguage.Korean:
                languageAbbreviation = "ko";
                break;
            case SystemLanguage.Latvian:
                languageAbbreviation = "lv";
                break;
            case SystemLanguage.Lithuanian:
                languageAbbreviation = "lt";
                break;
            case SystemLanguage.Norwegian:
                languageAbbreviation = "no";
                break;
            case SystemLanguage.Polish:
                languageAbbreviation = "pl";
                break;
            case SystemLanguage.Portuguese:
                languageAbbreviation = "pt";
                break;
            case SystemLanguage.Romanian:
                languageAbbreviation = "ro";
                break;
            case SystemLanguage.Russian:
                languageAbbreviation = "ru";
                break;
            case SystemLanguage.SerboCroatian:
                languageAbbreviation = "hr";
                break;
            case SystemLanguage.Slovak:
                languageAbbreviation = "sk";
                break;
            case SystemLanguage.Slovenian:
                languageAbbreviation = "sl";
                break;
            case SystemLanguage.Spanish:
                languageAbbreviation = "es";
                break;
            case SystemLanguage.Swedish:
                languageAbbreviation = "sv";
                break;
            case SystemLanguage.Thai:
                languageAbbreviation = "th";
                break;
            case SystemLanguage.Turkish:
                languageAbbreviation = "tr";
                break;
            case SystemLanguage.Ukrainian:
                languageAbbreviation = "uk";
                break;
            case SystemLanguage.Unknown:
                languageAbbreviation = "";
                break;
            case SystemLanguage.Vietnamese:
                languageAbbreviation = "vi";
                break;
        }

        return languageAbbreviation;
    }
}

public class AndroidVersion
{
    static AndroidJavaClass versionInfo;
    static AndroidVersion() => versionInfo = new AndroidJavaClass("android.os.Build$VERSION");
    public static string BASE_OS => versionInfo.GetStatic<string>("BASE_OS");
    public static string CODENAME => versionInfo.GetStatic<string>("CODENAME");
    public static string INCREMENTAL => versionInfo.GetStatic<string>("INCREMENTAL");
    public static int PREVIEW_SDK_INT => versionInfo.GetStatic<int>("PREVIEW_SDK_INT");
    public static string RELEASE => versionInfo.GetStatic<string>("RELEASE");
    public static string SDK => versionInfo.GetStatic<string>("SDK");
    public static int SDK_INT => versionInfo.GetStatic<int>("SDK_INT");
    public static string SECURITY_PATCH => versionInfo.GetStatic<string>("SECURITY_PATCH");

    public static string ALL_VERSION
    {
        get
        {
            var version = "BASE_OS: " + BASE_OS + "\n";
            version += "CODENAME: " + CODENAME + "\n";
            version += "INCREMENTAL: " + INCREMENTAL + "\n";
            version += "PREVIEW_SDK_INT: " + PREVIEW_SDK_INT + "\n";
            version += "RELEASE: " + RELEASE + "\n";
            version += "SDK: " + SDK + "\n";
            version += "SDK_INT: " + SDK_INT + "\n";
            version += "SECURITY_PATCH: " + SECURITY_PATCH;

            return version;
        }
    }
}