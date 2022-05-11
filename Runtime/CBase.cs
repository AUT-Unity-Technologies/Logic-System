using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;

namespace LogicSystem
{
    /// <summary>
    /// Base class for all components
    /// It provides reflected information and registration to the entity.
    /// </summary>
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public abstract class CBase : MonoBehaviour
    {
        /// <summary>
        /// The entity this component is attached to.
        /// </summary>
        protected Entity entity;
        
        #if UNITY_EDITOR
        [SerializeField]
        private uint foldSate;
        
        #endif

        /// <summary>
        /// Private backing field of <see cref="Inputs"/>
        /// </summary>
        private List<InputRef> _inputs;
        
        /// <summary>
        /// List of all the inputs that this component has
        /// </summary>
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

        /// <summary>
        /// Static method for getting a list of all the Inputs that are on the component
        /// </summary>
        /// <param name="comp">The actual component to get Inputs for</param>
        /// <returns>The list of Inputs that are on the component</returns>
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
                    res.Add(new InputRef(method.Name, (InputRef.Input)method.CreateDelegate(typeof(InputRef.Input), comp)));
                }
            }
            
            StringBuilder b = new StringBuilder();
            
            {
                b.Append($"{type.Name} has:\n");
                foreach (var inp in res)
                {
                    b.Append($"{inp.Name}\n");
                }
                //Debug.Log(b.ToString());
            }
            
            return res;
        }
        
        /// <summary>
        /// Private backing field for <see cref="Outputs"/>
        /// </summary>
        private List<OutputRef> _outputs;
        
        /// <summary>
        /// List of all of the outputs that are on the component
        /// </summary>
        public List<OutputRef> Outputs => _outputs ??= ExtractOutputs(this);

        /// <summary>
        /// Static cache of the reflected Output references
        /// The key is the type they were generated for
        /// It should not be directly accessed as it is not guaranteed that the needed type
        /// was already cached.
        /// Use <see cref="ExtractOutputs"/> instead 
        /// </summary>
        private static Dictionary<Type, List<OutputRef>> _staticOutputList = new();

        private static List<OutputRef> ExtractOutputs(CBase comp)
        {
            var compType = comp.GetType();
            
            if (_staticOutputList.TryGetValue(compType, out var list))
            {
                return list;
            }
            
            //var members = compType.GetMembers();
            
            var res = compType.GetMembers()
                .OfType<FieldInfo>()
                .Where(info => info.FieldType == typeof(Output))
                .Select(field =>
                {
                    var getter = ReflectionHelper.CreateFieldGetter<Output>(compType, field);
                    return new OutputRef(field.Name, getter);
                })
                .ToList();
            
            /*
            foreach (var member in members)
            {
                if ((member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property))
                {
                    if (member is FieldInfo field && field.FieldType == typeof(Output))
                    {
                        var getter = ReflectionHelper.CreateFieldGetter<Output>(compType, field);
                        var get = new OutputRef(field.Name,getter);
                        res.Add(get);
                    }
                }
            }*/
            
            _staticOutputList.Add(compType,res);

            {
                StringBuilder b = new StringBuilder();
                b.Append($"{compType.Name} has:\n");
                foreach (var inp in res)
                {
                    b.Append($"{inp.name}\n");
                }

                //Debug.Log(b.ToString());
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
                //entity.UpdateComponentName(this);
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

    public class InputRef
    {
        public delegate void Input(Event ev);
        
        private readonly Input _fn;

        public string Name { get; }
        
        public InputRef(string name, Input fn)
        {
            this.Name = name;
            _fn = fn;
        }

        

        public void Invoke(Event ev)
        {
            _fn.Invoke(ev);
        }
        
    }
    
    public class OutputRef
    {
        public readonly string name;
        private readonly Func<object, Output> _g;

        //public delegate Output Getter();
        
        public OutputRef(string n, Func<object,Output> g)
        {
            this.name = n;
            _g = g;
        }

        public Output Get(CBase comp)
        {
            return _g.Invoke(comp);
        }
    }
}