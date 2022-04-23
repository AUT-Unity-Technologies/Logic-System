using System;
//using DefaultNamespace;
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