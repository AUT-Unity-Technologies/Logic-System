using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LogicSystem
{
    public class TestComponent : CBase
    {

        public Output onSpawn;

        //public GuidReference guidReference;
        
        //public List<TestObject> test;

        //public UnityEvent ev;
        
        [Input()]
        public void TestInput()
        {
            
        }
        
    }

    [Serializable]
    public class Output
    {
        
        public List<Binding> targets = new List<Binding>();
    }

    [Serializable]
    public class Binding
    {
        public GuidReference targetEntity;
        public CBase target;
        public string input;
    }


    public class InputAttribute : Attribute
    {
        
    }
}