using System;
using UnityEngine;
using static System.Globalization.NumberStyles;

namespace Plugins.TyrAdsSDK.Scripts.Caching
{
    public static class TA_Extensions
    {
        public static T Please<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }

        public static Color HexToColor(string hexColor)
        {
            if (hexColor.StartsWith("#")) hexColor = hexColor[1..];
            var r = int.Parse(hexColor.Substring(0, 2), HexNumber) / 255f;
            var g = int.Parse(hexColor.Substring(2, 2), HexNumber) / 255f;
            var b = int.Parse(hexColor.Substring(4, 2), HexNumber) / 255f;
            return new Color(r, g, b);
        }

        public static string ConvertToString(int seconds)
        {
            var days = seconds / (3600 * 24);
            var hours = seconds % (3600 * 24) / 3600;
            var minutes = seconds % (3600 * 24) % 3600 / 60;
            return $"{days:D2}d {hours:D2}h {minutes:D2}m";
        }

        public static int ConvertDaysToSeconds(int days)
        {
            return days * 24 * 60 * 60;
        }

    
    }
}