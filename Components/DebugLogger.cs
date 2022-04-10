using UnityEngine;
using Event = LogicSystem.EventSystem.Event;

namespace LogicSystem.Components
{
    [AddComponentMenu("Logic Components/Debug Logger")]
    public class DebugLogger : CBase
    {

        public string message; 
        
        [Input]
        public void Log(Event ev)
        {
            Debug.Log(message);
        }

    }
}