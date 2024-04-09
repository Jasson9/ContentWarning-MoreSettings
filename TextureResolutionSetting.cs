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
    public class TextureResolutionSetting : EnumSetting, IExposedSetting
    {
        public override void ApplyValue()
        {
            QualitySettings.globalTextureMipmapLimit = 3 - base.Value;
        }

        protected override int GetDefaultValue()
        {
            return 3;
        }

        public override List<string> GetChoices()
        {
            return new List<string> { "Low", "Medium", "High", "Very High" };
        }

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Texture Resolution";
        }
    }
}
