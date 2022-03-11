using System.Collections.Generic;
using UnityEngine;

namespace LogicSystem
{
    [RequireComponent(typeof(Entity))]
    public class CBase : MonoBehaviour
    {

        [Input]
        public void Toggle()
        {
            this.enabled = !this.enabled;
        }
        
    }
}