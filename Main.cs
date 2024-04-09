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

namespace MoreSettings
{
    [BepInPlugin("MoreSettings","More Settings","0.0.1")]
    public class Plugin : BaseUnityPlugin 
    {
        private Harmony harmony;
        static internal List<Setting> additionalSettings = new List<Setting>();
        private void Awake()
        {
            Logger.LogInfo("MoreSettings Starting!!!");

            addSetting(new RenderScaleSetting());
            addSetting(new FSRToogleSetting());
            addSetting(new FSRSharpnessSetting());
            addSetting(new TextureResolutionSetting());
            //addSetting(new AntiAliasingSetting()); // not working currently
            //addSetting(new PostProcessingSetting()); // not working currently

            harmony = new Harmony("MoreSettings");
            harmony.PatchAll(typeof(GraphicsPatch));
        }

        public static void addSetting(Setting setting)
        {
            additionalSettings.Add(setting);
        }
    }

    public class GraphicsPatch
    {
        [HarmonyPatch(typeof(ShadowQualitySetting), "SetShadowSettings")]
        [HarmonyPrefix]
        static void PatchShadowQualityApply(ShadowQualitySetting __instance, ref UnityEngine.Rendering.Universal.ShadowResolution shadowResolution, ref float shadowDistance)
        {
            if(__instance.Value == 2)
            {
                shadowResolution = 0;
                shadowDistance = 0;
            }
        }

        [HarmonyPatch(typeof(ShadowQualitySetting), "GetChoices")]
        [HarmonyPostfix]
        static void PatchShadowQualityChoices(ref List<string> __result)
        {
            __result =  new List<string> { "High", "Low" , "Off"};
        }

        [HarmonyPatch(typeof(SettingsHandler),MethodType.Constructor)]
        [HarmonyPostfix]
        static public void PatchSettingsHandler(SettingsHandler __instance)
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
            Debug.Log("Applying Settings Patch [MoreSettings]");
        }
    }
}
