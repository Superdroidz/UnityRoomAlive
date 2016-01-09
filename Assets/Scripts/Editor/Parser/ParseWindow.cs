using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class ParseWindow : EditorWindow {

    private static string windowTitle = "XML File";
    private static int buttonWidth = 130;
    private string xmlFilePath;

    XmlNodeList kinectsNames;
    XmlNodeList kinectsIP;
    XmlNodeList projectorsNames;
    XmlNodeList projectorsIP;
    XmlNodeList projectorsDisplay;

    XmlDocument doc;

    XmlNodeList kinectDefault;
    XmlNodeList projectorDefault;

    string newKinectsName;
    string newKinectsIP;
    string newProjectorsName;
    string newProjectorsIP;
    string newProjectorsDisplay;

    bool addingKinect = false;
    bool addingProjector = false;
    bool removingKinect = false;
    bool removingProjector = false;

    int kinectNum;
    int projectorNum;

    public void setFilePath(string filePath)
    {
        xmlFilePath = filePath;
    }

    public void ParseFile()
    {
        doc = new XmlDocument();
        doc.Load(xmlFilePath);

        XmlNodeList kinects = doc.GetElementsByTagName("cameras");
        string kinect = kinects[0].OuterXml;
        kinectsNames = getTags(kinect, "name");
        kinectsIP = getTags(kinect, "hostNameOrAddress");

        XmlNodeList projectors = doc.GetElementsByTagName("projectors");
        string project = projectors[0].OuterXml;
        projectorsNames = getTags(project, "name");
        projectorsIP = getTags(project, "hostNameOrAddress");
        projectorsDisplay = getTags(project, "displayIndex");

        XmlNodeList poses = doc.GetElementsByTagName("pose");
        XmlNodeList ensemble = doc.GetElementsByTagName("ProjectorCameraEnsemble");
        
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
                    tempNames[index+1].InnerText = projectorsNames[i].InnerText;
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
            doc.Save(xmlFilePath);

    }

    public void AddKinect()
    {
        XmlNodeList tempKinect = doc.GetElementsByTagName("cameras");
        string temp = "ProjectorCameraEnsemble.Camera";

        XmlNodeList ensemble = doc.GetElementsByTagName("ProjectorCameraEnsemble");
        string nameSpace = ensemble[0].NamespaceURI;

        string mainAtt = ensemble[0].Attributes[0].InnerText;

        XmlElement elem = doc.CreateElement(temp, nameSpace);
        XmlElement cal = doc.CreateElement("calibration", nameSpace);
        XmlAttribute att = doc.CreateAttribute("i", "nil", mainAtt);
        att.InnerText = "true";
        cal.Attributes.Append(att);
        XmlElement IP = doc.CreateElement("hostNameOrAddress", nameSpace);
        IP.InnerText = newKinectsIP;
        XmlElement name = doc.CreateElement("name", nameSpace);
        name.InnerText = newKinectsName;
        XmlElement pose = doc.CreateElement("pose", nameSpace);
        att = doc.CreateAttribute("i", "nil", mainAtt);
        att.InnerText = "true";
        pose.Attributes.Append(att);
        
        elem.AppendChild(cal);
        elem.AppendChild(IP);
        elem.AppendChild(name);
        elem.AppendChild(pose);

        tempKinect[tempKinect.Count-1].AppendChild(elem);

        doc.Save(xmlFilePath);

    }
    public void RemoveKinect()
    {
        XmlNodeList tempKinect = doc.GetElementsByTagName("cameras");
        tempKinect[0].RemoveChild(tempKinect[0].ChildNodes[kinectNum]);

        doc.Save(xmlFilePath);
    }
    public void AddProjector()
    {
        XmlNodeList tempProjector = doc.GetElementsByTagName("projectors");
        string temp = "ProjectorCameraEnsemble.Projector";

        XmlNodeList ensemble = doc.GetElementsByTagName("ProjectorCameraEnsemble");
        string nameSpace = ensemble[0].NamespaceURI;

        string mainAtt = ensemble[0].Attributes[0].InnerText;

        XmlElement elem = doc.CreateElement(temp, nameSpace);

        XmlElement camMatrix = doc.CreateElement("cameraMatrix", nameSpace);
        XmlAttribute att = doc.CreateAttribute("i", "nil", mainAtt);
        att.InnerText = "true";
        camMatrix.Attributes.Append(att);
        XmlElement display = doc.CreateElement("displayIndex", nameSpace);
        display.InnerText = newProjectorsDisplay;
        XmlElement height = doc.CreateElement("height", nameSpace);
        height.InnerText = "0";
        XmlElement host = doc.CreateElement("hostNameOrAddress", nameSpace);
        host.InnerText = newProjectorsIP;
        XmlElement lensDistort = doc.CreateElement("lensDistortion", nameSpace);
        att = doc.CreateAttribute("i", "nil", mainAtt);
        att.InnerText = "true";
        lensDistort.Attributes.Append(att);
        XmlElement lockIntrinsics = doc.CreateElement("lockIntrinsics", nameSpace);
        lockIntrinsics.InnerText = "false";
        XmlElement name = doc.CreateElement("name", nameSpace);
        name.InnerText = newProjectorsName;
        XmlElement pose = doc.CreateElement("pose", nameSpace);
        att = doc.CreateAttribute("i", "nil", mainAtt);
        att.InnerText = "true";
        pose.Attributes.Append(att);
        XmlElement width = doc.CreateElement("width", nameSpace);
        width.InnerText = "0";

        elem.AppendChild(camMatrix);
        elem.AppendChild(display);
        elem.AppendChild(height);
        elem.AppendChild(host);
        elem.AppendChild(lensDistort);
        elem.AppendChild(lockIntrinsics);
        elem.AppendChild(name);
        elem.AppendChild(pose);
        elem.AppendChild(width);

        tempProjector[tempProjector.Count - 1].AppendChild(elem);

        doc.Save(xmlFilePath);
    }
    public void RemoveProjector()
    {
        XmlNodeList tempKinect = doc.GetElementsByTagName("projectors");
        tempKinect[0].RemoveChild(tempKinect[0].ChildNodes[projectorNum]);

        doc.Save(xmlFilePath);
    }

    public void ShowWindow()
    {
       
        ParseWindow window = (ParseWindow)EditorWindow.GetWindow(typeof(ParseWindow)); //Creates Window;
        GUIContent titleContent = new GUIContent();
        titleContent.text = Path.GetFileName(xmlFilePath);
        window.titleContent = titleContent;
        window.Show();
    }

    void OnGUI()
    {
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

            if (GUILayout.Button("Add Kinect", GUILayout.Width(buttonWidth)))
            {
                addingKinect = true;
                addingProjector = false;
                removingKinect = false;
                removingProjector = false;
            }
            if (GUILayout.Button("Remove Kinect", GUILayout.Width(buttonWidth)))
            {
                addingKinect = false;
                addingProjector = false;
                removingKinect = true;
                removingProjector = false;
            }
            if (GUILayout.Button("Add Projector", GUILayout.Width(buttonWidth)))
            {
                addingKinect = false;
                addingProjector = true;
                removingKinect = false;
                removingProjector = false;
            }
            if (GUILayout.Button("Remove Projector", GUILayout.Width(buttonWidth)))
            {
                addingKinect = false;
                addingProjector = false;
                removingKinect = false;
                removingProjector = true;
            }
            if (addingKinect)
            {

                EditorGUILayout.BeginHorizontal();
                newKinectsName = EditorGUILayout.TextField("New Kinect's Name: ", newKinectsName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newKinectsIP = EditorGUILayout.TextField("New Kinect's IP: ", newKinectsIP);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Add", GUILayout.Width(buttonWidth)) && newKinectsName.Length > 0 && newKinectsIP.Length > 0)
                {
                    addingKinect = false;
                    AddKinect();
                    ParseFile();
                }
            }
            if (removingKinect)
            {

                EditorGUILayout.BeginHorizontal();
                kinectNum = int.Parse(EditorGUILayout.TextField("Remove Kinect Number: ", kinectNum+""));
                EditorGUILayout.EndHorizontal();
                
                if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)) && kinectNum > 0 && kinectNum < kinectsNames.Count)
                {
                    removingKinect = false;
                    RemoveKinect();
                    ParseFile();
                }
            }
            if (addingProjector)
            {

                EditorGUILayout.BeginHorizontal();
                newProjectorsName = EditorGUILayout.TextField("New Projector's Name: ", newProjectorsName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newProjectorsIP = EditorGUILayout.TextField("New Projector's IP: ", newProjectorsIP);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newProjectorsDisplay = EditorGUILayout.TextField("New Projector's Display Index: ", newProjectorsDisplay);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Add", GUILayout.Width(buttonWidth)) && newProjectorsName.Length > 0 && newProjectorsIP.Length > 0 && newProjectorsDisplay.Length > 0)
                {
                    addingProjector = false;
                    AddProjector();
                    ParseFile();
                }
            }
            if (removingProjector)
            {

                EditorGUILayout.BeginHorizontal();
                projectorNum = int.Parse(EditorGUILayout.TextField("Remove Projector Number: ", projectorNum + ""));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)) && projectorNum < projectorsNames.Count)
                {
                    removingProjector = false;
                    RemoveProjector();
                    ParseFile();
                }
            }
        
    }
}
