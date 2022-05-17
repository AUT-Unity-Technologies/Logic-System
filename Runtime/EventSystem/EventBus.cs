using System;
using System.Collections.Generic;
using com.dpeter99.framework.Runtime;
using com.dpeter99.utils;
using UnityEngine;
using UnityEngine.LowLevel;
using Object = UnityEngine.Object;

namespace LogicSystem.EventSystem
{
    [AutoService]
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
            
            while (events.Count > 0 && events.Peek().targetTime < curr)
            {
                var ev = events.Dequeue();
                Debug.Log(ev.ToString(),this);

                switch (ev.target.type)
                {
                    case BindingType.Direct:
                        ev.target.targetEntity.entity.ProcessEvent(ev);
                        break;
                    case BindingType.System:
                        break;
                    case BindingType.Player:
                        DispatcToPlayer(ev);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DispatcToPlayer(Event ev)
        {
            var player = ModuleProvider.Get<PlayGameModeBase>().players[ev.target.playerId] as MonoBehaviour;
            player.GetComponent<Entity>().ProcessEvent(ev);

        }
    }
}