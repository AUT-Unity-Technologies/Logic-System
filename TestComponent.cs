using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LogicSystem
{
    public class TestComponent : CBase
    {

        public Output onSpawn; 

        public List<TestObject> test;
        
        [Input()]
        public void TestOuptut()
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
        public MonoBehaviour target;
        public string input;
    }


    public class InputAttribute : Attribute
    {
        
    }
}