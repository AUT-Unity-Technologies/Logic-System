using System;
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

        private void OnSceneGUI()
        {
            //Entity t = target as Entity;
            
            //Handles.DrawWireCube(t.transform.position, Vector3.one);
            //Handles.ArrowHandleCap(0,t.transform.position,Quaternion.identity,100,Event.current.type);
            //GizmosExtensions.DrawArrowFancy(t.transform.position,Vector3.up);
        }

        [DrawGizmo(GizmoType.NonSelected| GizmoType.Selected | GizmoType.Pickable)]
        public static void BasicGizmo(Entity node, GizmoType type)
        {
            bool selected = (type & GizmoType.Selected) != 0;
            
            Gizmos.color = selected ? Color.green :  Color.green *0.5f;

            var position = node.transform.position;
            var right = node.transform.right;
            
            //Gizmos.DrawSphere(position,0.1f);



            float arrowSize = 0.2f;
            Gizmos.color = selected ? Color.blue :  Color.white;
            bool drawEntityIcon = true;
            foreach (var component in node.components)
            {
                var iconForComp = EditorGUIUtility.GetIconForObject(component);
                if (iconForComp != null)
                {
                    drawEntityIcon = false;
                }
                
                foreach (var outputRef in component.Outputs)
                {
                    Output o = outputRef.Get(component);
                    foreach (var target in o.targets)
                    {
                        var end_go = target.targetEntity.gameObject;
                        if (end_go != null && end_go != node.gameObject)
                        {
                            var end_pos = end_go.transform.position;
                            Vector3 direction = end_pos - position;
                            var mag = direction.magnitude;
                            Vector3 end = position + (direction.normalized * (mag - arrowSize));
                            
                            GizmosExtensions.DrawArrowFancyTwoPoints(position, end,arrowSize);
                            
                        }
                    }
                }
            }
            
            if(drawEntityIcon)
                Gizmos.DrawIcon(position,  IOConfig.kPackageRoot + "/Editor/EditorResources/hexagon.png", true);
            
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