using UnityEditor;
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
            
            
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            
            
            EditorGUI.BeginChangeCheck();
            
            {
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
}