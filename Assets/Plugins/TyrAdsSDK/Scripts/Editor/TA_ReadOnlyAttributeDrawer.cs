using Plugins.TyrAdsSDK.Scripts.UI;
using Plugins.TyrAdsSDK.Scripts.UI.Elements;
using UnityEditor;
using UnityEngine;

namespace TyrAdsSDK.Editor
{
    [CustomPropertyDrawer(typeof(TA_ReadOnlyAttribute))]
    public class TA_ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
#if  UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; 
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;  
        }
#endif
    }
}