using System;
using System.Collections.Generic;
using com.dpeter99.framework.Runtime;
using LogicSystem.EventSystem;
using UnityEngine;
using Event = LogicSystem.EventSystem.Event;


namespace LogicSystem
{
    [Serializable]
    public class Output
    {
        
        public List<Binding> targets = new ();

        public void Call(Entity source)
        {
            //If we are not in play mode we do not do anything
            if (!Application.isPlaying)
                return;

            foreach (var target in targets)
            {
                
                ModuleProvider.Get<EventBus>().AddEvent(
                    new Event(source)
                    {
                        target = target,
                        targetTime = Time.frameCount
                    }
                );    
            }
            
            
        }
    }
}