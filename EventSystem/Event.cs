using System;

namespace LogicSystem.EventSystem
{
    public class Event
    {
        public int startTime;
        public int targetTime;

        public Binding target;

        public override string ToString()
        {
            return
                "() => " +
                target.targetEntity.entity.name + "::" + target.target + "::" + target.input;
        }
    }
}