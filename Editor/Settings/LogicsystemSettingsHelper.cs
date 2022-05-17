using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor.Settings
{
   
    static class LogicSystemSettingsHelper
    {
        internal static LogicSystemSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<LogicSystemSettings>(LogicSystemSettings.k_MyCustomSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<LogicSystemSettings>();
                AssetDatabase.CreateAsset(settings, LogicSystemSettings.k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}