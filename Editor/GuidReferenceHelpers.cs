using UnityEditor;

namespace LogicSystem.Editor
{
    public static class GuidReferenceHelpers
    {
        
        static GuidReference def = new ();
        
        public static void ParseGUIDFromProp(SerializedProperty guidProp, out byte[] byteArray)
        {
            byteArray = new byte[16];
        
            int arraySize = guidProp.arraySize;
            for (int i = 0; i < arraySize; ++i)
            {
                var byteProp = guidProp.GetArrayElementAtIndex(i);
                byteArray[i] = (byte)byteProp.intValue;
            }
        }

        public static System.Guid GetGuidFromProperty(SerializedProperty referenceProp)
        {
            var guidProp = referenceProp.FindPropertyRelative("serializedGuid");

            byte[] guid;
            ParseGUIDFromProp(guidProp,out guid);
            
            return new System.Guid(guid);
        }
        
    }
}