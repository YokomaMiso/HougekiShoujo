using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// �^�O�̐�pUI��\�������邽�߂̑���
[AttributeUsage(AttributeTargets.Field)]
public class TagAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
/// �^�O���̐�pUI��\�������邽�߂�PropertyDrawer
[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �Ώۂ̃v���p�e�B�������񂩂ǂ���
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // �^�O�t�B�[���h��\��
        var tag = EditorGUI.TagField(position, label, property.stringValue);

        // �^�O���𔽉f
        property.stringValue = tag;
    }
}
#endif