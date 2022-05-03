using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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

        private List<InputRef> _inputs;
        public List<InputRef> Inputs
        {
            get
            {
                if (_inputs is null)
                {
                    _inputs = ExtractInputs(this);
                }

                return _inputs;
            }
        }

        private static List<InputRef> ExtractInputs(CBase comp)
        {
            var type = comp.GetType();
            var methods =
                comp.GetType()
                    .GetMethods();

            var res = new List<InputRef>();
            
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute(typeof(InputAttribute)) is not null)
                {
                    res.Add(new InputRef((InputRef.Input)method.CreateDelegate(typeof(InputRef.Input), comp)));
                }
            }

            return res;
        }
        
        private List<OutputRef> _outputs;
        public List<OutputRef> Outputs
        {
            get
            {
                if (_outputs is null)
                {
                    _outputs = ExtractOutputs(this);
                }

                return _outputs;
            }
        }

        private static List<OutputRef> ExtractOutputs(CBase comp)
        {
            var type = comp.GetType();
            var members =
                comp.GetType()
                    .GetMembers();

            var res = new List<OutputRef>();
            
            foreach (var member in members)
            {
                if ((member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property))
                {
                    if (member is FieldInfo field && field.FieldType == typeof(Output))
                    {
                        var getter = ReflectionHelper.CreateFieldGetter<Output>(type, field);
                        var get = new OutputRef(field.Name,getter);
                        res.Add(get);
                    }
                    //res.Add(new (method.Name));
                    //res.Add();
                }
            }

            return res;
        }


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

            StringBuilder b = new StringBuilder();
            
            var t = this.GetType();
            b.Append($"{t.Name} has:\n");
            foreach (var inp in Inputs)
            {
                b.Append($"{inp._fn.Method.Name}\n");
            }
            foreach (var inp in Outputs)
            {
                b.Append($"{inp.Name}\n");
            }
            
            Debug.Log(b.ToString());
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

    public class InputRef
    {
        public delegate void Input(Event ev);
        
        public readonly Input _fn;

        public InputRef(Input fn)
        {
            _fn = fn;
        }

        public void Call(Event ev)
        {
            _fn.Invoke(ev);
        }
        
    }
    
    public class OutputRef
    {
        public string Name;
        public readonly Func<object, Output> _g;

        //public delegate Output Getter();
        
        public OutputRef(string n, Func<object,Output> g)
        {
            this.Name = n;
            _g = g;
        }
        
    }
}