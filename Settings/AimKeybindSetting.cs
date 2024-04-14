using UnityEngine;
using Zorro.Settings;

namespace MoreSettings.Settings
{
    internal class AimKeybindSetting : KeyCodeSetting, IExposedSetting
    {

        public string GetDisplayName()
        {
            return "Aim";
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        protected override KeyCode GetDefaultKey()
        {
            return KeyCode.Mouse1;
        }
    }
}
