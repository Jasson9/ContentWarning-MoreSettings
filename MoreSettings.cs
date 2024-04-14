using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Settings;
using Setting = Zorro.Settings.Setting;
using MoreSettings.Settings.Type;
using MoreSettings.Settings;

namespace MoreSettings
{
    [BepInPlugin("MoreSettings","More Settings","0.2.0")]
    public class MoreSettings : BaseUnityPlugin 
    {
        private Harmony harmony;
        static internal List<Setting> additionalSettings = new List<Setting>();
        static internal List<IPatch> patches = new List<IPatch>();
        private void Awake()
        {
            Logger.LogInfo("MoreSettings Loaded!!!");

            addSetting(new Settings.HDRSetting());
            addSetting(new Settings.RenderScaleSetting());
            addSetting(new Settings.FSRToggleSetting());
            addSetting(new Settings.FSRSharpnessSetting());
            addSetting(new Settings.TextureResolutionSetting());
            addSetting(new Settings.AntiAliasingSetting());
            addSetting(new Settings.PostProcessingSetting());
            addSetting(new Settings.FOVSetting());
            addSetting(new Settings.RecordSaveSetting());
            addSetting(new Settings.CrouchingModeSetting());
            addSetting(new Settings.UseItemKeybindSetting());
            addSetting(new Settings.AimKeybindSetting());
            addSetting(new ResetGraphicsToDefault());
            addSetting(new ResetAudioToDefault());
            addSetting(new ResetControlsToDefault());

            addPatches(new ShadowQualityPatch());
            addPatches(new VoiceVolumePatch());
            addPatches(new SFXVolumePatch());
            addPatches(new MasterVolumePatch());
            addPatches(new KeybindPatch());
            //addPatches(new ResetToDefault());

            harmony = new Harmony("MoreSettings");
            harmony.PatchAll(typeof(MainPatch));
            ApplyPatches();
        }

        internal void ApplyPatches()
        {   
            foreach (var setting in additionalSettings)
            {
                if(setting is IPatch)
                {
                    var t = setting as IPatch;
                    t.ApplyPatch(ref harmony);
                }
            }

            foreach (var patch in patches)
            {
                patch.ApplyPatch(ref harmony);
            }
        }

        public static void addSetting(Setting setting)
        {
            additionalSettings.Add(setting);
        }

        internal static void addPatches(IPatch patch)
        {
            patches.Add(patch);
        }
    }

    internal class MainPatch
    {

        [HarmonyPatch(typeof(SettingsHandler),MethodType.Constructor)]
        [HarmonyPostfix]
        static void PatchSettingsHandler(SettingsHandler __instance)
        {
            var settings = Traverse.Create(__instance).Field("settings").GetValue() as List<Setting>;
            var settingsSaveLoad = Traverse.Create(__instance).Field("_settingsSaveLoad").GetValue() as ISettingsSaveLoad;
            settings = settings.Concat(MoreSettings.additionalSettings).ToList();
            Traverse.Create(__instance).Field("settings").SetValue(settings);
            foreach (Setting setting in MoreSettings.additionalSettings)
            {
                setting.Load(settingsSaveLoad);
                setting.ApplyValue();
            }
            Debug.Log("Settings Patch Applied [MoreSettings]");
        }

        [HarmonyPatch(typeof(Player),"Start")]
        [HarmonyPostfix]
        static void ApplySettingAtStart(Player __instance)
        {
            if(__instance.IsLocal == true)
            {
                Tools.ApplySettings();
            }
        }
    }
}
