using UnityEngine;
using UnityEditor;

/// <summary>
/// Colors an object name in hierarchy.
/// Makes it easier and more beautiful to organize objects in your scene.
/// Sample Usage: #red ENVIRONMENT
/// </summary>
[InitializeOnLoad]
public static class HierarchyHeaderColorant
{
    static HierarchyHeaderColorant()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("#", System.StringComparison.Ordinal))
        {
            // format is #color
            var colorName = gameObject.name.Substring(1).Split(' ')[0];
            // convert colorname to unity color
            var color = Color.gray;
            if (ColorUtility.TryParseHtmlString(colorName.ToLower(), out var _color))
            {
                color = _color;
            }

            EditorGUI.DrawRect(selectionRect, color);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("#" + colorName + " ", "").ToUpperInvariant());
        }
    }
}
