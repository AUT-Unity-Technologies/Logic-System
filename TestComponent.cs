using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LogicSystem
{
    public class TestComponent : CBase
    {

        public Output onSpawn;

        [Input()]
        public void TestInput()
        {
            
        }
        
    }

    [Serializable]
    public class Output
    {
        
        public List<Binding> targets = new List<Binding>();

        public void Call()
        {
            InGameDebugUI.Log("asd");
        }
    }

    [Serializable]
    public class Binding
    {
        public GuidReference targetEntity;
        public string target;
        public string input;
    }


    public class InputAttribute : Attribute
    {
        
    }
}