using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor.Settings
{
// Create a new type of Settings Asset.

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class MyCustomSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("LogicSystemSettings", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "Logic system",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = LogicSystemSettingsHelper.GetSerializedSettings();


                    var defaultPlayer = settings.FindProperty("m_DefaultPlayerEntity");

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        
                        EditorGUILayout.PropertyField(defaultPlayer);
                        
                        if (check.changed)
                        {
                            settings.ApplyModifiedProperties();
                        }
                    }
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Number", "Some String" })
            };

            return provider;
        }
    }
}