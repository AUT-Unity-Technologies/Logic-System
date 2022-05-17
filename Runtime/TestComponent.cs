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


    public class InputAttribute : Attribute
    {
        
    }
}