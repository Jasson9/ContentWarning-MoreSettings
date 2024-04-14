using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Settings;
using Ookii.Dialogs.WinForms;
using System.Windows.Forms;
using System.Security.Cryptography;
using BepInEx;
using Zorro.Settings.DebugUI;
using TMPro;
using HarmonyLib;
using System.IO;
using Zorro.Core;
using MoreSettings.Settings.Type;

namespace MoreSettings.Settings
{
    public class RecordSaveSetting : StringSetting, IExposedSetting, IPatch
    {
        internal static TextMeshProUGUI titleComponent = null;

        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Graphics;
        }

        public string GetDisplayName()
        {
            return "Record Save Location: " + Value;
        }

        public override void ApplyValue()
        {
            if (titleComponent != null)
            {
                titleComponent.text = GetDisplayName();
            }
        }

        protected override string GetDefaultValue()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        internal static void OnButtonClicked()
        {
            var folderPrompt = new VistaFolderBrowserDialog();
            if (folderPrompt.ShowDialog() == DialogResult.OK && folderPrompt.SelectedPath.IsNullOrWhiteSpace() == false)
            {
                var setting = GameHandler.Instance.SettingsHandler.GetSetting<RecordSaveSetting>();
                setting.SetValue(folderPrompt.SelectedPath, GameHandler.Instance.SettingsHandler);
                GameHandler.Instance.SettingsHandler.RegisterPage();
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
            static void RecordLocationSettingUI(KeyCodeSettingUI __instance, Setting setting, ISettingHandler settingHandler)
            {
                if (setting is RecordSaveSetting keyCodeSetting)
                {
                    __instance.label.text = "Select Folder";
                    __instance.button.onClick.AddListener(RecordSaveSetting.OnButtonClicked);
                }
            }

            [HarmonyPatch(typeof(SettingsCell), "Setup")]
            [HarmonyPostfix]
            static void GetSettingCellTitle(SettingsCell __instance, Setting setting) =>
                titleComponent = (setting is RecordSaveSetting) ? __instance.title :   titleComponent;

            // this function is a bit weird but it's designed this way so it works without interfering other similiar mod
            [HarmonyPatch(typeof(CameraRecording), "SaveToDesktop")]
            [HarmonyPostfix]
            static void PatchSaveRecording(CameraRecording __instance, out string videoFileName)
            {
                if (!RecordingsHandler.TryGetRecordingPath(__instance.videoHandle, out var path))
                {
                    videoFileName = string.Empty;
                    return;
                }

                videoFileName = "content_warning_" + __instance.videoHandle.id.ToShortString() + ".webm";
                string oldPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName);
                string newDestFolder = GameHandler.Instance.SettingsHandler.GetSetting<RecordSaveSetting>().Value;
                string newPath = Path.Combine(newDestFolder, videoFileName);
                if (oldPath.Equals(newPath)) return; // indicate the user didn't set the custom location at setting thus we wont move/copy it
                try
                {
                    if (Directory.Exists(newDestFolder) == false) Directory.CreateDirectory(newDestFolder);
                    if (File.Exists(oldPath)) File.Move(oldPath, newPath);
                    else File.Copy(path, newPath); // if other mod already remove the one from desktop
                    Debug.Log("Video saved successfully to " + newPath + " [MoreSettings]");
                    return;
                }
                catch (Exception e)
                {
                    if (oldPath.Equals(newPath) == false) File.Copy(path, oldPath); // if user set custom path but failed to save, save to the desktop instead
                    Debug.LogException(e);
                    Debug.Log("Error encountered when saving to custom directory, video saved to desktop directory instead [MoreSettings]");
                }
                return;
            }
        }
    }

}
