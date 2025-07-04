#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gaskellgames.EditorOnly
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    [CustomPropertyDrawer(typeof(Bool4))]
    public class Bool4Drawer : PropertyDrawer
    {
        #region variables

        private SerializedProperty x;
        private SerializedProperty y;
        private SerializedProperty z;
        private SerializedProperty w;
        
        private float singleLine = EditorGUIUtility.singleLineHeight;
        
        #endregion

        //----------------------------------------------------------------------------------------------------
        
        #region Property Height
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) - (singleLine * 1.1f);
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region OnGUI

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // open property and get reference to instance
            EditorGUI.BeginProperty(position, label, property);

            // get reference to SerializeFields
            x = property.FindPropertyRelative("x");
            y = property.FindPropertyRelative("y");
            z = property.FindPropertyRelative("z");
            w = property.FindPropertyRelative("w");
            
            // draw property
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label.text, label.tooltip));
            GUILayout.Space(2);
            x.boolValue = EditorGUILayout.ToggleLeft("X", x.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
            y.boolValue = EditorGUILayout.ToggleLeft("Y", y.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
            z.boolValue = EditorGUILayout.ToggleLeft("Z", z.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
            w.boolValue = EditorGUILayout.ToggleLeft("W", w.boolValue, GUILayout.Width(30), GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();

            // close property
            EditorGUI.EndProperty();
        }

        #endregion

    } // class end
}
#endif
