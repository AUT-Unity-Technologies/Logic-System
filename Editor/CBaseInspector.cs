﻿using System;
using System.Collections.Generic;
using System.Reflection;
using com.dpeter99.utils.editor.UnityEditor.Rendering;
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
            //sp.Clear();
            //outputs.Clear();
        }

        public override void OnInspectorGUI()
        {
            /*
            //base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();

            var name = serializedObject.FindProperty("_name");
            EditorGUILayout.PropertyField(name);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            tab = GUILayout.Toolbar (tab, new string[] {"Object", "Outputs"});
            switch (tab) {
                case 0:
                    DrawProperties();
                    break;
                case 1:
                    DrawOutputs();
                    break;
            }
            */
            
                        
            CBaseUI.Inspector.Draw(_serializedCBase,this);
            
        }

        private void DrawProperties()
        {
            /*
            EditorGUI.BeginChangeCheck();
            
            foreach (SerializedProperty s in sp)
                EditorGUILayout.PropertyField(s);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            */
        }
        
        private void DrawOutputs()
        {
            //foreach (SerializedProperty s in outputs)
            //    EditorGUILayout.PropertyField(s);
        }
    }


    public class CBaseUI
    {
        /// <summary>Enum flags to store what parts of the UI are open. This is a bit-filed and easily stored in a int.</summary>
        public enum Expandable
        {
            /// <summary> Projection</summary>
            Outputs = 1 << 0,
            
            Settings = 1 << 1,
        }
        
        static readonly ExpandedState<Expandable, CBase> k_ExpandedState = new (Expandable.Settings, "Logic-System");
        
        public static readonly CED.IDrawer ObjectSettings = CED.FoldoutGroup(
            //CameraUI.Styles.projectionSettingsHeaderContent,
            new GUIContent("Settings"),
            Expandable.Settings,
            k_ExpandedState,
            FoldoutOption.Indent,
            Drawer_Settings
        );
        
        public static readonly CED.IDrawer ObjectOutputs = CED.FoldoutGroup(
            //CameraUI.Styles.projectionSettingsHeaderContent,
            new GUIContent("Outputs"),
            Expandable.Outputs,
            k_ExpandedState,
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