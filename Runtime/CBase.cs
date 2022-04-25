using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;

namespace LogicSystem
{
    /// <summary>
    /// Base class for all components
    /// </summary>
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public class CBase : MonoBehaviour
    {
        protected Entity entity;
        
        #if UNITY_EDITOR
        [SerializeField]
        private uint foldSate;
        
        #endif
        
        [SerializeField]
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                entity.UpdateComponentName(this);
                _name = value;
            }
        }

        private void Awake()
        {
            entity = gameObject.GetComponent<Entity>();
        }

        private void OnEnable()
        {
            if (Name == null)
            {
                Name = this.GetType().Name;
            }
            var ent = gameObject.GetComponent<Entity>();
            if (ent != null)
            {
                ent.AddComponent(this);
            }
        }

        private void OnDisable()
        {
            var ent = gameObject.GetComponent<Entity>();
            if (ent != null)
            {
                ent.RemoveComponent(this);
            }
        }


        [Input]
        public void Toggle(Event ev)
        {
            this.enabled = !this.enabled;
        }
        
        
        
    }
}