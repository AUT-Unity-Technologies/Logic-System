using System;
using UnityEngine;

namespace LogicSystem.Components
{
    public class PropDoorRotating: CBase
    {
        public GameObject doorModel;

        public float min;
        public float max;

        float target;
        float t;

        public enum DoorState
        {
            Closed,
            Closing,
            Open,
            Opening,
        }

        public DoorState state;
        
        private void Update()
        {
            switch (state)
            {
                case DoorState.Closed:
                    break;
                case DoorState.Closing:
                    break;
                case DoorState.Open:
                    break;
                case DoorState.Opening:
                    DoOpen();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }

        private void DoOpen()
        {
            t += Time.deltaTime;
            
            var rot = Mathf.LerpAngle(0, target, t);
            
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                rot,
                transform.eulerAngles.z
            );
        }


        [Input]
        public void Open(LogicSystem.EventSystem.Event ev)
        {
            //Debug.Log("opening");

            target = max;
            state = DoorState.Opening;
        }
    }
}