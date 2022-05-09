using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
#endif

namespace LogicSystem
{
    /// <summary>
    /// Marks a Game object that can be referenced cross scenes.
    /// </summary>
    /// 
    [ExecuteInEditMode, DisallowMultipleComponent]
    public class Entity : MonoBehaviour, ISerializationCallbackReceiver
    {
        // System guid we use for comparison and generation
        private Guid _guid = Guid.Empty;

        // Unity's serialization system doesn't know about System.Guid, so we convert to a byte array
        // Fun fact, we tried using strings at first, but that allocated memory and was twice as slow
        [SerializeField] private byte[] serializedGuid;
        
        
        // When de-serializing or creating this component, we want to either restore our serialized GUID
        // or create a new one.
        private void CreateGuid()
        {
            // if our serialized data is invalid, then we are a new object and need a new GUID
            if (serializedGuid == null || serializedGuid.Length != 16)
            {
#if UNITY_EDITOR
                // if in editor, make sure we aren't a prefab of some kind
                if (IsAssetOnDisk())
                {
                    return;
                }

                Undo.RecordObject(this, "Added GUID");
#endif
                _guid = Guid.NewGuid();
                serializedGuid = _guid.ToByteArray();

#if UNITY_EDITOR
                // If we are creating a new GUID for a prefab instance of a prefab, but we have somehow lost our prefab connection
                // force a save of the modified prefab instance properties
                if (PrefabUtility.IsPartOfNonAssetPrefabInstance(this))
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                }
#endif
            }
            else if (_guid == Guid.Empty)
            {
                // otherwise, we should set our system guid to our serialized guid
                _guid = new Guid(serializedGuid);
            }

            // register with the GUID Manager so that other components can access this
            if (_guid != Guid.Empty)
            {
                //TODO: Add to a manager
                
                if (!EntityManager.Add(this))
                {
                    // if registration fails, we probably have a duplicate or invalid GUID, get us a new one.
                    serializedGuid = null;
                    _guid = Guid.Empty;
                    CreateGuid();
                }
                
            }
        }

#if UNITY_EDITOR
        private bool IsEditingInPrefabMode()
        {
            if (EditorUtility.IsPersistent(this))
            {
                // if the game object is stored on disk, it is a prefab of some kind, despite not returning true for IsPartOfPrefabAsset =/
                return true;
            }
            else
            {
                // If the GameObject is not persistent let's determine which stage we are in first because getting Prefab info depends on it
                var mainStage = StageUtility.GetMainStageHandle();
                var currentStage = StageUtility.GetStageHandle(gameObject);
                if (currentStage != mainStage)
                {
                    var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
                    if (prefabStage != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsAssetOnDisk()
        {
            return PrefabUtility.IsPartOfPrefabAsset(this) || IsEditingInPrefabMode();
        }
#endif

        // We cannot allow a GUID to be saved into a prefab, and we need to convert to byte[]
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            // This lets us detect if we are a prefab instance or a prefab asset.
            // A prefab asset cannot contain a GUID since it would then be duplicated when instanced.
            if (IsAssetOnDisk())
            {
                serializedGuid = null;
                _guid = System.Guid.Empty;
            }
            else
#endif
            {
                if (_guid != Guid.Empty)
                {
                    serializedGuid = _guid.ToByteArray();
                }
            }
        }

        // On load, we can go head a restore our system guid for later use
        public void OnAfterDeserialize()
        {
            if (serializedGuid != null && serializedGuid.Length == 16)
            {
                _guid = new Guid(serializedGuid);
            }
        }

        private void Awake()
        {
            CreateGuid();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            // similar to on Serialize, but gets called on Copying a Component or Applying a Prefab
            // at a time that lets us detect what we are
            if (IsAssetOnDisk())
            {
                serializedGuid = null;
                _guid = System.Guid.Empty;
            }
            else
#endif
            {
                CreateGuid();
            }
        }

        // Never return an invalid GUID
        public Guid GetGuid()
        {
            if (_guid == Guid.Empty && serializedGuid != null && serializedGuid.Length == 16)
            {
                _guid = new Guid(serializedGuid);
            }

            return _guid;
        }

        // let the manager know we are gone, so other objects no longer find this
        public void OnDestroy()
        {
            EntityManager.Remove(_guid);
        }

        /// <summary>
        /// A list to hold all the components that are attached to this entity.
        /// This list is used to find the entity when an event is received.
        /// </summary>
        [NonSerialized]
        public readonly List<CBase> components = new();

        /// <summary>
        /// Add a component to the entity
        /// This is needed for a component to get events.
        /// </summary>
        /// <param name="cBase">The component to add</param>
        public void AddComponent(CBase cBase)
        {
            //TODO: Check that the cbase is on the same GO as we are
            components.Add(cBase);
        }

        /// <summary>
        /// Removes the component from the entity
        /// This should only be done when the component is destroyed. 
        /// </summary>
        /// <param name="cBase"></param>
        public void RemoveComponent(CBase cBase)
        {
            components.Remove(cBase);
        }

        [Obsolete]
        public void UpdateComponentName(CBase cBase)
        {
            
        }

        /// <summary>
        /// Propagates an event to the entity for processing
        /// This will dispatch the event to the target component for processing
        /// For this to work the event's target component needs to be added to the <see cref="AddComponent"/>
        /// </summary>
        /// <param name="ev">The event to process</param>
        /// <example>
        /// This examples shows how to directly send an event:
        /// <code>
        /// Event e = new Event();
        /// 
        /// entity.ProcessEvent(e)
        /// </code>
        /// </example>
        public void ProcessEvent(Event ev)
        {
            var comp = this.components.Find(c => c.Name == ev.target.target);
            if (comp != null)
            {
                var input = comp.Inputs.FirstOrDefault(i => i.Name == ev.target.input);
                
                if (input != null)
                {
                    input.Invoke(ev);
                }

            }
        }
    }
}