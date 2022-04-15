using System;
using System.Collections.Generic;
using Cinemachine.Editor;
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
            
            var list = GetList(targets);
            
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

            var list = GetList(targets);
            
            var line = position;
            line.height = LINE_HEIGHT;

            EditorGUI.BeginProperty(position, GUIContent.none, property);
            
            property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, property.name);
            line.y += LINE_HEIGHT;

            EditorGUI.BeginChangeCheck();
            
            if (property.isExpanded)
            {
                //var list = GetList(targets);
                line.height = position.height-line.height;
                
                list.DoList(line, new GUIContent("Targets"));
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        public ReorderableList GetList(SerializedProperty targets)
        {
            if (list == null)
            {
                list = new ReorderableList(targets, true, true, true);
                /*list.onAddCallback += reorderableList =>
                {
                    Debug.Log("asd");
                    targets.InsertArrayElementAtIndex(targets.arraySize);
                };*/
            }

            return list;
        }
        
    }
    
    
    [CustomPropertyDrawer(typeof(Binding))]
    public class BindingEditor : PropertyDrawer
    {
        private static Binding def = new();
        
        //protected new Binding propertyObject { get { return (Binding)base.propertyObject; } }
        private SerializedProperty stringField = null;

        protected void Initialize(SerializedProperty prop)
        {
            //base.Initialize(prop);

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
            //base.OnGUI(position,property,label);
            
            var targetEnt = property.FindPropertyRelative("targetEntity");
            
            var guid = GuidReferenceHelpers.GetGuidFromProperty(targetEnt);

            Entity? entity = null;
            if (guid != Guid.Empty)
            {
                var go = EntityManager.ResolveGuid(guid);
                entity = go.GetComponent<Entity>();
            }

            var target = property.FindPropertyRelative(() => def.target);
            var input = property.FindPropertyRelative(() => def.input);
            
            
            var pos = new Rect(position);
            pos.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(pos, targetEnt,GUIContent.none);
            
            pos.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var componentRect = new Rect(pos);
            componentRect.width = pos.width / 2;

            GUIHelpers.DoBasicPreview<string>(componentRect, target);

            var targetRect = new Rect(pos);
            targetRect.x += pos.width / 2;
            targetRect.width = pos.width / 2;
            
            if (GUI.Button(targetRect,input.stringValue))
            {
                var data = ScriptableObject.CreateInstance<SearchTreeContextTest>();
                //var data = new SearchTreeContextTest();
                
                Debug.Log(entity?.name);
                if (entity is not null)
                {
                    data.Init(entity, s =>
                    {
                        target.stringValue = s.Split("#")[0];
                        input.stringValue = s.Split("#")[1];

                        target.serializedObject.ApplyModifiedProperties();
                    });
                    data.hideFlags = HideFlags.HideAndDontSave;

                    SearchWindow.Open(new SearchWindowContext(EditorGUIUtility.GUIToScreenPoint(new Vector2(pos.xMax, pos.yMax + pos.height))), data);
                }
            }

        }
    }
    
}