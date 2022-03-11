using System.Collections.Generic;
using LogicSystem.Editor.Util;
using Packages.ObjectPicker;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.Experimental.GraphView;
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
            height += EditorGUIUtility.singleLineHeight;
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
    public class BindingEditor : NestablePropertyDrawer
    {
        
        protected new Binding propertyObject { get { return (Binding)base.propertyObject; } }
        private SerializedProperty stringField = null;

        protected override void Initialize(SerializedProperty prop)
        {
            base.Initialize(prop);

            if (stringField == null)
                stringField = prop.FindPropertyRelative("field");
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //18*2 + 2
            return EditorGUIUtility.singleLineHeight*2 + EditorGUIUtility.standardVerticalSpacing + 8;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position,property,label);
            
            var targetEnt = property.FindPropertyRelative("targetEntity");
            //var targetEntRef = targetEnt.GetSerializedObject() as GuidReference;

            var pos = new Rect(position);
            pos.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(pos, targetEnt,GUIContent.none);
            
            pos.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
             

            if (GUI.Button(pos,"asd"))
            {
                //var data = ScriptableObject.CreateInstance<SearchTreeContextTest>();
                var data = new SearchTreeContextTest();
                data.Init(this.propertyObject.targetEntity.entity);
                //data.hideFlags = HideFlags.HideAndDontSave;
                
                SearchWindow.Open(new SearchWindowContext( EditorGUIUtility.GUIToScreenPoint(new Vector2(pos.xMax,pos.yMax))),data);
            }

        }
    }
    
}