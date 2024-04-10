using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Settings;

namespace MoreSettings
{
    public class Tools
    {
        internal static List<Setting> settings;

        public static void LogMessage(string message)
        {
            FileLog.Log(message);
            Debug.Log(message);
        }

        public static void ApplySettings()
        {
            foreach (var setting in settings)
            {
                setting.ApplyValue();
            }
        }
    }
}
