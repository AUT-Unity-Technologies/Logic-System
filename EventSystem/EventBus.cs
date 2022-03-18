using System;
using System.Collections.Generic;
using com.dpeter99.framework.src;
using com.dpeter99.utils;
using UnityEngine;
using UnityEngine.LowLevel;

namespace LogicSystem.EventSystem
{
    [Manager]
    public class EventBus : MonoBehaviour, IModule
    {
        private PriorityQueue<Event, int> events = new();

        public void AddEvent(Event e)
        {
            events.Enqueue(e,e.targetTime);
        }

        private void Awake()
        {
            var sys = new PlayerLoopSystem()
            {
                updateDelegate = DispatchEvents,
                type = typeof(EventBus)
            };

            var loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelpers.AppendSystemToPlayerLoopList(sys,ref loop,typeof(UnityEngine.PlayerLoop.EarlyUpdate));
            PlayerLoop.SetPlayerLoop(loop);
        }

        private void DispatchEvents()
        {
            //Debug.Log("Hi!");
            var curr = Time.frameCount;
            while (events.Peek().targetTime < curr)
            {
                var ev = events.Dequeue();
                //Debug.Log("New Event found?");
                ev.target.targetEntity.entity.ProcessEvent(ev);
            }
        }
    }
}