using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GUIEditMode))]
public class GUIEditModeEditor : Editor {

	GUIEditMode editScript;

	void OnEnable()
	{
		editScript = (GUIEditMode)target;
	}

	void OnSceneGUI()
	{
		Handles.BeginGUI ();
		if (editScript != null) {
			if (GUI.Button (new Rect (0, 0, 200, 50), ""+editScript._State))
				editScript.NextState();

			if (GUI.Button (new Rect (Screen.width - 200, 0, 200, 50), "Save"))
				editScript.DoSave ();
			if (GUI.Button (new Rect (Screen.width - 200, 50, 200, 50), "Show"))
				editScript.DoShow ();
		}
		Handles.EndGUI ();
		SceneView.RepaintAll ();
	}
}
