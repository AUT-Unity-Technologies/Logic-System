using System;
using System.Collections.Generic;
using System.Reflection;
using com.dpeter99.utils.Editor.InspectorExtensions.AreaHelpers;
using UnityEditor.Rendering;
//using Cinemachine.Editor;
using UnityEditor;
//using UnityEditor.ShaderGraph.Drawing.Inspector;
using UnityEngine;

namespace LogicSystem.Editor
{
    using CED = CoreEditorDrawer<SerializedCBase>;
    
    [CustomEditor(typeof(CBase),true)]
    public class CBaseInspector : UnityEditor.Editor
    {
        private SerializedCBase _serializedCBase;
        
        
        private void OnEnable()
        {
            _serializedCBase = new SerializedCBase(serializedObject);
        }

        private void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            CBaseUI.Inspector.Draw(_serializedCBase,this);
        }

    }


    public class CBaseUI
    {
        /// <summary>Enum flags to store what parts of the UI are open. This is a bit-filed and easily stored in a int.</summary>
        [Flags]
        public enum Expandable
        {
            /// <summary> Projection</summary>
            Outputs = 1 << 0,
            
            Settings = 1 << 1,
        }
        
        static readonly ExpandedState<Expandable, CBase, SerializedCBase> k_ExpandedState = new (Expandable.Settings, "Logic-System");

        private static readonly ExpandedStateFromProperty<Expandable, SerializedCBase> _ExpandedState = new(
            accessor: data => data.foldState.GetEnumValue<Expandable>(),
            setter: (data, expandable) =>
            {
                data.foldState.SetEnumValue(expandable);
                data.foldState.serializedObject.ApplyModifiedProperties();
            });
        
        public static readonly CED.IDrawer ObjectSettings = CED.FoldoutGroup(
            //CameraUI.Styles.projectionSettingsHeaderContent,
            new GUIContent("Settings"),
            Expandable.Settings,
            _ExpandedState,
            FoldoutOption.Indent,
            Drawer_Settings
        );
        
        public static readonly CED.IDrawer ObjectOutputs = CED.FoldoutGroup(
            //CameraUI.Styles.projectionSettingsHeaderContent,
            new GUIContent("Outputs"),
            Expandable.Outputs,
            _ExpandedState,
            FoldoutOption.Indent,
            Drawer_Outputs
        );
        
        public static readonly CED.IDrawer[] Inspector = new[]
        {
            ObjectSettings,
            ObjectOutputs
        };


        static void Drawer_Settings(SerializedCBase p, UnityEditor.Editor owner)
        {
            EditorGUI.BeginChangeCheck();

            {
                var nameRect = new RectArea(EditorGUILayout.GetControlRect(true));

                var nameprop = nameRect.GetHorizontalArea(nameRect.free.width - 20);

                EditorGUI.PropertyField(nameprop,p.name,new GUIContent("Component Name"));

                if (GUI.Button(nameRect, new GUIContent("Reset")))
                {
                    p.name.stringValue = p.serializedObject.targetObject.GetType().Name;
                }
            }
            
            
            foreach (SerializedProperty s in p.props)
                EditorGUILayout.PropertyField(s);

            if (EditorGUI.EndChangeCheck())
            {
                p.serializedObject.ApplyModifiedProperties();
            }
        }
        
        static void Drawer_Outputs(SerializedCBase p, UnityEditor.Editor owner)
        {
            foreach (SerializedProperty s in p.outputs)
                EditorGUILayout.PropertyField(s);
        }
        
    }
}