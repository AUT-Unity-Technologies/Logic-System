using System;
using UnityEngine.Serialization;

namespace LogicSystem
{
    /// <summary>
    /// 
    /// </summary>
    public enum BindingType
    {
        Direct,
        System,
        Player
    }
    
    [Serializable]
    public class Binding
    {
        public BindingType type;
        
        public int playerId = 0;
        
        public GuidReference targetEntity;
        
        [FormerlySerializedAs("target")] 
        public string targetComponent;
        
        public string input;


        public override string ToString()
        {
            var res = "";
            switch (type)
            {
                case BindingType.Direct:
                    res += targetEntity.entity.name;
                    break;
                case BindingType.System:
                    break;
                case BindingType.Player:
                    res += "Player #" + playerId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res + "::" + targetComponent + "::" + input;
        }
    }
}