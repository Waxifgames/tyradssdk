using Plugins.TyrAdsSDK.Scripts.API.Services;
using Plugins.TyrAdsSDK.Scripts.Caching;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.DATA
{
   [AddComponentMenu("")]
   public class TA_DATA : MonoBehaviour
   {
           
      [Space(10)]
      public TA_Downloader imageDownloader;
      public TA_PrefsSave save;
      public TA_CacheSaver cache;
   }
}
