using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;

namespace LogicSystem
{
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public class CBase : MonoBehaviour
    {
        protected Entity entity;
        
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