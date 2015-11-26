using UnityEngine;
using UnityEditor;
using System.Collections;

public class PrimaryServerWindow : EditorWindow {
	
	private static string windowTitle = "Primary Server";
	private static int buttonWidth = 130;
	
	string localKinects = "0";
	string localProjectors = "0";

	string totalKinects = "0";
	string totalProjectors = "0";

	public PrimaryServerWindow(){
	}

	private static void StartProjectorServers()
	{

	}

	private static void StartKinectServers()
	{

	}

	private static void FindKinects()
	{

	}

	private static void FindProjectors()
	{

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
		//Local Kinect Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.TextField ("Local Kinects: ", localKinects);
		if (GUILayout.Button("Start Kinect Server",GUILayout.Width(buttonWidth)))
		{
			StartKinectServers();
		};
		EditorGUILayout.EndHorizontal();

		//Local Projectors Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.TextField ("Local Projectors: ", localProjectors);
		if (GUILayout.Button("Start Projector Server",GUILayout.Width(buttonWidth)))
		{
			StartProjectorServers();
		}
		EditorGUILayout.EndHorizontal();

		//Total Kinect Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Total Number of Kinects: ", totalKinects);
		if (GUILayout.Button("Find Kinects",GUILayout.Width(buttonWidth)))
		{
			FindKinects();
		}
		EditorGUILayout.EndHorizontal();

		//Total Projectors Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Total Number of Projectors: ", totalProjectors);
		if (GUILayout.Button("Find Projectors", GUILayout.Width(buttonWidth)))
		{
			FindProjectors();
		}
		EditorGUILayout.EndHorizontal();
	}
}
