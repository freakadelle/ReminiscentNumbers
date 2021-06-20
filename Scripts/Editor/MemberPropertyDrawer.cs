using UnityEngine;
using UnityEditor;
using MyEditorTools;

namespace Renumbrance.Editor
{
    [CustomPropertyDrawer(typeof(Member))]
    public class MemberPropertyDrawer : PropertyDrawer
    {
        SerializedProperty value, baseValue;

        Member propertyObject;

        /// <summary>
        /// Unity method for drawing GUI in Editor
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Member target = (Member)property.serializedObject.targetObject;

            //if (propertyObject == null)
                //propertyObject = (Member)EditorTools.GetTargetObjectOfProperty(property);

            EditorGUI.PropertyField(position, property, label, true);

            if (property.isExpanded)
            {
                //if (GUI.Button(new Rect(position.xMin + 20f, position.yMax - 18f, position.width - 30f, 20f), "Clear Modifications"))
                //{
                //    if (propertyObject != null)
                //        propertyObject.ClearModifications();
                //    //Debug.Log(property.serializedObject.targetObject.GetType().GetMethod("ClearModifications").Invoke(property.serializedObject.targetObject, null);
                //}
            } else
            {
                Rect newValueRect = position;
                newValueRect.xMin = 140;

                property.Next(true);
                value = property.Copy();

                var previousGUIState = GUI.enabled;
                GUI.enabled = false;

                EditorGUI.DoubleField(newValueRect, value.doubleValue);

                GUI.enabled = previousGUIState;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
                return EditorGUI.GetPropertyHeight(property) + 22f;
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}