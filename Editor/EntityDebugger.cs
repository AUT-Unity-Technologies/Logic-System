using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace LogicSystem.Editor
{
    public class EntityDebugger : EditorWindow
    {
        [MenuItem("Tools/EntityDebugger")]
        private static void ShowWindow()
        {
            var window = GetWindow<EntityDebugger>();
            window.titleContent = new GUIContent("Entities");
            window.Show();
        }

        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField] TreeViewState m_TreeViewState;

        //The TreeView is not serializable, so it should be reconstructed from the tree data.
        SimpleTreeView m_SimpleTreeView;
        
        void OnEnable ()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState ();

            m_SimpleTreeView = new SimpleTreeView(m_TreeViewState);
        }

        void OnGUI ()
        {
            if (m_SimpleTreeView == null)
            {
                GUILayout.Label("Ammm IDK something is wrong");
                return;
            }
                
            m_SimpleTreeView.Reload();
            m_SimpleTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
        }
        
        class SimpleTreeView : TreeView
        {
            public SimpleTreeView(TreeViewState treeViewState)
                : base(treeViewState)
            {
                Reload();
            }
        
            protected override TreeViewItem BuildRoot ()
            {
                // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
                // are created from data. Here we create a fixed set of items. In a real world example,
                // a data model should be passed into the TreeView and the items created from the model.

                // This section illustrates that IDs should be unique. The root item is required to 
                // have a depth of -1, and the rest of the items increment from that.
                var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};
                var allItems = new List<TreeViewItem>();

                var entities = EntityManager.GetAllEntites();
                var scenes = entities.GroupBy(ent => ent.sourceScene);
                
                foreach (var scene in scenes)
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.Key.name);
                    
                    allItems.Add(
                        new TreeViewItem { id = scene.Key.handle, depth = 0, displayName = scene.Key.name }
                    );

                    foreach (var entity in scene)
                    {
                        if (entity.entity == null)
                        {
                            continue;
                        }
                        allItems.Add(
                            new TreeViewItem {id = entity.go.GetInstanceID(), depth = 1, displayName = entity.entity.name ?? "Unknown"}
                        );
                    }

                }

                // Utility method that initializes the TreeViewItem.children and .parent for all items.
                SetupParentsAndChildrenFromDepths (root, allItems);
            
                // Return root of the tree
                return root;
            }

            protected override void SelectionChanged(IList<int> selectedIds)
            {
                var ids = selectedIds.ToArray();
                
                if ((uint) ids.Length > 0U)
                    Selection.activeInstanceID = ids[ids.Length - 1];
                Selection.instanceIDs = ids;
            }
        }
        
        
    }
}