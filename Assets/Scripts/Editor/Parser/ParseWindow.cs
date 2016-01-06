using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Xml;

public class ParseWindow : EditorWindow {

    private static string windowTitle = "XML File";
    private static int buttonWidth = 130;

    XmlNodeList kinects;
    XmlNodeList projectors;
    XmlNodeList names;

    string localKinects = "0";
    string localProjectors = "0";

    string totalKinects = "0";
    string totalProjectors = "0";

    void ParseFile()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("Assets/XML File/cal.xml");

        kinects = doc.GetElementsByTagName("cameras");
        string kinect = "<doc>" + kinects[0].InnerXml + "</doc>";
        kinects = getNames(kinect);

        projectors = doc.GetElementsByTagName("projectors");
        string project = "<doc>" + projectors[0].InnerXml + "</doc>";
        projectors = getNames(project);
    }

    XmlNodeList getNames(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        XmlNodeList results = doc.GetElementsByTagName("name");
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
        ParseFile();
        
        foreach (XmlNode kinect in kinects)
        {
            EditorGUILayout.BeginHorizontal();
            localKinects = EditorGUILayout.TextField("Kinect Name: ", kinect.InnerText);
            EditorGUILayout.EndHorizontal();
        }
        
        
        foreach (XmlNode projector in projectors)
        {
            EditorGUILayout.BeginHorizontal();
            localKinects = EditorGUILayout.TextField("Projector Name: ", projector.InnerText);
            EditorGUILayout.EndHorizontal();
        }
        
        
    }
}
