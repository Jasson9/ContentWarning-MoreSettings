using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;
using UnityEngine;

namespace MoreSettings
{
    public class AntiAliasingSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            Camera[] cameras = (Camera[])Resources.FindObjectsOfTypeAll(typeof(Camera));
            switch (base.Value)
            {
                case 0:
                    obj.msaaSampleCount = 1;
                    obj.GetRenderer(0).supportedRenderingFeatures.msaa = false;
                    QualitySettings.antiAliasing = 0;
                    break;
                case 1:
                    obj.msaaSampleCount = 8;
                    obj.GetRenderer(0).supportedRenderingFeatures.msaa = true;
                    QualitySettings.antiAliasing = 8;
                    break;
            }
            foreach (var camera in cameras)
            {
                switch (base.Value) 
                {
                    case 0:
                        camera.allowMSAA = false;
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.None;
                        break;
                    case 1:
                        camera.allowMSAA = true;
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                        camera.GetUniversalAdditionalCameraData().antialiasingQuality = AntialiasingQuality.High;
                        break;
                }
            }
        }

        protected override int GetDefaultValue()
        {
            return 0;
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
            return "Anti Aliasing";
        }
    }
}
