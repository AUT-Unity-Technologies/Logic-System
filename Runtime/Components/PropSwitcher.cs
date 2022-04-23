using UnityEngine;

namespace LogicSystem.Components
{
    public class PropSwitcher: CBase
    {

        public MeshRenderer target;
        
        public Material material_A;
        public Material material_B;

        private bool switched = false;
        
        [Input]
        public void Switch(LogicSystem.EventSystem.Event ev)
        {
            if (!switched)
            {
                target.material = material_B;
            }
            else
            {
                target.material = material_A;    
            }

            switched = !switched;

        }

    }
}