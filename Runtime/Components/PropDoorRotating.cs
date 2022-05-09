using System;
using UnityEngine;

namespace LogicSystem.Components
{
    public class PropDoorRotating: CBase
    {
        public GameObject doorModel;

        public float min;
        public float max;

        private float _target;
        private float _t;

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
            _t += Time.deltaTime;
            
            var rot = Mathf.LerpAngle(0, _target, _t);

            var eulerAngles = transform.eulerAngles;
                
            eulerAngles = new Vector3(
                eulerAngles.x,
                rot,
                eulerAngles.z
            );
            transform.eulerAngles = eulerAngles;
        }


        [Input]
        public void Open(LogicSystem.EventSystem.Event ev)
        {
            //Debug.Log("opening");

            _target = max;
            state = DoorState.Opening;
        }
    }
}