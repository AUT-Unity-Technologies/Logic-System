using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace LogicSystem.Editor
{
    /// <summary>
    /// A abstraction class over the <see cref="SerializedObject"/> to group the parsing code.
    /// This is where we collect all the outputs so we can display them grouped.
    /// </summary>
    public class SerializedCBase
    {
        public SerializedObject serializedObject { get; }

        public List<SerializedProperty> outputs = new ();
        
        public List<SerializedProperty> props = new ();
        
        public SerializedProperty foldState { get; private set; }
        
        public SerializedCBase(SerializedObject target)
        {
            serializedObject = target;

            foldState = serializedObject.FindProperty("foldSate");
            
            var members = target.targetObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var member in members)
            {
                var prop = serializedObject.FindProperty(member.Name);
                if (prop != null)
                {
                    if (member.FieldType != typeof(Output))
                    {
                        props.Add(prop);
                    }
                    else
                    {
                        outputs.Add(prop);
                    }    
                }
                
            }
        }
    }
}