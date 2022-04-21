using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using com.dpeter99.framework.src;
using com.dpeter99.utils.Basic;
using LogicSystem;
using UnityEngine.SceneManagement;

namespace LogicSystem
{
    // Class to handle registering and accessing objects by GUID
    [Manager]
    [InstantiateIfMissing]
    public class EntityManager : Singleton<EntityManager>, IModule
    {
        // for each GUID we need to know the Game Object it references
        // and an event to store all the callbacks that need to know when it is destroyed
        public struct GuidInfo
        {
            public Scene sourceScene;
            
            public GameObject go;
            public Entity entity;

            public event Action<GameObject> OnAdd;
            public event Action OnRemove;

            public GuidInfo(Entity comp)
            {
                go = comp.gameObject;
                entity = comp;

                sourceScene = go.scene;
                
                OnRemove = null;
                OnAdd = null;
            }

            public void HandleAddCallback()
            {
                if (OnAdd != null)
                {
                    OnAdd(go);
                }
            }

            public void HandleRemoveCallback()
            {
                if (OnRemove != null)
                {
                    OnRemove();
                }
            }
        }

        // Singleton interface
        //static EntityManager Instance;

        static EntityManager()
        {
            //new EntityManager();
        }
        
        public EntityManager()
        {
            Debug.Log("Inst");
            guidToObjectMap = new Dictionary<System.Guid, GuidInfo>();
        }

        
        // All the public API is static so you need not worry about creating an instance
        public static bool Add(Entity entity)
        {
            return Instance.InternalAdd(entity);
        }

        public static void Remove(System.Guid guid)
        {
            Instance.InternalRemove(guid);
        }

        public static GameObject ResolveGuid(System.Guid guid, Action<GameObject> onAddCallback, Action onRemoveCallback)
        {
            return Instance.ResolveGuidInternal(guid, onAddCallback, onRemoveCallback);
        }

        public static GameObject ResolveGuid(System.Guid guid, Action onDestroyCallback)
        {
            return Instance.ResolveGuidInternal(guid, null, onDestroyCallback);
        }

        public static GameObject ResolveGuid(System.Guid guid)
        {
            return Instance.ResolveGuidInternal(guid, null, null);
        }

        // instance data
        private Dictionary<System.Guid, GuidInfo> guidToObjectMap;


        private bool InternalAdd(Entity entity)
        {
            Guid guid = entity.GetGuid();

            GuidInfo info = new GuidInfo(entity);

            if (!guidToObjectMap.ContainsKey(guid))
            {
                guidToObjectMap.Add(guid, info);
                return true;
            }

            GuidInfo existingInfo = guidToObjectMap[guid];
            if (existingInfo.go != null && existingInfo.go != entity.gameObject)
            {
                // normally, a duplicate GUID is a big problem, means you won't necessarily be referencing what you expect
                if (Application.isPlaying)
                {
                    Debug.AssertFormat(false, entity,
                        "Guid Collision Detected between {0} and {1}.\nAssigning new Guid. Consider tracking runtime instances using a direct reference or other method.",
                        (guidToObjectMap[guid].go != null ? guidToObjectMap[guid].go.name : "NULL"), (entity != null ? entity.name : "NULL"));
                }
                else
                {
                    // however, at editor time, copying an object with a GUID will duplicate the GUID resulting in a collision and repair.
                    // we warn about this just for pedantry reasons, and so you can detect if you are unexpectedly copying these components
                    Debug.LogWarningFormat(entity, "Guid Collision Detected while creating {0}.\nAssigning new Guid.",
                        (entity != null ? entity.name : "NULL"));
                }

                return false;
            }

            // if we already tried to find this GUID, but haven't set the game object to anything specific, copy any OnAdd callbacks then call them
            existingInfo.go = info.go;
            existingInfo.HandleAddCallback();
            guidToObjectMap[guid] = existingInfo;
            return true;
        }

        private void InternalRemove(System.Guid guid)
        {
            GuidInfo info;
            if (guidToObjectMap.TryGetValue(guid, out info))
            {
                // trigger all the destroy delegates that have registered
                info.HandleRemoveCallback();
            }

            guidToObjectMap.Remove(guid);
        }

        // nice easy api to find a GUID, and if it works, register an on destroy callback
        // this should be used to register functions to cleanup any data you cache on finding
        // your target. Otherwise, you might keep components in memory by referencing them
        private GameObject ResolveGuidInternal(System.Guid guid, Action<GameObject> onAddCallback, Action onRemoveCallback)
        {
            GuidInfo info;
            if (guidToObjectMap.TryGetValue(guid, out info))
            {
                if (onAddCallback != null)
                {
                    info.OnAdd += onAddCallback;
                }

                if (onRemoveCallback != null)
                {
                    info.OnRemove += onRemoveCallback;
                }

                guidToObjectMap[guid] = info;
                return info.go;
            }

            if (onAddCallback != null)
            {
                info.OnAdd += onAddCallback;
            }

            if (onRemoveCallback != null)
            {
                info.OnRemove += onRemoveCallback;
            }

            guidToObjectMap.Add(guid, info);

            return null;
        }

        public static IEnumerable<GuidInfo> GetAllEntites()
        {
            return Instance.guidToObjectMap.Values;
        }
    }
}