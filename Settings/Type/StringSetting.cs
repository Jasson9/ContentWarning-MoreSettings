using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core;
using Zorro.Settings;
using Zorro.Settings.DebugUI;

namespace MoreSettings.Settings.Type
{
    public abstract class StringSetting : Setting
    {
        public string Value { get; protected set; }

        public override SettingUI GetDebugUI(ISettingHandler settingHandler)
        {
            return new SettingUI();// not implemented for now
        }

        public override void Load(ISettingsSaveLoad loader)
        {
            if(loader.TryGetString(GetType(), out var value))
            {
                Value = value;
            }
            else
            {
                Debug.LogWarning("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
                Value = GetDefaultValue();
            }
        }

        protected abstract string GetDefaultValue();

        public override GameObject GetSettingUICell()
        {
            return SingletonAsset<InputCellMapper>.Instance.KeyCodeSettingCell;
        }

        public override void Save(ISettingsSaveLoad saver)
        {
            saver.SaveString(GetType(), Value);
        }

        public void SetValue(string value, ISettingHandler handler)
        {
            Value = value;
            ApplyValue();
            handler.SaveSetting(this);
        }
    }
}
