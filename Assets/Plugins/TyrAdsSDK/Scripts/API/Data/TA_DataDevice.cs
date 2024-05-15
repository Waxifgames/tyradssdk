using System;

// ReSharper disable InconsistentNaming

namespace Plugins.TyrAdsSDK.Scripts.API.Data
{
    [Serializable]
    public class TA_StoreUserDeviceResponse
    {
        public string message;
        public TA_StoreUserDeviceData data;
    }

    [Serializable]
    public class TA_StoreUserDeviceData
    {
        public bool isNewDevice;
    }

    [Serializable]
    public class TA_DeviceData
    {
        public string device;
        public string brand;
        public string model;
  
        public string manufacturer;
        public string product;
  
        public string host;
        
        public string hardware;
        public string serial_number;
        public string android_id;
        public string device_age;
    
        public string display;
        public string height_inches;
        public string width_inches; 
        public string height_px; 
        public string width_px; 
        public string xdpi; 
        public string ydpi; 
  
        public string baseOs;
        public string codename;
        public string type;
        public string tags;
  
        public string build;
        public string build_sign;

        public string version;
        public string sdk_version;
        public string release_version;
        
        public string package;
        public string installer_store;
  
        public string lang;
        public string os_lang;
  
        public bool rooted;
        public bool @virtual;
    }
}