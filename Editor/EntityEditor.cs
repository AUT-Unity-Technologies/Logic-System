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
        
        [DrawGizmo(GizmoType.NonSelected| GizmoType.Selected | GizmoType.Pickable)]
        public static void BasicGizmo(Entity node, GizmoType type)
        {
            bool selected = (type & GizmoType.Selected) != 0;
            
            Gizmos.color = selected ? Color.green :  Color.green *0.5f;

            var position = node.transform.position;
            var right = node.transform.right;
            
            //Gizmos.DrawSphere(position,0.1f);
            Gizmos.DrawIcon(position,  IOConfig.kPackageRoot + "/Editor/EditorResources/hexagon.png", true);

            Gizmos.color = selected ? Color.blue :  Color.white;
            
            /*
            foreach (var link in node.graph)
            {
                if (link == null)
                    continue;
                
                if(Selection.objects.Contains(link.gameObject))
                    Gizmos.color = Color.blue;
                
                Gizmos.DrawLine(position , link.transform.position );

                if (!link.graph.Contains(node))
                {
                    Gizmos.DrawIcon(link.transform.position + Vector3.up,"Assets/Vulgus/Scripts/Crowd/Editor/warning.png");
                }
                
                Gizmos.color = selected ? Color.blue :  Color.white;
                
            }
            */
        }
    }
}