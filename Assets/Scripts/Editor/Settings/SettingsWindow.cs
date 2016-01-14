using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class SettingsWindow : EditorWindow
{

    class SettingsData
    {
        private string kinectServerPath;
        private string projectorServerPath;
        private string consoleApplicationPath;

        public SettingsData(string kinectPath, string projectorPath, string consolePath)
        {
            kinectServerPath = kinectPath;
            projectorServerPath = projectorPath;
            consoleApplicationPath = consolePath;
        }
    }

    private static string windowTitle = "RoomAlive Settings";
    private static int buttonWidth = 130;
    private static string kinectServerPath;
    private static string projectorServerPath;
    private static string consoleApplicationPath;
    private static string settingsFilePath = Application.dataPath + "/settings.xml";
    private XmlDocument settings;

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

    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            settings = new XmlDocument();

            settings.Load(settingsFilePath);

            kinectServerPath = settings.GetElementsByTagName("KinectPath")[0].InnerText;
            projectorServerPath = settings.GetElementsByTagName("ProjectorPath")[0].InnerText;
            consoleApplicationPath = settings.GetElementsByTagName("ConsoleApplication")[0].InnerText;
        }
        else
        {
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
        consoleApplicationPath = EditorGUILayout.TextField("Console Application Path: ", consoleApplicationPath);
        if (GUILayout.Button("Browse", GUILayout.Width(buttonWidth)))
        {
            consoleApplicationPath = EditorUtility.OpenFilePanel("Select Console Application", "", "exe");
        }
        EditorGUILayout.EndHorizontal();

        //Saving settings to XML Button
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Settings", GUILayout.Width(buttonWidth)))
        {
            SettingsData settings = new SettingsData(kinectServerPath, kinectServerPath, consoleApplicationPath);

            XmlWriterSettings XMLsettings = new XmlWriterSettings();
            XMLsettings.Indent = true;

            using (FileStream fileStream = new FileStream(settingsFilePath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                UnityEngine.Debug.Log("Got to save the file");
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                writer.WriteElementString("KinectPath", kinectServerPath);
                writer.WriteElementString("ProjectorPath", projectorServerPath);
                writer.WriteElementString("ConsoleApplication", consoleApplicationPath);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        //Loading settings from file
        if (GUILayout.Button("Load Settings", GUILayout.Width(buttonWidth)))
        {
            LoadSettings();
        }

        EditorGUILayout.EndHorizontal();
    }

}