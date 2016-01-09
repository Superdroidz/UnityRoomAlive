using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class SettingsWindow : EditorWindow {

    private static string windowTitle = "RoomAlive Settings";
    private static int buttonWidth = 130;
    private string kinectServerPath;
    private string projectorServerPath;
    private string consoleApplicationPath;

    public string KinectServerPath
    {
        get
        {
            return kinectServerPath;
        }

        set
        {
            kinectServerPath = value;
        }
    }

    public string ProjectorServerPath
    {
        get
        {
            return projectorServerPath;
        }

        set
        {
            projectorServerPath = value;
        }
    }

    public string ConsoleApplicationPath
    {
        get
        {
            return consoleApplicationPath;
        }

        set
        {
            consoleApplicationPath = value;
        }
    }

    public void ShowWindow()
    {
        SettingsWindow window = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        GUIContent titleContent = new GUIContent();
        titleContent.text = windowTitle;
        window.titleContent = titleContent;
        window.Show();
    }

    void OnGUI()
    {
        //Kinect Server Destination Section
        EditorGUILayout.BeginHorizontal();
        kinectServerPath = EditorGUILayout.TextField("Kinect Server Path: ", kinectServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            kinectServerPath = EditorUtility.OpenFilePanel("Select Kinect Server", "", "exe");
        }
        EditorGUILayout.EndHorizontal();
        
        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        projectorServerPath = EditorGUILayout.TextField("Projector Server Path: ", projectorServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            projectorServerPath = EditorUtility.OpenFilePanel("Select Projector Server", "", "exe");
        }
        EditorGUILayout.EndHorizontal();

        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        consoleApplicationPath = EditorGUILayout.TextField("Projector Server Path: ", consoleApplicationPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            consoleApplicationPath = EditorUtility.OpenFilePanel("Select Console Application", "", "exe");
        }
        EditorGUILayout.EndHorizontal();

    }

}
