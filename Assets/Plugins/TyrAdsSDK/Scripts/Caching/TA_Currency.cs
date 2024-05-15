using System;
using UnityEngine;

namespace Plugins.TyrAdsSDK.Scripts.Caching
{
    public class TA_TPointData
    {
        public double amount;
        public string suffix;
        public string format;
    }

    public static class TA_Currency
    {
        public static Texture2D Icon { get; private set; }
        public static string Name { get; private set; }
        public static void SetIcon(Texture2D texture) => Icon = texture;
        public static void SetName(string name) => Name = name;

        public static TA_TPointData ConvertTPoints(double amount)
        {
            var points = new TA_TPointData
            {
                amount = amount,
                suffix = "",
                format = "0.0"
            };

            switch (points.amount)
            {
                case >= 1000000:
                    points.amount /= 1000000;
                    points.suffix = "M";
                    break;
                case >= 1000:
                    points.amount /= 1000;
                    points.suffix = "K";
                    break;
                default:
                    points.format = "0";
                    break;
            }

            if (IsIntegerWithZero(points.amount))
                points.format = "0";

            return points;
        }

        static bool IsIntegerWithZero(double value)
            => value == 0
               || Math.Abs(value - Math.Truncate(value)) < 0.01f
               && value % 1 == 0;
    }
}