using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TextGroupList))]
public class LevelDataEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("rows"),
                true, true, true, true);

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 250, rect.y, rect.width - 250, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("key"), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}