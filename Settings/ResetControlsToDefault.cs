using HarmonyLib;
using MoreSettings.Settings.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zorro.Settings;
using Unity;
using UnityEngine;
using Object = UnityEngine.Object;
using TMPro;
using Ookii.Dialogs.WinForms;
using System.Windows.Forms;
using Zorro.Core;
using System.Reflection;
using Zorro.UI;

namespace MoreSettings.Settings
{
    internal class ResetControlsToDefault : StringSetting, IExposedSetting, IPatch
    {
        internal static TextMeshProUGUI titleComponent = null;
        internal static UIPageHandler pageHandler = null;
        internal static SettingsMenu menu = null;
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Controls;
        }

        public string GetDisplayName()
        {
            return "Reset Controls To Default";
        }

        public override void ApplyValue()
        {
        }

        protected override string GetDefaultValue()
        {
            return "";
        }

        internal static void OnButtonClicked()
        {
            var settings = GameHandler.Instance.SettingsHandler.GetSettings(SettingCategory.Controls);
            foreach (Setting setting in settings)
            {

                if(setting is ResolutionSetting screenres)
                {
                    var res = screenres.GetResolutions().FirstOrDefault();
                    Debug.Log("Default Value " + res.width);
                    screenres.SetValue(res, GameHandler.Instance.SettingsHandler);
                    screenres.Update();
                }

                if(setting is IntSetting intset)
                {
                    MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    var res = dynMethod.Invoke(setting, new object[] { });
                    Debug.Log("Default Value " +(int)res);
                    intset.SetValue((int)res, GameHandler.Instance.SettingsHandler);
                    intset.Update();
                }

                if (setting is FloatSetting floatset)
                {
                    MethodInfo dynMethod = setting.GetType().GetMethod("GetDefaultValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    var res = dynMethod.Invoke(setting, new object[] { });
                    Debug.Log("Default Value " + (float)res);
                    floatset.SetValue((float)res, GameHandler.Instance.SettingsHandler);
                    floatset.Update();
                }
            }

            GameHandler.Instance.SettingsHandler.Update();
            GameHandler.Instance.SettingsHandler.RegisterPage();
            if(pageHandler != null)
            {
                pageHandler.TransistionToPage<MainMenuMainPage>();
                pageHandler.TransistionToPage<MainMenuSettingsPage>();
                menu.SelectCategory(SettingCategory.Controls);
            }
        }

        public void ApplyPatch(ref Harmony harmony)
        {
            harmony.PatchAll(typeof(Patch));
        }

        internal class Patch
        {
            [HarmonyPatch(typeof(KeyCodeSettingUI), "Setup")]
            [HarmonyPostfix]
            static void ResetAudioToDefaultSettingUI(KeyCodeSettingUI __instance, Setting setting, ISettingHandler settingHandler)
            {
                if (setting is ResetControlsToDefault keyCodeSetting)
                {
                    __instance.label.text = "Reset";
                    __instance.button.onClick.AddListener(ResetControlsToDefault.OnButtonClicked);
                }
            }

            [HarmonyPatch(typeof(SettingsCell), "Setup")]
            [HarmonyPostfix]
            static void GetSettingCellTitle(SettingsCell __instance, Setting setting) =>
                titleComponent = (setting is ResetControlsToDefault) ? __instance.title : titleComponent;

            [HarmonyPatch(typeof(MainMenuMainPage), "OnPageEnter")]
            [HarmonyPostfix]
            static void PatchGetSettingHandler(MainMenuMainPage __instance)=> pageHandler = __instance.GetPageHandler<UIPageHandler>();

            [HarmonyPatch(typeof(SettingCategoryTab), "Select")]
            [HarmonyPostfix]
            static void PatchCategory(SettingCategoryTab __instance)
            {
                menu = __instance.settingsMenu;
            }
        }
    }
}
