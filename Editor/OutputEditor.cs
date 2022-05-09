﻿using System;
using com.dpeter99.utils.Editor.InspectorExtensions.AreaHelpers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using ReorderableList = Malee.List.ReorderableList;

namespace LogicSystem.Editor
{
    [CustomPropertyDrawer(typeof(Output), true)]
    public class OutputEditor : PropertyDrawer
    {
        public static float LINE_HEIGHT = EditorGUIUtility.singleLineHeight;
        
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
            //height += EditorGUIUtility.singleLineHeight;
            //height += list.HeaderHeight + list.FooterHeight;
            //height += LINE_HEIGHT * (targets.arraySize+2);

            height += list.GetHeight();
            
            return (property.isExpanded ? height : LINE_HEIGHT) + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targets = property.FindPropertyRelative("targets");

            targets.isExpanded = property.isExpanded;
            
            var list = GetList(targets);

            var line = position;
            //line.height = LINE_HEIGHT;
            
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            
            //property.isExpanded = EditorGUI.Foldout(line, property.isExpanded, property.name);
            //line.y += LINE_HEIGHT;

            
            
            EditorGUI.BeginChangeCheck();
            
            //if (property.isExpanded)
            {
                //var list = GetList(targets);
                line.height = position.height;
                
                list.DoList(position, new GUIContent(property.name,IOConfig.ArrowOut));
                
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            property.isExpanded = targets.isExpanded;
            
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
            var targetEnt = property.FindPropertyRelative("targetEntity");
            
            var guid = GuidReferenceHelpers.GetGuidFromProperty(targetEnt);
            
            Entity? entity = null;
            if (guid != Guid.Empty)
            {
                var go = EntityManager.ResolveGuid(guid);
                entity = go.GetComponent<Entity>();
            }

            var target = property.FindPropertyRelative("target");
            var input = property.FindPropertyRelative("input");
            
            
            
            var area = new RectArea(position);
            
            {
                var line = area.GetNextLine();
                
                EditorGUI.PropertyField(line, targetEnt, new GUIContent(IOConfig.Hexagon));
            }
            
            {
                var pos = area.GetNextLine();

                pos.AddLabelPrefix(new GUIContent(IOConfig.ArrowOut));
                
                {
                    var comp_name_area = pos.GetHorizontalArea(pos.free.width / 2);

                    var componentTarget = entity.components.Find(c => c.Name == target.stringValue);

                    if (GUIHelpers.DoBasicPreview(comp_name_area, componentTarget.Name, componentTarget))
                    {
                        EditorGUIUtility.PingObject(componentTarget);
                    }
                }
                
                {
                    var inputButtonArea = pos.GetHorizontalArea(pos.free.width);
                    var serachWindoePos = inputButtonArea.free;
                    
                    if (GUI.Button(inputButtonArea, input.stringValue))
                    {
                        if (entity is not null)
                        {
                            var data = ScriptableObject.CreateInstance<SearchTreeContextTest>();
                            data.hideFlags = HideFlags.HideAndDontSave;
                            
                            data.Init(entity, s =>
                            {
                                target.stringValue = s.Split("#")[0];
                                input.stringValue = s.Split("#")[1];

                                target.serializedObject.ApplyModifiedProperties();
                            });

                            SearchWindow.Open(
                                new SearchWindowContext(EditorGUIUtility.GUIToScreenPoint(new Vector2(serachWindoePos.xMax, serachWindoePos.yMax + serachWindoePos.height))),
                                data
                                );
                        }
                    }
                }
            }
            
        }
        
        
    }
}