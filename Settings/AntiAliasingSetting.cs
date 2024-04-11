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
            Camera[] cameras = (Camera[])Resources.FindObjectsOfTypeAll(typeof(Camera));
            foreach (var camera in cameras)
            {
                switch (base.Value)
                {
                    case 0:
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.None;
                        break;
                    case 1:
                        camera.GetUniversalAdditionalCameraData().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                        camera.GetUniversalAdditionalCameraData().antialiasingQuality = AntialiasingQuality.High;
                        break;
                    case 2:
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
            return new List<string> { "OFF", "FXAA", "SMAA" };
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
