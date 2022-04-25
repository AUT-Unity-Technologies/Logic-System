using System;
using UnityEngine;

namespace LogicSystem.Components
{
    /// <summary>
    /// This component fires events corresponding to trigger events.
    /// To use it you have to add a collider to the Entity, this can be a simple box/sphere collider or a mesh collider.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Trigger : CBase
    {
        private static Color _visualizerColor = new Color(0,1,0,0.25f);

        /// <summary>
        /// Disables the trigger after first activation of any of the outputs
        /// </summary>
        public bool once = false;
        
        
        
        /// <summary>
        /// On Start Touch is fired when a object enters the Trigger area. 
        /// </summary>
        public Output OnStartTouch;
        
        /// <summary>
        /// On End Touch is fired when an object exists the Trigger (<see cref="Collider"/>) area.
        /// </summary>
        public Output OnEndTouch;

        private void OnTriggerEnter(Collider other)
        {
            if (isActiveAndEnabled)
            {
                OnStartTouch.Call(entity);

                if (once) enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isActiveAndEnabled)
            {
                OnEndTouch.Call(entity);
                
                if (once) enabled = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //throw new NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            Collider collider = GetComponent<Collider>();

            Gizmos.color = _visualizerColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (collider.isTrigger)
            {
                if (collider is BoxCollider box)
                {
                    Gizmos.DrawCube(box.center, box.size);
                }
            }
        }
    }
}