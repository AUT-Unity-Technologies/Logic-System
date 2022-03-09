using System.Collections.Generic;
using Packages.ObjectPicker;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WaresoftEditor.Common;
using ReorderableList = Malee.List.ReorderableList;


namespace LogicSystem.Editor
{
    [CustomPropertyDrawer(typeof(Output), true)]
    public class OutputEditor : PropertyDrawer
    {
        public const float LINE_HEIGHT = 18f;
        
        //private static readonly Dictionary<int, ReorderableList> _lists = new ();
        private ReorderableList list;

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            
            var targets = property.FindPropertyRelative("targets");
            
            if (list == null)
            {
                list = new ReorderableList(targets, true, true, true);
            }
            
            //var list = GetList(targets);
            
            float height = 0;
            height += LINE_HEIGHT;
            //height += list.HeaderHeight + list.FooterHeight;
            //height += LINE_HEIGHT * (targets.arraySize+2);

            height += list.GetHeight();
            
            return property.isExpanded ? height : LINE_HEIGHT;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targets = property.FindPropertyRelative("targets");
            
            if (list == null)
            {
                list = new ReorderableList(targets, true, true, true);
            }
            
            var line = position;
            line.height = LINE_HEIGHT;

            EditorGUI.BeginProperty(position, GUIContent.none, property);
            
            property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, property.name);
            line.y += LINE_HEIGHT;

            
            
            if (property.isExpanded)
            {
                //var list = GetList(targets);
                line.height = position.height-line.height;
                
                list.DoList(line, new GUIContent("Targets"));
            }

            
            EditorGUI.EndProperty();
        }
        
    }
    
    
    [CustomPropertyDrawer(typeof(Binding))]
    public class BindingEditor : UnityEditor.PropertyDrawer
    {
        public const float LINE_HEIGHT = 18f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label)*2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetEnt = property.FindPropertyRelative("targetEntity");
            
            
            var pos = new Rect(position);
            pos.width = (position.width/3)*2;

            
            EditorGUI.PropertyField(pos, targetEnt,GUIContent.none);
            pos.AddXMin(pos.width);
            pos.width = position.width / 3;


            EditorGUI.BeginDisabledGroup(SceneObjectPicker.PropertyPicking == property);
            if (GUI.Button(pos,"o"))
            {
                SceneObjectPicker.Instance.StartPicking(targetEnt,typeof(Entity),"asd");
            }
            EditorGUI.EndDisabledGroup();
                

            //base.OnGUI(position, property, label);
            //GUILayout.Label("asd");
            
        }
    }
    
}