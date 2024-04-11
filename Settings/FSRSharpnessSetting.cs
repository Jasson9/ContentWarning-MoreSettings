using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;

namespace MoreSettings
{
    public class FSRSharpnessSetting : FloatSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "FSR Sharpness Amount";
        }

        protected override float GetDefaultValue()
        {
            return 0.92f;
        }

        protected override float2 GetMinMaxValue()
        {
            return new float2(0f, 1f);
        }

        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            obj.fsrSharpness = base.Value;
        }
    }
}
