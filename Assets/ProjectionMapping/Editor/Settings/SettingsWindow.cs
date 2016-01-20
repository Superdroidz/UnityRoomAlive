using System;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SettingsWindow : EditorWindow {

    // Class for storing settings. The constructor is used in OnGUI for first
    // initialization.
    [Serializable]
    public class SettingsData {
        public string KinectServerPath;
        public string ProjectorServerPath;
        public string ConsoleApplicationPath;
        public bool IsTrackingHead;
        public bool IsVerbose;
        public bool IsDebug;

        public SettingsData() {
            KinectServerPath = "";
            ProjectorServerPath = "";
            ConsoleApplicationPath = "";
        }
    }

    private static string windowTitle = "RoomAlive Settings";
    private static int buttonWidth = 130;
    private static string settingsFilePath = Application.dataPath + "/settings.xml";

    private static SettingsData settings;
    public static SettingsData Settings
    {
        get
        {
            if (settings == null) {
                settings = new SettingsData();
                LoadSettings();
            }
            return settings;
        }
        private set
        {
            settings = value;
        }
    }

    public void ShowWindow()
    {
        var window = (SettingsWindow)GetWindow(typeof(SettingsWindow));
        var titleContent = new GUIContent
        {
            text = windowTitle
        };
        window.titleContent = titleContent;
        window.Show();
    }

    void OnGUI()
    {
        string tempPath;

        //Kinect Server Destination Section
        EditorGUILayout.BeginHorizontal();
        tempPath = EditorGUILayout.TextField("Kinect Server Path: ", Settings.KinectServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            tempPath = EditorUtility.OpenFilePanel("Select Kinect Server", "", "exe");
        }
        if (File.Exists(tempPath))
        {
            Settings.KinectServerPath = tempPath;
        }
        EditorGUILayout.EndHorizontal();

        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        tempPath = EditorGUILayout.TextField("Projector Server Path: ", Settings.ProjectorServerPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            tempPath = EditorUtility.OpenFilePanel("Select Projector Server", "", "exe");
        }
        if (File.Exists(tempPath))
        {
            Settings.ProjectorServerPath = tempPath;
        }
        EditorGUILayout.EndHorizontal();

        //Projector Server Destination Section
        EditorGUILayout.BeginHorizontal();
        tempPath = EditorGUILayout.TextField("Console Application Path: ", Settings.ConsoleApplicationPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            tempPath = EditorUtility.OpenFilePanel("Select Console Application", "", "exe");
        }
        if (File.Exists(tempPath))
        {
            Settings.ConsoleApplicationPath = tempPath;
        }
        EditorGUILayout.EndHorizontal();

        //Whether to use head tracking Section
        EditorGUILayout.BeginHorizontal();
        Settings.IsTrackingHead = EditorGUILayout.Toggle("Use head tracking:", Settings.IsTrackingHead);
        EditorGUILayout.EndHorizontal();

        //Show verbose messages?
        EditorGUILayout.BeginHorizontal();
        Settings.IsVerbose = EditorGUILayout.Toggle("Verbose console output:", Settings.IsVerbose);
        EditorGUILayout.EndHorizontal();

        //Show Debug messages?
        EditorGUILayout.BeginHorizontal();
        Settings.IsDebug = EditorGUILayout.Toggle("Enable debug messages:", Settings.IsDebug);
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
    private static void SaveSettings()
    {
        var serializedSettings = JsonUtility.ToJson(Settings);
        File.WriteAllText(settingsFilePath, serializedSettings);
    }

    // Load settings from a JSON file
    private static void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            var settingsText = File.ReadAllText(settingsFilePath);
            Settings = JsonUtility.FromJson<SettingsData>(settingsText);
        }
    }
}
