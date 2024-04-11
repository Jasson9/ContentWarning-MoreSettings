using HarmonyLib;
using MoreSettings.Settings.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace MoreSetting
{
    internal class VoiceVolumePatch: IPatch
    {
        [HarmonyPatch(typeof(VoiceVolumeSetting), "GetMinMaxValue")]
        [HarmonyPostfix]
        static void PatchVoiceVolumeMinMax(ref float2 __result)
        {
            __result = new float2(0f, 2f);
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(VoiceVolumePatch));
        }
    }
}
