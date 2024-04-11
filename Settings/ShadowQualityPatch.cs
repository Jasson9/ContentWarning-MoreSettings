using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;
using MoreSettings.Settings.Type;
namespace MoreSettings
{
    internal class ShadowQualityPatch:IPatch
    {
        [HarmonyPatch(typeof(ShadowQualitySetting), "SetShadowSettings")]
        [HarmonyPrefix]
        static void PatchShadowQualityApply(ShadowQualitySetting __instance, ref UnityEngine.Rendering.Universal.ShadowResolution shadowResolution, ref float shadowDistance)
        {
            if (__instance.Value == 2)
            {
                shadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._256;
                shadowDistance = 30f;
            }
            else if (__instance.Value == 3)
            {
                shadowResolution = 0;
                shadowDistance = 0;
            }
        }

        [HarmonyPatch(typeof(ShadowQualitySetting), "SetShadowSettings")]
        [HarmonyPostfix]
        static void postPatchShadow(ShadowQualitySetting __instance, ref UnityEngine.Rendering.Universal.ShadowResolution shadowResolution, ref float shadowDistance)
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            // fix incorect shadow resolution implementation. High setting has lower resolution than low
            ShadowChanger.AdditionalLightShadowResolution = shadowResolution;
            ShadowChanger.MainLightShadowResolution = shadowResolution;
            Debug.Log("Shadow Resolution " + obj.mainLightShadowmapResolution + " [MoreSettings]");
        }

        [HarmonyPatch(typeof(ShadowQualitySetting), "GetChoices")]
        [HarmonyPostfix]
        static void PatchShadowQualityChoices(ref List<string> __result)
        {
            __result = new List<string> { "High", "Low", "Lowest", "Off" };
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(ShadowQualityPatch));
        }

        // for some reason unity doesn't makes setter at shadow related fields private so it has to be this way, props to JimmyCushnie for the code snippet
        public static class ShadowChanger
        {
            private static FieldInfo MainLightCastShadows_FieldInfo;
            private static FieldInfo AdditionalLightCastShadows_FieldInfo;
            private static FieldInfo MainLightShadowmapResolution_FieldInfo;
            private static FieldInfo AdditionalLightShadowmapResolution_FieldInfo;
            private static FieldInfo Cascade2Split_FieldInfo;
            private static FieldInfo Cascade4Split_FieldInfo;
            private static FieldInfo SoftShadowsEnabled_FieldInfo;

            static ShadowChanger()
            {
                var pipelineAssetType = typeof(UniversalRenderPipelineAsset);
                var flags = BindingFlags.Instance | BindingFlags.NonPublic;

                MainLightCastShadows_FieldInfo = pipelineAssetType.GetField("m_MainLightShadowsSupported", flags);
                AdditionalLightCastShadows_FieldInfo = pipelineAssetType.GetField("m_AdditionalLightShadowsSupported", flags);
                MainLightShadowmapResolution_FieldInfo = pipelineAssetType.GetField("m_MainLightShadowmapResolution", flags);
                AdditionalLightShadowmapResolution_FieldInfo = pipelineAssetType.GetField("m_AdditionalLightsShadowmapResolution", flags);
                Cascade2Split_FieldInfo = pipelineAssetType.GetField("m_Cascade2Split", flags);
                Cascade4Split_FieldInfo = pipelineAssetType.GetField("m_Cascade4Split", flags);
                SoftShadowsEnabled_FieldInfo = pipelineAssetType.GetField("m_SoftShadowsSupported", flags);
            }

            public static bool MainLightCastShadows
            {
                get => (bool)MainLightCastShadows_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => MainLightCastShadows_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static bool AdditionalLightCastShadows
            {
                get => (bool)AdditionalLightCastShadows_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => AdditionalLightCastShadows_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static ShadowResolution MainLightShadowResolution
            {
                get => (ShadowResolution)MainLightShadowmapResolution_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => MainLightShadowmapResolution_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static ShadowResolution AdditionalLightShadowResolution
            {
                get => (ShadowResolution)AdditionalLightShadowmapResolution_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => AdditionalLightShadowmapResolution_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static float Cascade2Split
            {
                get => (float)Cascade2Split_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => Cascade2Split_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static Vector3 Cascade4Split
            {
                get => (Vector3)Cascade4Split_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => Cascade4Split_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }

            public static bool SoftShadowsEnabled
            {
                get => (bool)SoftShadowsEnabled_FieldInfo.GetValue(GraphicsSettings.currentRenderPipeline);
                set => SoftShadowsEnabled_FieldInfo.SetValue(GraphicsSettings.currentRenderPipeline, value);
            }
        }
    }
}
