using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts
{
    public class AndroidSettings : MonoBehaviour
    {
        public bool NeverSleepScreen;
        public bool BackButtonQuit;
        public bool DisableDebugger;
        public bool force60Fps;

        void Start()
        {
            if (force60Fps) Application.targetFrameRate = 60;
            Init();
        }

        void Init()
        {
            if (NeverSleepScreen) Screen.sleepTimeout = SleepTimeout.NeverSleep;
            if (!BackButtonQuit) enabled = false;
            if (DisableDebugger)
            {
                Debug.Log("Debugger disabled for performance boost!");
                DisableDebug();
            }
            else
            {
                Debug.Log("Please, disable debugger for performance boost!");
                EnableDebug();
            }

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#endif
        }

        void EnableDebug()
        {
            Debug.unityLogger.logEnabled = true;
        }

        void DisableDebug()
        {
            Debug.unityLogger.logEnabled = false;
        }

        void Update()
        {
            if (Application.platform != RuntimePlatform.Android) return;
            if (!Input.GetKey(KeyCode.Escape)) return;
            Application.Quit();
        }
    }
}