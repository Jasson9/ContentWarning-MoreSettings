using HarmonyLib;
using MoreSettings.Settings.Type;
using UnityEngine;
using static Player;

namespace MoreSettings.Settings
{
    public class KeybindPatch : IPatch
    {
        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(KeybindPatch));
        }

        [HarmonyPatch(typeof(PlayerInput), "SampeInput")]
        [HarmonyPostfix]
        static void UseItemPatch(PlayerInput __instance, PlayerData data)
        {
            if (!(data.inputOverideAmount > 0.99f))
            {
                if (data.cantUseItemFor <= 0f)
                {
                    KeyCode key1 = (KeyCode)GameHandler.Instance.SettingsHandler.GetSetting<UseItemKeybindSetting>().Value;
                    __instance.clickWasPressed = GlobalInputHandler.GetKeyDown(key1);
                    __instance.clickIsPressed = GlobalInputHandler.GetKey(key1);
                    __instance.clickWasReleased = GlobalInputHandler.GetKeyUp(key1);
                }
                KeyCode key2 = (KeyCode)GameHandler.Instance.SettingsHandler.GetSetting<AimKeybindSetting>().Value;
                __instance.aimWasPressed = GlobalInputHandler.GetKeyDown(key2);
                __instance.aimIsPressed = GlobalInputHandler.GetKey(key2);
            }
        }

    }
}
