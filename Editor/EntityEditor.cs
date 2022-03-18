using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : UnityEditor.Editor
    {
        private bool openComponents = false;
        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            var t = this.target as Entity;
            
            EditorGUILayout.LabelField("Guid:", t.GetGuid().ToString());

            //var prop = serializedObject.FindProperty("components");
            //prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, "Components");
            //if (prop.isExpanded)
            openComponents = EditorGUILayout.Foldout(openComponents, "Components");
            if (openComponents)
            {
                foreach (var component in t.components)
                {
                    var a = EditorGUIUtility.ObjectContent(component, typeof(CBase));
                    a.text = component.name;
                    
                    EditorGUILayout.LabelField(a);
                }    
            }
            
        }
    }
}