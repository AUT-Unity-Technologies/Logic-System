using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class GUIHelpers
{
    private static readonly int s_BasicObjectPreviewHash = nameof (s_BasicObjectPreviewHash).GetHashCode();
    
    private static readonly GUIContent s_MixedValueContent = EditorGUIUtility.TrTextContent("—", "Mixed Values");

    public static bool DoBasicPreview<T>(Rect area, SerializedProperty prop)
    {
        return DoBasicPreview(area,prop,typeof(T));
    }
    
    public static bool DoBasicPreview(Rect area, SerializedProperty prop, Type type)
    {
        EditorGUI.BeginProperty(area, GUIContent.none, prop);
        
        //int controlId = GUIUtility.GetControlID(s_BasicObjectPreviewHash, FocusType.Keyboard, area);
        
        var style = EditorStyles.objectField;
        
        var img = new Rect(area);
        //img.width = EditorGUIUtility.singleLineHeight;

        GUIContent content = new GUIContent("?");

        if (prop.type == "string")
        {
            content = new GUIContent(prop.stringValue);
        }
        else if (typeof(Object).IsAssignableFrom(type))
        {
            content = EditorGUIUtility.ObjectContent(prop.objectReferenceValue, type);
        }

        if (EditorGUI.showMixedValue || prop.hasMultipleDifferentValues)
        {
            content = s_MixedValueContent;
        }
        
        var clicked = GUI.Button(img,content,style);

        EditorGUI.EndProperty();
        
        return clicked;
    }
}