using LogicSystem.EventSystem;

namespace LogicSystem.Components
{
    public class MathCount: CBase
    {
        public int min = 0;
        public int max = 10;

        private int _current = 0;

        public Output OnHitMax;
        
        public Output OnValueChanged;
        
        public int Current
        {
            get => _current;

            set
            {
                if (value <= max && value >= min)
                {
                    _current = value;    
                }
                ValueChanged();
            }
        }

        private void ValueChanged()
        {
            //OnHitMax.Call(entity);
            if (_current == max)
            {
                OnHitMax.Call(entity);
            }
            
        }


        [Input]
        public void Increment(Event ev)
        {
            Current++;
        }

        [Input]
        public void Decrement(Event ev)
        {
            Current--;
        }
        
        
        
    }
}