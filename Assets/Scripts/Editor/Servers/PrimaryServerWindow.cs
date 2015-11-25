using UnityEngine;
using UnityEditor;
using System.Collections;

public class PrimaryServerWindow : EditorWindow {
	
	private static string windowTitle = "Primary Server";

	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	public PrimaryServerWindow(){
	}

	public void ShowWindow()
	{
		PrimaryServerWindow window = (PrimaryServerWindow)EditorWindow.GetWindow(typeof(PrimaryServerWindow)); //Creates Window;
		GUIContent titleContent = new GUIContent();
		titleContent.text = windowTitle;
		window.titleContent = titleContent;
		window.Show();
	}
	
	void OnGUI () {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Text Field", myString);
		
		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup ();
	}
}
