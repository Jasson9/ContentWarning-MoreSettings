using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zorro.Settings;
using UnityEngine;

namespace MoreSettings.Settings
{
    public class PostProcessingSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            UniversalAdditionalCameraData[] camerasData = (UniversalAdditionalCameraData[])Resources.FindObjectsOfTypeAll(typeof(UniversalAdditionalCameraData));
            foreach (var cameraData in camerasData)
            {
                switch (base.Value)
                {
                    case 0:
                        cameraData.renderPostProcessing = false;
                        break;
                    case 1:
                        cameraData.renderPostProcessing = true;
                        break;
                }
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
            return "Post processing (off = Brightness won't work)";
        }
    }
}
