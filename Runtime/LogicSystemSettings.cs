using UnityEngine;

namespace LogicSystem
{
    public class LogicSystemSettings : ScriptableObject
    {
        public const string k_ResourceName = "LogicSystemSettings";
        
        public const string k_MyCustomSettingsPath = "Assets/_Game/Resources/" + k_ResourceName + ".asset";


        [SerializeField] public GameObject m_DefaultPlayerEntity;

        public static LogicSystemSettings getSettings()
        {
            var a = Resources.Load<LogicSystemSettings>(k_ResourceName);

            return a;

        }
    }
}