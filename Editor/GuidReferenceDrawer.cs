﻿using UnityEditor;
using UnityEngine;
using LogicSystem;
using LogicSystem.Editor;
using WaresoftEditor.Common;

// Using a property drawer to allow any class to have a field of type GuidRefernce and still get good UX
// If you are writing your own inspector for a class that uses a GuidReference, drawing it with
// EditorLayout.PropertyField(prop) or similar will get this to show up automatically
[CustomPropertyDrawer(typeof(GuidReference))]
public class GuidReferenceDrawer : PropertyDrawer
{
    SerializedProperty guidProp;
    SerializedProperty sceneProp;
    SerializedProperty nameProp;

    // cache off GUI content to avoid creating garbage every frame in editor
    GUIContent sceneLabel = new GUIContent("", "The target object is expected in this scene asset.");
    GUIContent clearButtonGUI = new GUIContent("Clear", "Remove Cross Scene Reference");

    // add an extra line to display source scene for targets
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        guidProp = property.FindPropertyRelative("serializedGuid");
        nameProp = property.FindPropertyRelative("cachedName");
        sceneProp = property.FindPropertyRelative("cachedScene");

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);
        
        
        position.height = EditorGUIUtility.singleLineHeight;
        
        // Draw prefix label, returning the new rect we can draw in
        var guidCompPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        float step = guidCompPosition.width / 10;
        guidCompPosition.width = step * 4;
        
        var scenePos = new Rect(guidCompPosition);
        scenePos.x += scenePos.width;
        
        
        
        
        System.Guid currentGuid;
        GameObject currentGO = null;

        // working with array properties is a bit unwieldy
        // you have to get the property at each index manually
        byte[] byteArray = new byte[16];
        ParseGUIDFromProp(out byteArray);

        currentGuid = new System.Guid(byteArray);
        currentGO = GuidManager.ResolveGuid(currentGuid);
        Entity currentGuidComponent = currentGO != null ? currentGO.GetComponent<Entity>() : null;

        Entity component = null;

        bool isNotLoaded = currentGuid != System.Guid.Empty && currentGuidComponent == null;
        
        EditorGUI.BeginChangeCheck();
        
        //#######################################################
        //################  Draw Property  ######################
        //#######################################################
        if (isNotLoaded)
        {
            // if our reference is set, but the target isn't loaded, we display the target and the scene it is in, and provide a way to clear the reference
            //guidCompPosition.xMax -= buttonWidth;

            bool guiEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.LabelField(guidCompPosition, new GUIContent(nameProp.stringValue, "Target GameObject is not currently loaded."), EditorStyles.objectField);
            GUI.enabled = guiEnabled;
        }
        else
        {
            // if our object is loaded, we can simply use an object field directly
            component = EditorGUI.ObjectField(guidCompPosition, currentGuidComponent, typeof(Entity), true) as Entity;
        }

        var changed = EditorGUI.EndChangeCheck();
        
        
        if (true)
        {
            float buttonWidth = 55.0f;
            
            Rect clearButtonRect = new Rect(scenePos);
            clearButtonRect.xMin = scenePos.xMax;
            clearButtonRect.width += EditorGUIUtility.singleLineHeight;

            if (GUI.Button(clearButtonRect, new GUIContent(IOConfig.CloseThick,"Remove Cross Scene Reference"), EditorStyles.iconButton))
            {
                ClearPreviousGuid();
            }
        }
        
        if (currentGuidComponent != null && component == null)
        {
            ClearPreviousGuid();
        }
        
        if (component != null && changed)
        {
            nameProp.stringValue = component.name;
            string scenePath = component.gameObject.scene.path;
            sceneProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            // only update the GUID Prop if something changed. This fixes multi-edit on GUID References
            if (component != currentGuidComponent)
            {
                byteArray = component.GetGuid().ToByteArray();
                int arraySize = guidProp.arraySize;
                for (int i = 0; i < arraySize; ++i)
                {
                    var byteProp = guidProp.GetArrayElementAtIndex(i);
                    byteProp.intValue = byteArray[i];
                }
            }
        }
        
        //#######################################################
        //################  Draw Scene  #########################
        //#######################################################
        
        bool cachedGUIState = GUI.enabled;
        GUI.enabled = true;

        //EditorGUI.ObjectField(scenePos, GUIContent.none, sceneProp.objectReferenceValue, typeof(SceneAsset), false);
        
        if (DoBasicPreview(scenePos,sceneProp))
        {
            EditorGUIUtility.PingObject(sceneProp.objectReferenceValue);
        }
        GUI.enabled = cachedGUIState;

        
        EditorGUI.EndProperty();
    }

    protected void ParseGUIDFromProp(out byte[] byteArray)
    {
        byteArray = new byte[16];
        
        int arraySize = guidProp.arraySize;
        for (int i = 0; i < arraySize; ++i)
        {
            var byteProp = guidProp.GetArrayElementAtIndex(i);
            byteArray[i] = (byte)byteProp.intValue;
        }
    }

    void ClearPreviousGuid()
    {
        nameProp.stringValue = string.Empty;
        sceneProp.objectReferenceValue = null;

        int arraySize = guidProp.arraySize;
        for (int i = 0; i < arraySize; ++i)
        {
            var byteProp = guidProp.GetArrayElementAtIndex(i);
            byteProp.intValue = 0;
        }
    }


    private static readonly int s_BasicObjectPreviewHash = nameof (s_BasicObjectPreviewHash).GetHashCode();
    
    private static readonly GUIContent s_MixedValueContent = EditorGUIUtility.TrTextContent("—", "Mixed Values");
    
    public static bool DoBasicPreview(Rect area, SerializedProperty prop)
    {
        EditorGUI.BeginProperty(area, GUIContent.none, prop);
        
        //int controlId = GUIUtility.GetControlID(s_BasicObjectPreviewHash, FocusType.Keyboard, area);
        
        var style = EditorStyles.objectField;
        
        var img = new Rect(area);
        //img.width = EditorGUIUtility.singleLineHeight;

        var content = EditorGUIUtility.ObjectContent(prop.objectReferenceValue, typeof(SceneAsset));
        
        if (EditorGUI.showMixedValue || prop.hasMultipleDifferentValues)
        {
            content = s_MixedValueContent;
        }
        
        var clicked = GUI.Button(img,content,style);

        EditorGUI.EndProperty();
        
        return clicked;
    }
}