using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;            

		// プロパティ名のラベルを描画し、ラベル以外のrectを返す
		Rect rect_content = EditorGUI.PrefixLabel(position, label);

		// ボタンを描画
		if (GUI.Button(rect_content, buttonAttribute.buttonName))
		{
			try
			{
				// メソッド名からメソッドを取得、実行
				MethodInfo method = property.serializedObject.targetObject.GetType().GetMethod(buttonAttribute.methodName);
				method.Invoke(property.serializedObject.targetObject, null);
			}
			catch
			{
				Debug.Log(buttonAttribute.methodName + " を実行できません");
			}

		}
		

	}

}