using UnityEngine;

namespace LogicSystem.Components
{
    [AddComponentMenu("Logic Components/Logic Auto")]
    public class LogicAuto : CBase
    {
        
        
        
        public Output onMapLoad;

        private void Start()
        {
            onMapLoad.Call(entity);
        }
    }
}