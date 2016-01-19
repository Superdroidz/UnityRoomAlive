using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SettingsWindow : EditorWindow {

    // Class for storing settings. The constructor is used in OnGUI for first
    // initialization.
    [Serializable]
    public class SettingsData {

        public string kinectServerPath;
        public string projectorServerPath;
        public string consoleApplicationPath;
        public bool isTrackingHead;

        public SettingsData() {
            kinectServerPath = "";
            projectorServerPath = "";
            consoleApplicationPath = "";
            isTrackingHead = false;
        }
    }

    private static string windowTitle = "RoomAlive Settings";
    private static int buttonWidth = 130;
    private static string settingsFilePath = Application.dataPath + "/settings.xml";

    public static SettingsData Settings { get; private set; }

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
        if (Settings == null) {
            Settings = new SettingsData();
        }

        //Kinect Server Destination Section
        EditorGUILayout.BeginHorizontal();
        Settings.kinectServerPath = EditorGUILayout.TextField("Kinect Server Path: ", Settings.kinectServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            Settings.kinectServerPath = EditorUtility.OpenFilePanel("Select Kinect Server", "", "exe");
        }
        EditorGUILayout.EndHorizontal();
        
        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        Settings.projectorServerPath = EditorGUILayout.TextField("Projector Server Path: ", Settings.projectorServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            Settings.projectorServerPath = EditorUtility.OpenFilePanel("Select Projector Server", "", "exe");
        }
        EditorGUILayout.EndHorizontal();

        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        Settings.consoleApplicationPath = EditorGUILayout.TextField("Console Application Path: ", Settings.consoleApplicationPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            Settings.consoleApplicationPath = EditorUtility.OpenFilePanel("Select Console Application", "", "exe");
        }
        EditorGUILayout.EndHorizontal();

        //Whether to use head tracking Section
        EditorGUILayout.BeginHorizontal();
        Settings.isTrackingHead = EditorGUILayout.Toggle("Use head tracking: ", Settings.isTrackingHead);
        EditorGUILayout.EndHorizontal();

        //Saving settings to XML Button
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Settings", GUILayout.Width(buttonWidth)))
        {
            SaveSettings();
        }

        //Loading settings from file
        if (GUILayout.Button("Load Settings", GUILayout.Width(buttonWidth)))
        {
            LoadSettings();
        }

        EditorGUILayout.EndHorizontal();
    }

    // Save settings to a JSON file
    private void SaveSettings()
    {
        string serializedSettings = JsonUtility.ToJson(Settings);
        File.WriteAllText(settingsFilePath, serializedSettings);
    }

    // Load settings from a JSON file
    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string settingsText = File.ReadAllText(settingsFilePath);
            Settings = JsonUtility.FromJson<SettingsData>(settingsText);
        }
    }
}
