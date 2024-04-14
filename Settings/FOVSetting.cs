using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;
using MoreSettings.Settings.Type;
using HarmonyLib;
using UnityEngine;

namespace MoreSettings.Settings
{
    public class FOVSetting : FloatSetting, IExposedSetting, IPatch
    {
        static Traverse<float> baseFOVTraverse = null;

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "FOV (Field Of View)";
        }

        protected override float GetDefaultValue()
        {
            return 70.08072f;
        }

        protected override float2 GetMinMaxValue()
        {
            return new float2(60f, 100f);
        }

        public override void ApplyValue()
        {
            if (baseFOVTraverse != null)
            {
                baseFOVTraverse.Value = Value;
            }
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        internal class Patch
        {
            [HarmonyPatch(typeof(MainCamera),"Awake")]
            [HarmonyPostfix]
            static void PatchFOV()
            {
                baseFOVTraverse = Traverse.Create(MainCamera.instance).Field<float>("baseFOV");
                Debug.Log("traverse " + baseFOVTraverse);
                try
                {
                    // unknown error at start, ignore it
                    if (baseFOVTraverse?.Value != null) baseFOVTraverse.Value = GameHandler.Instance.SettingsHandler.GetSetting<FOVSetting>().Value;
                }
                catch (Exception e) { }
            }
        }
    }
}
