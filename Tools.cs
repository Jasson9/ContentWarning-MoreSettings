using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreSettings
{
    public class Tools
    {
        public static void LogMessage(string message)
        {
            FileLog.Log(message);
            Debug.Log(message);
        }

    }
}
