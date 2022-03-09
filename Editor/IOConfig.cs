
using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor
{
    public class IOConfig
    {
        public static string kPackageRoot = "Assets/LogicSystem";
        
        private static Texture2D sCloseThick = null;
        internal static Texture2D CloseThick
        {
            get
            {
                if (sCloseThick == null)
                    sCloseThick = AssetDatabase.LoadAssetAtPath<Texture2D>(
                        kPackageRoot
                        + "/Editor/EditorResources/close-thick.png");
                ;
                if (sCloseThick != null)
                    sCloseThick.hideFlags = HideFlags.DontSaveInEditor;
                return sCloseThick;
            }
        }
    }
}