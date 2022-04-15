using System;
using UnityEngine;

namespace LogicSystem.Components
{
    
    [RequireComponent(typeof(Collider))]
    public class Trigger : CBase
    {
        public Output OnStartTouch;
        
        public Output OnEndTouch;

        private void OnTriggerEnter(Collider other)
        {
            if (isActiveAndEnabled)
            {
                OnStartTouch.Call(entity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isActiveAndEnabled)
            {
                OnEndTouch.Call(entity);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //throw new NotImplementedException();
        }
    }
}