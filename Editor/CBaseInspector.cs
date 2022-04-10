using System;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine.Editor;
using UnityEditor;
using UnityEditor.ShaderGraph.Drawing.Inspector;
using UnityEngine;

namespace LogicSystem.Editor
{
    
    [CustomEditor(typeof(CBase),true)]
    public class CBaseInspector : UnityEditor.Editor
    {
        private int tab = 0;
        
        List<SerializedProperty> sp;

        private List<SerializedProperty> outputs;
        
        private void OnEnable()
        {
            var members = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            sp = new();
            outputs = new();
            
            foreach (var member in members)
            {
                var prop = serializedObject.FindProperty(member.Name);
                if (prop != null)
                {
                    if (member.FieldType != typeof(Output))
                    {
                        sp.Add(prop);
                    }
                    else
                    {
                        outputs.Add(prop);
                    }    
                }
                
            }
        }

        private void OnDisable()
        {
            sp.Clear();
            outputs.Clear();
        }

        public override void OnInspectorGUI()
        {
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
            
            
            
        }

        private void DrawProperties()
        {
            EditorGUI.BeginChangeCheck();
            
            foreach (SerializedProperty s in sp)
                EditorGUILayout.PropertyField(s);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        private void DrawOutputs()
        {
            foreach (SerializedProperty s in outputs)
                EditorGUILayout.PropertyField(s);
        }
    }
    
    
    
}