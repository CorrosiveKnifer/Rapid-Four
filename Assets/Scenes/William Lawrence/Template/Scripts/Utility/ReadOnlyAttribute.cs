using UnityEngine;
using System;
using System.Reflection;

[AttributeUsage (AttributeTargets.Field, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer (typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
	public override void OnGUI(Rect rect, UnityEditor.SerializedProperty _prop, GUIContent _label)
	{
		bool wasEnabled = GUI.enabled;
		GUI.enabled = false;
		UnityEditor.EditorGUI.PropertyField(rect, _prop);
		GUI.enabled = wasEnabled;
	}
}
#endif