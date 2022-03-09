using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor
{
    [CustomEditor(typeof(Entity))]
    public class EntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = this.target as Entity;
            
            EditorGUILayout.LabelField("Guid:", t.GetGuid().ToString());
        }
    }
}