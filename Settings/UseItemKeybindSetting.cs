using HarmonyLib;
using MoreSettings.Settings.Type;
using UnityEngine;
using Zorro.Settings;

namespace MoreSettings.Settings
{
    internal class UseItemKeybindSetting : KeyCodeSetting, IExposedSetting, IPatch
    {
        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        public string GetDisplayName()
        {
            return "Use Item";
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        protected override KeyCode GetDefaultKey()
        {
            return KeyCode.Mouse0;
        }
    }
}
