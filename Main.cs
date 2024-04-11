using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using MoreSettings;
using Zorro.Settings;
using UnityEngine.Rendering;
using Unity.Mathematics;
using UnityEngine.UI;
using System.IO;
using Zorro.Core;
using Setting = Zorro.Settings.Setting;
using MoreSettings.Settings.Type;
using MoreSetting;

namespace MoreSettings
{
    [BepInPlugin("MoreSettings","More Settings","0.1.0")]
    public class Plugin : BaseUnityPlugin 
    {
        private Harmony harmony;
        static internal List<Setting> additionalSettings = new List<Setting>();
        static internal List<IPatch> patches = new List<IPatch>();
        private void Awake()
        {
            Logger.LogInfo("MoreSettings Loaded!!!");

            addSetting(new RenderScaleSetting());
            addSetting(new FSRToggleSetting());
            addSetting(new FSRSharpnessSetting());
            addSetting(new TextureResolutionSetting());
            addSetting(new AntiAliasingSetting());
            addSetting(new PostProcessingSetting());
            addSetting(new FOVSetting());
            addSetting(new RecordSaveSetting());
            addSetting(new CrouchingModeSetting());

            addPatches(new ShadowQualityPatch());
            addPatches(new VoiceVolumePatch());
            addPatches(new SFXVolumePatch());
            addPatches(new MasterVolumePatch());

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

    public class MainPatch
    {

        [HarmonyPatch(typeof(SettingsHandler),MethodType.Constructor)]
        [HarmonyPostfix]
        static void PatchSettingsHandler(SettingsHandler __instance)
        {
            var settings = Traverse.Create(__instance).Field("settings").GetValue() as List<Setting>;
            var settingsSaveLoad = Traverse.Create(__instance).Field("_settingsSaveLoad").GetValue() as ISettingsSaveLoad;
            settings = settings.Concat(Plugin.additionalSettings).ToList();
            Traverse.Create(__instance).Field("settings").SetValue(settings);
            foreach (Setting setting in Plugin.additionalSettings)
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
