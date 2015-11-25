using UnityEngine;
using UnityEditor;
using System.Collections;

public class SecondaryServerWindow : EditorWindow {
	
	private static string windowTitle = "Secondary Server";
	
	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;
	
	public SecondaryServerWindow(){
	}
	
	public void ShowWindow()
	{
		SecondaryServerWindow window = (SecondaryServerWindow)EditorWindow.GetWindow(typeof(SecondaryServerWindow)); //Creates Window;
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
