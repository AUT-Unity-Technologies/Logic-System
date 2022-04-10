
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
        
        private static Texture2D sPickerIcon = null;
        internal static Texture2D PickerIcon
        {
            get
            {
                if (sPickerIcon == null)
                    sPickerIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                        kPackageRoot
                        + "/Editor/EditorResources/SceneViewPickerIcon.psd");
                ;
                if (sPickerIcon != null)
                    sPickerIcon.hideFlags = HideFlags.DontSaveInEditor;
                return sPickerIcon;
            }
        }

        
        private static Texture2D sArrowOut = null;
        internal static Texture2D ArrowOut => LoadAsset(ref sArrowOut, kPackageRoot + "/Editor/EditorResources/arrow-right-bold-hexagon-outline.png");
        
        
        private static Texture2D LoadAsset(ref Texture2D asset, string path)
        {
            if (asset == null)
                asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            ;
            if (asset != null)
                asset.hideFlags = HideFlags.DontSaveInEditor;
            return (Texture2D)asset;
        }
    }
}