namespace LogicSystem.EventSystem
{
    public class Event
    {
        public Entity trigger;
        
        public int startTime;
        public int targetTime;

        public Binding target;

        public Event(Entity trigger)
        {
            this.trigger = trigger;
        }
        
        public override string ToString()
        {
            return
                "(" + trigger.name + ") => " + target;
        }
    }
}