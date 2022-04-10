using System;
using System.Collections.Generic;
using com.dpeter99.framework.src;
using LogicSystem.EventSystem;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;


namespace LogicSystem
{
    [Serializable]
    public class Output
    {
        
        public List<Binding> targets = new ();

        public void Call()
        {
            //If we are not in play mode we do not do anything
            if (!Application.isPlaying)
                return;

            foreach (var target in targets)
            {
                AppScope.root.GetComponent<EventBus>().AddEvent(
                    new Event()
                    {
                        target = target,
                        targetTime = Time.frameCount
                    }
                );    
            }
            
            
        }
    }
}