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
    public class PostProcessingSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            UnityEngine.Object cameraObj = Resources.FindObjectsOfTypeAll(typeof(Camera)).FirstOrDefault();
            Camera camera = cameraObj as Camera;
            switch (base.Value)
            {
                case 0:
                    camera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
                    break;
                case 1:
                    camera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
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
            return "Post processing";
        }
    }
}
