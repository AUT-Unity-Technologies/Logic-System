using System;
using com.dpeter99.framework.Runtime;
using com.dpeter99.utils.Editor.InspectorExtensions.AreaHelpers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEngine;

namespace LogicSystem.Editor
{
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
            return EditorGUIUtility.singleLineHeight*2 + EditorGUIUtility.standardVerticalSpacing + 8;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = property.FindPropertyRelative("type");
            
            var targetEnt = property.FindPropertyRelative("targetEntity");
            
            var guid = GuidReferenceHelpers.GetGuidFromProperty(targetEnt);
            
            Entity? entity = null;
            if (guid != Guid.Empty)
            {
                var go = EntityManager.ResolveGuid(guid);
                entity = go.GetComponent<Entity>();
            }

            var target = property.FindPropertyRelative("targetComponent");
            var input = property.FindPropertyRelative("input");
            
            
            
            var area = new RectArea(position);
            
            {
                var typeSwitcharea = area.GetHorizontalArea(30).GetNextLine();

                var options = new GUIContent[]
                {
                    new ("Entity",IOConfig.Hexagon),
                    new ("System",IOConfig.CloseThick),
                    new ("Player",IOConfig.Human)
                };


                var buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.margin = new RectOffset(0, 0, 0, 0);
                buttonStyle.padding = new RectOffset(2, 0, 2, 2);
                buttonStyle.alignment = TextAnchor.MiddleLeft;
                buttonStyle.fixedWidth = 30;

                if (GUI.Button(typeSwitcharea, options[type.intValue].With(a=>a.text="") ,buttonStyle))
                {
                    
                    EditorUtility.DisplayCustomMenu(typeSwitcharea,options,type.intValue, (data, strings, selected) =>
                    {
                        var prop = data as SerializedProperty;

                        if (prop.intValue != selected)
                        {
                            prop.intValue = selected;
                            prop.serializedObject.ApplyModifiedProperties();
                        }
                        
                    },type);
                }
                
                
            }

            switch ((BindingType)(type.enumValueIndex))
            {
                case BindingType.Direct:
                    DrawDirectEntity();
                    break;
                case BindingType.System:
                    break;
                case BindingType.Player:
                    DrawPlayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            void DrawDirectEntity()
            {
                {
                    var line = area.GetNextLine();

                    EditorGUI.PropertyField(line, targetEnt, new GUIContent(IOConfig.Hexagon));
                }

                {
                    var line = area.GetNextLine();
                    
                    DrawInputSelector(line, entity, input, target);
                }
            }

            void DrawPlayer()
            {
                var playerID = property.FindPropertyRelative("playerId");
                
                {
                    var line = area.GetNextLine();

                    EditorGUI.PropertyField(line, playerID, new GUIContent("PlayerID",IOConfig.Human));
                }

                {
                    var pos = area.GetNextLine();

                    var playerEntity = LogicSystemSettings.getSettings().m_DefaultPlayerEntity.GetComponent<Entity>();
                    
                    //TODO: This is a hack
                    //We need to do this to make sure the prefab version of the player has the components wired up
                    playerEntity.ForceCollectComponents();
                    
                    DrawInputSelector(pos, playerEntity,input, target);
                }
            }
            
        }

        private static void DrawInputSelector(RectArea pos, Entity entity, SerializedProperty input, SerializedProperty target)
        {
            

            pos.AddLabelPrefix(new GUIContent(IOConfig.ArrowOut));

            {
                var comp_name_area = pos.GetHorizontalArea(pos.free.width / 2);
                string name = "";
                CBase componentTarget = null;
                if (entity != null)
                {
                    componentTarget = entity.components.Find(c => c.Name == target.stringValue);

                    name = componentTarget != null ? componentTarget.Name ?? " - " : " - ";
                }

                if (GUIHelpers.DoBasicPreview(comp_name_area, name, componentTarget))
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