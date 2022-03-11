using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;

namespace LogicSystem.Editor
{
    public class SearchTreeContextTest : ScriptableObject, ISearchWindowProvider
    {
        private Entity _entity;
        
        public void Init(Entity entity)
        {
            _entity = entity;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            var root_content = new GUIContent(_entity.gameObject.name);
            
            tree.Add(new SearchTreeGroupEntry(root_content,0));

            var components = _entity.gameObject.GetComponents<CBase>();

            foreach (var component in components)
            {
                var comp_content = EditorGUIUtility.ObjectContent(component, typeof(CBase));
                comp_content.text = component.GetType().Name;
                tree.Add(new SearchTreeGroupEntry(comp_content,1));

                var querry =
                    from a in component.GetType().GetMethods()
                    let attributes = a.GetCustomAttributes(typeof(InputAttribute), true)
                    where attributes != null && attributes.Length > 0
                    select a;

                querry = querry.OrderBy(m => m.DeclaringType.FullName);

                List<string> groups = new();

                
                
                foreach (var methodInfo in querry)
                {
                    int level = 2;
                    if (methodInfo.DeclaringType != component.GetType())
                    {
                        string groupName = methodInfo.DeclaringType.Name;
                        if (!groups.Contains(groupName))
                        {
                            tree.Add(new SearchTreeGroupEntry(new GUIContent(groupName),level));
                            level++;
                            groups.Add(groupName);
                        }
                    }
                    
                    var searchTreeEntry = new SearchTreeEntry(new GUIContent(methodInfo.Name));
                    searchTreeEntry.level = level ;
                    tree.Add(searchTreeEntry);
                }
                
            }
            
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            return true;
        }

        
    }
}