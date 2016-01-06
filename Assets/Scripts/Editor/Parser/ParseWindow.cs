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

    

    XmlDocument doc;

    bool open = true;

    void ParseFile()
    {
        doc = new XmlDocument();
        doc.Load("Assets/XML File/cal.xml");

        XmlNodeList kinects = doc.GetElementsByTagName("cameras");
        string kinect = kinects[0].OuterXml;
        kinectsNames = getTags(kinect, "name");
        kinectsIP = getTags(kinect, "hostNameOrAddress");

        XmlNodeList projectors = doc.GetElementsByTagName("projectors");
        string project = projectors[0].OuterXml;
        projectorsNames = getTags(project, "name");
        projectorsIP = getTags(project, "hostNameOrAddress");
        projectorsDisplay = getTags(project, "displayIndex");

        
    }

    XmlNodeList getTags(string xml, string tag)
    {
        XmlDocument temp = new XmlDocument();
        temp.LoadXml(xml);
        XmlNodeList results = temp.GetElementsByTagName(tag);
        return results;
    }

    void SaveChanges()
    {
        XmlNodeList tempNames = doc.GetElementsByTagName("name");
        XmlNodeList tempIP = doc.GetElementsByTagName("hostNameOrAddress");
        XmlNodeList tempDisplay = doc.GetElementsByTagName("displayIndex");
        int count = 0;
        for (int i = 0; i < kinectsNames.Count; i++)
            {
                if (kinectsNames[i].InnerText.Length > 0)
                {
                    tempNames[i].InnerText = kinectsNames[i].InnerText;
                }
                if (kinectsIP[i].InnerText.Length > 0)
                {
                    tempIP[i].InnerText = kinectsIP[i].InnerText;
                }
                count++;
            }
        
            for (int i = 0; i < projectorsNames.Count; i++)
            {
                int index = i + count;
                if(projectorsNames[i].InnerText.Length > 0) {
                    Debug.Log("original:" + tempNames[index].InnerText);
                    Debug.Log("new:" + projectorsNames[i].InnerText);
                    tempNames[index+1].InnerText = projectorsNames[i].InnerText;
                    Debug.Log(tempNames[index].InnerText);
                }
                if (projectorsIP[i].InnerText.Length > 0)
                {
                    tempIP[index].InnerText = projectorsIP[i].InnerText;
                }
                if (projectorsDisplay[i].InnerText.Length > 0)
                {
                    tempDisplay[i].InnerText = projectorsDisplay[i].InnerText;
                }
            }
            doc.Save("Assets/XML File/cal.xml");

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
        if (kinectsNames != null) {
            for (int i = 0; i < kinectsNames.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                kinectsNames[i].InnerText = EditorGUILayout.TextField("Kinect " + (i) + "'s Name: ", kinectsNames[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                kinectsIP[i].InnerText = EditorGUILayout.TextField("Kinect " + (i) + "'s IP: ", kinectsIP[i].InnerText);
                EditorGUILayout.EndHorizontal();
            }
    }

        if (projectorsNames != null)
        {
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
        }

            if (GUILayout.Button("Parse XML File", GUILayout.Width(buttonWidth)))
            {
                ParseFile();
            };

            if (GUILayout.Button("Save Changes", GUILayout.Width(buttonWidth)))
            {
                SaveChanges();
            };
        
    }
}
