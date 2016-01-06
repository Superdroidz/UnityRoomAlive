using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ParseWindow : EditorWindow {

    private static string windowTitle = "XML File";
    private static int buttonWidth = 130;

    XmlNodeList kinectsNames;
    
    XmlNodeList kinectsIP;
    XmlNodeList projectorsNames;
    XmlNodeList projectorsIP;
    XmlNodeList projectorsDisplay;

    bool open = true;

    void ParseFile()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/XML File/cal.xml");

        XmlNodeList kinects = doc.GetElementsByTagName("cameras");
        string kinect = "<doc>" + kinects[0].InnerXml + "</doc>";
        kinectsNames = getTags(kinect, "name");
        kinectsIP = getTags(kinect, "hostNameOrAddress");

        XmlNodeList projectors = doc.GetElementsByTagName("projectors");
        string project = "<doc>" + projectors[0].InnerXml + "</doc>";
        projectorsNames = getTags(project, "name");
        projectorsIP = getTags(project, "hostNameOrAddress");
        projectorsDisplay = getTags(project, "displayIndex");
    }

    XmlNodeList getTags(string xml, string tag)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        XmlNodeList results = doc.GetElementsByTagName(tag);
        return results;
    }

    public void ShowWindow()
    {
        ParseWindow window = (ParseWindow)EditorWindow.GetWindow(typeof(ParseWindow)); //Creates Window;
        GUIContent titleContent = new GUIContent();
        titleContent.text = windowTitle;
        window.titleContent = titleContent;
        window.Show();
    }

    void OnGUI()
    {
        
        if (open)
        {
            open = false;
            ParseFile();
        }
            for (int i = 0; i < kinectsNames.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                kinectsNames[i].InnerText = EditorGUILayout.TextField("Kinect " + (i) + "'s Name: ", kinectsNames[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                kinectsIP[i].InnerText = EditorGUILayout.TextField("Kinect " + (i) + "'s IP: ", kinectsIP[i].InnerText);
                EditorGUILayout.EndHorizontal();
            }


            for (int i = 0; i < projectorsNames.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                projectorsNames[i].InnerText = EditorGUILayout.TextField("Projector " + (i) + "'s Name: ", projectorsNames[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                projectorsIP[i].InnerText = EditorGUILayout.TextField("Projector " + (i) + "'s IP: ", projectorsIP[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                projectorsDisplay[i].InnerText = EditorGUILayout.TextField("Projector " + (i) + "'s Display Index: ", projectorsDisplay[i].InnerText);
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Parse XML File", GUILayout.Width(buttonWidth)))
            {
                ParseFile();
            };
        
    }
}
