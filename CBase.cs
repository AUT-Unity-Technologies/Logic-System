using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LogicSystem
{
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public class CBase : MonoBehaviour
    {
        private Entity ent;
        
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
                ent.UpdateComponentName(this);
                _name = value;
            }
        }

        private void Awake()
        {
            ent = gameObject.GetComponent<Entity>();
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
        public void Toggle()
        {
            this.enabled = !this.enabled;
        }
        
        
        
    }
}