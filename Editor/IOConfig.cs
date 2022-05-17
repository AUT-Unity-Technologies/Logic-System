
using UnityEditor;
using UnityEngine;

namespace LogicSystem.Editor
{
    public class IOConfig
    {
        public static string kPackageRoot = "Packages/hu.bme.aut.logicsystem";
        
        private static Texture2D sCloseThick = null;
        internal static Texture2D CloseThick => LoadAsset(ref sCloseThick, kPackageRoot + "/Editor/EditorResources/close-thick.png");


        private static Texture2D sPickerIcon = null;
        internal static Texture2D PickerIcon => LoadAsset(ref sPickerIcon, kPackageRoot + "/Editor/EditorResources/SceneViewPickerIcon.psd");


        private static Texture2D sArrowOut = null;
        internal static Texture2D ArrowOut => LoadAsset(ref sArrowOut, kPackageRoot + "/Editor/EditorResources/arrow-right-bold-hexagon-outline.png");
        
        
        private static Texture2D sHexagon = null;
        internal static Texture2D Hexagon => LoadAsset(ref sHexagon, kPackageRoot + "/Editor/EditorResources/hexagon.png");
        
        private static Texture2D sHuman = null;
        internal static Texture2D Human => LoadAsset(ref sHuman, kPackageRoot + "/Editor/EditorResources/human-handsup.png");
        
        
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