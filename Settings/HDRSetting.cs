using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace MoreSettings.Settings
{
    public class HDRSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            switch (base.Value)
            {
                case 0:
                    obj.supportsHDR = false;
                    break;
                case 1:
                    obj.supportsHDR = true;
                    break;
            }
        }

        protected override int GetDefaultValue()
        {
            return 1;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "OFF", "ON" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "High Dynamic Range (HDR)";
        }
    }
}
