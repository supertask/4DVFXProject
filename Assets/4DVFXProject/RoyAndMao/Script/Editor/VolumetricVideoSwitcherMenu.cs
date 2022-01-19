using UnityEngine;
using System.Collections;
using UnityEditor;

public class VolumetricVideoSwitcherMenu : MonoBehaviour {

	//　自前のメニューと項目を作成
	[MenuItem("4DVFXProject/項目1 %t", false, 1)]
	static void OutputConsole() {
		Debug.Log ("項目1が選択された");
	}

	[MenuItem("4DVFXProject/項目1 %t", true)]
	static bool IsValidate() {
		//　選択されているのがゲームオブジェクトか？
		return Selection.activeObject.GetType () == typeof(GameObject);
	}

	[MenuItem("4DVFXProject/項目2 #t", false, 12)]
	static void OutputConsole2() {
		Debug.Log ("項目2が選択された");
	}

	[MenuItem("4DVFXProject/項目3 %#&t", false, 13)]
	static void OutputConsole3() {
		Debug.Log ("項目3が選択された");
	}
}