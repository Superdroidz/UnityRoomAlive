using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class ParseWindow : EditorWindow {
    private static int buttonWidth = 130;
    private string xmlFilePath;

    //Current Values for each Kinect and Projector
    XmlNodeList kinectsNames;
    XmlNodeList kinectsIP;
    XmlNodeList projectorsNames;
    XmlNodeList projectorsIP;
    XmlNodeList projectorsDisplay;

    //Original Values for each Kinect and Projector
    //used to check for any pending changes that have been made within the ParseWindow
    XmlNodeList originalKinectsNames;
    XmlNodeList originalKinectsIP;
    XmlNodeList originalProjectorsNames;
    XmlNodeList originalProjectorsIP;
    XmlNodeList originalProjectorsDisplay;

    //Current XML document
    XmlDocument doc;

    //Values for the Kinect or Projector currently being added by the user
    string newKinectsName;
    string newKinectsIP;
    string newProjectorsName;
    string newProjectorsIP;
    string newProjectorsDisplay;

    //Boolean to track whether there are any pending changes the user has made within the ParseWindow
    bool changes = false;

    //Booleans to track whether the user is currently adding or removing a Kinect or Projector
    bool addingKinect = false;
    bool addingProjector = false;
    bool removingKinect = false;
    bool removingProjector = false;

    //Booleans to track the Foldouts and whether they are collapsed or not
    bool showCameras = false;
    List<bool> showEachCamera = new List<bool>();
    bool showProjectors = false;
    List<bool> showEachProjector = new List<bool>();

    //Numbers containing the index of the Kinect or Projector the user wishes to remove
    int kinectNum;
    int projectorNum;

    public void setFilePath(string filePath)
    {
        xmlFilePath = filePath;
    }
    public void setCurrentDoc(XmlDocument newDoc)
    {
        doc = newDoc;
    }
    public void setParseWindow(ParseWindow window)
    {
        //Preserve changes by setting all the global values of this ParseWindow
        //to the parameter ParseWindow's global values
        this.kinectsNames = window.kinectsNames;
        this.kinectsIP = window.kinectsIP;
        this.projectorsNames = window.projectorsNames;
        this.projectorsIP = window.projectorsIP;
        this.projectorsDisplay = window.projectorsDisplay;

        this.originalKinectsNames = window.originalKinectsNames;
        this.originalKinectsIP = window.originalKinectsIP;
        this.originalProjectorsNames = window.originalProjectorsIP;
        this.originalProjectorsIP = window.originalProjectorsIP;
        this.originalProjectorsDisplay = window.originalProjectorsDisplay;

        this.newKinectsName = window.newKinectsName;
        this.newKinectsIP = window.newKinectsIP;
        this.newProjectorsName = window.newProjectorsName;
        this.newProjectorsIP = window.newProjectorsIP;
        this.newProjectorsDisplay = window.newProjectorsDisplay;

        this.changes = window.changes;

        this.addingKinect = window.addingKinect;
        this.addingProjector = window.addingProjector;
        this.removingKinect = window.removingKinect;
        this.removingProjector = window.removingProjector;

        this.showCameras = window.showCameras;
        this.showEachCamera = window.showEachCamera;
        this.showProjectors = window.showProjectors;
        this.showEachProjector = window.showEachProjector;

        this.kinectNum = window.kinectNum;
        this.projectorNum = window.projectorNum;
    }

    public void ParseFile()
    {
        //Get all the "camera" Elements in the XML file
        XmlNodeList kinects = doc.GetElementsByTagName("cameras");
        //As we know there is only one set of tags for "cameras", grab the first member
        string kinect = kinects[0].OuterXml;
        originalKinectsNames = getTags(kinect, "name");
        originalKinectsIP = getTags(kinect, "hostNameOrAddress");

        //Get all the "projector" Elements in the XML file
        XmlNodeList projectors = doc.GetElementsByTagName("projectors");
        //As we know there is only one set of tags for "projectors", grab the first member
        string project = projectors[0].OuterXml;
        originalProjectorsNames = getTags(project, "name");
        originalProjectorsIP = getTags(project, "hostNameOrAddress");
        originalProjectorsDisplay = getTags(project, "displayIndex");


        //Necessary to store values in secondary variables so that any changes can be reverted
        kinectsNames = getTags(kinect, "name"); ;
        kinectsIP = getTags(kinect, "hostNameOrAddress"); ;

        projectorsNames = getTags(project, "name");
        projectorsIP = getTags(project, "hostNameOrAddress");
        projectorsDisplay = getTags(project, "displayIndex");
        
    }
    public void LoadFile()
    {
        doc = new XmlDocument();
        doc.Load(xmlFilePath);
    }

    XmlNodeList getTags(string xml, string tag)
    {
        //This function treats the parameter xml as a document so that we may search within it
        //to find all the tags that match the parameter tag
        XmlDocument temp = new XmlDocument();
        temp.LoadXml(xml);
        XmlNodeList results = temp.GetElementsByTagName(tag);
        return results;
    }

    void SaveChanges()
    {
        //Get all the current values of the relevent elements in the existing document
        XmlNodeList tempNames = doc.GetElementsByTagName("name");
        XmlNodeList tempIP = doc.GetElementsByTagName("hostNameOrAddress");
        XmlNodeList tempDisplay = doc.GetElementsByTagName("displayIndex");
        int count = 0;
        //Change each camera element to its new value
        for (int i = 0; i < kinectsNames.Count; i++)
            {
                if (kinectsNames[i].InnerText.Length > 0)
                {
                    tempNames[i].InnerText = kinectsNames[i].InnerText;
                    originalKinectsNames[i].InnerText = kinectsNames[i].InnerText;
                }
                if (kinectsIP[i].InnerText.Length > 0)
                {
                    tempIP[i].InnerText = kinectsIP[i].InnerText;
                    originalKinectsIP[i].InnerText = kinectsIP[i].InnerText;
                }
                count++;
            }
        //Change each projector element to its new value
            for (int i = 0; i < projectorsNames.Count; i++)
            {
                int index = i + count;
                if(projectorsNames[i].InnerText.Length > 0) {
                    tempNames[index+1].InnerText = projectorsNames[i].InnerText;
                    originalProjectorsNames[i].InnerText = projectorsNames[i].InnerText;
                }
                if (projectorsIP[i].InnerText.Length > 0)
                {
                    tempIP[index].InnerText = projectorsIP[i].InnerText;
                    originalProjectorsIP[i].InnerText = projectorsIP[i].InnerText;
                }
                if (projectorsDisplay[i].InnerText.Length > 0)
                {
                    tempDisplay[i].InnerText = projectorsDisplay[i].InnerText;
                    originalProjectorsDisplay[i].InnerText = projectorsDisplay[i].InnerText;
                }
            }
            doc.Save(xmlFilePath);
    }

    public void AddKinect()
    {
        //To keep the new Kinect consistent with the existing Kinects, must mimic all fields namespace and values,
        //excluding the relevant fields so we can insert the desired values
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

    }
    public void RemoveKinect()
    {
        //Get all the current cameras in the existing document and remove the relevant camera
        XmlNodeList tempKinect = doc.GetElementsByTagName("cameras");
        tempKinect[0].RemoveChild(tempKinect[0].ChildNodes[kinectNum]);
    }
    public void AddProjector()
    {
        //To keep the new Projector consistent with the existing Projectors, must mimic all fields namespace and values,
        //excluding the relevant fields so we can insert the desired values
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
    }
    public void RemoveProjector()
    {
        //Get all the current projectors in the existing document and remove the relevant projector
        XmlNodeList tempKinect = doc.GetElementsByTagName("projectors");
        tempKinect[0].RemoveChild(tempKinect[0].ChildNodes[projectorNum]);
    }

    public void ShowWindow()
    {
       
        ParseWindow window = (ParseWindow)GetWindow(typeof(ParseWindow)); //Creates Window;
        GUIContent titleContent = new GUIContent();
        titleContent.text = Path.GetFileName(xmlFilePath);
        window.titleContent = titleContent;
        window.Show();
    }

    void OnGUI()
    {
        //While the ParseWindow is still open
        //Don't attempt any interactions with camera values if there isn't any cameras in the file
        if (kinectsNames != null) {
            showCameras = EditorGUILayout.Foldout(showCameras, "Cameras");
            EditorGUI.indentLevel++;
            if (showCameras) { //if the Camera Foldout has been collapsed
                for (int i = 0; i < kinectsNames.Count; i++) //for each Kinect
            {
                if (i == showEachCamera.Count) //if the collapse list has no record for this Kinect
                {
                    showEachCamera.Add(false);
                }
                showEachCamera[i] = EditorGUILayout.Foldout(showEachCamera[i], "Kinect " + (i));
                if (showEachCamera[i]) { //if this Kinect's Foldout has been collapsed
                    EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                kinectsNames[i].InnerText = EditorGUILayout.TextField("Name: ", kinectsNames[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                kinectsIP[i].InnerText = EditorGUILayout.TextField("IP: ", kinectsIP[i].InnerText);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

                if (!kinectsNames[i].InnerText.Equals(originalKinectsNames[i].InnerText)
                    || !kinectsIP[i].InnerText.Equals(originalKinectsIP[i].InnerText))
                { //if any changes have been made to any of the text fields for this Kinect
                    changes = true;
                }
            }
        }
            EditorGUI.indentLevel--;
    }
        //Don't attempt any interactions with projector values if there isn't any projectors in the file
        if (projectorsNames != null)
        {
             showProjectors = EditorGUILayout.Foldout(showProjectors, "Projectors");
             EditorGUI.indentLevel++;
             if (showProjectors) //if the Projector Foldout has been collapsed
             {
                 for (int i = 0; i < projectorsNames.Count; i++) //for each Projector
                 {
                     if (i == showEachProjector.Count) //if the collapse list has no record for this Projector
                {
                    showEachProjector.Add(false);
                }
                showEachProjector[i] = EditorGUILayout.Foldout(showEachProjector[i], "Projector " + (i));

                if (showEachProjector[i]) //if this Projector's Foldout has been collapsed
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    projectorsNames[i].InnerText = EditorGUILayout.TextField("Name: ", projectorsNames[i].InnerText);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    projectorsIP[i].InnerText = EditorGUILayout.TextField("IP: ", projectorsIP[i].InnerText);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    projectorsDisplay[i].InnerText = EditorGUILayout.TextField("Display Index: ", projectorsDisplay[i].InnerText);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                    if (!projectorsNames[i].InnerText.Equals(originalProjectorsNames[i].InnerText)
                        || !projectorsIP[i].InnerText.Equals(originalProjectorsIP[i].InnerText)
                            || !projectorsDisplay[i].InnerText.Equals(originalProjectorsDisplay[i].InnerText))
                    { //if any changes have been made to any of the text fields for this Projector
                        changes = true;

                    }
                 }
             }
             EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
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
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
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
            EditorGUILayout.EndHorizontal();
            if (addingKinect)
            {

                EditorGUILayout.BeginHorizontal();
                newKinectsName = EditorGUILayout.TextField("Kinect's Name: ", newKinectsName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newKinectsIP = EditorGUILayout.TextField("Kinect's IP: ", newKinectsIP);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Add", GUILayout.Width(buttonWidth)) && newKinectsName.Length > 0 && newKinectsIP.Length > 0)
                {
                    addingKinect = false;
                    changes = true;
                    AddKinect();
                    ParseFile();
                }
            }
            if (removingKinect)
            {

                EditorGUILayout.BeginHorizontal();
                kinectNum = int.Parse(EditorGUILayout.TextField("Kinect Number: ", kinectNum+""));
                EditorGUILayout.EndHorizontal();
                
                if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)) && kinectNum > 0 && kinectNum < kinectsNames.Count)
                {
                    removingKinect = false;
                    changes = true;
                    RemoveKinect();
                    ParseFile();
                }
            }
            if (addingProjector)
            {

                EditorGUILayout.BeginHorizontal();
                newProjectorsName = EditorGUILayout.TextField("Projector's Name: ", newProjectorsName);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newProjectorsIP = EditorGUILayout.TextField("Projector's IP: ", newProjectorsIP);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                newProjectorsDisplay = EditorGUILayout.TextField("Projector's Display Index: ", newProjectorsDisplay);
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Add", GUILayout.Width(buttonWidth)) && newProjectorsName.Length > 0 && newProjectorsIP.Length > 0 && newProjectorsDisplay.Length > 0)
                {
                    addingProjector = false;
                    changes = true;
                    AddProjector();
                    ParseFile();
                }
            }
            if (removingProjector)
            {

                EditorGUILayout.BeginHorizontal();
                projectorNum = int.Parse(EditorGUILayout.TextField("Projector Number: ", projectorNum + ""));
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove", GUILayout.Width(buttonWidth)) && projectorNum < projectorsNames.Count)
                {
                    removingProjector = false;
                    changes = true;
                    RemoveProjector();
                    ParseFile();
                }
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply", GUILayout.Width(buttonWidth)))
            {
                SaveChanges();
                changes = false;
            };
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Reset", GUILayout.Width(buttonWidth)))
            {
                LoadFile();
                ParseFile();
                changes = false;
            };
            EditorGUILayout.EndHorizontal();
    }

    void OnDestroy()
    {
        //When the user tries to close the ParseWindow
        if (changes)
        {
            int popup = EditorUtility.DisplayDialogComplex("Save Changes?", "You have made unfinished changes to the XML file. Would you like to save your changes?", "Yes", "No", "Cancel");
            
            if (popup == 0) //if the user selects "Yes"
            {
                SaveChanges();
            }
            //if the user selects "No", proceed to close the window without any further action
            else if (popup == 2) //if the user selects "Cancel"
            {
                //Create an identical ParseWindow that will remain after this ParseWindow closes
                //allowing all changes to be preserved
                ParseWindow newWindow = (ParseWindow)ScriptableObject.CreateInstance("ParseWindow");
                newWindow.setFilePath(xmlFilePath);
                newWindow.setCurrentDoc(doc);
                newWindow.setParseWindow(this);
                newWindow.ShowWindow();
            }
        }
    }
}
