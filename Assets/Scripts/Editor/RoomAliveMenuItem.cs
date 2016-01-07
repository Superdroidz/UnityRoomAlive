using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System;

public class RoomAliveMenuItem : EditorWindow{
    public static ParseWindow ParseWindow;

    private static bool fileSetupComplete = false;
    private static bool calibrationComplete = false;

    [MenuItem("Custom Editor/Add Mesh Colliders %m")]
    private static void PopulateMeshColliders()
    {
        GameObject selection = Selection.activeGameObject;
        AddMeshToObjectOrChildren(selection);
    }

    private static void AddMeshToObjectOrChildren(GameObject selection)
    {
        if (selection.GetComponent<MeshFilter>() != null &&
            selection.GetComponent<Collider>() == null)
        {
            var filter = selection.GetComponent<MeshFilter>().sharedMesh;
            MeshCollider collider = selection.AddComponent<MeshCollider>();
            collider.sharedMesh = filter;
            UnityEngine.Debug.Log("Added MeshCollider to object " + selection.name);
        }
        else
        {
            foreach (Transform child in selection.transform)
            {
                AddMeshToObjectOrChildren(child.gameObject);
            }
        }
    }

    [MenuItem("RoomAlive/Start Kinect Server", false, 1)]
    private static void RunKinectServer()
    {
        System.Diagnostics.Process.Start("C:\\Users\\Adam\\Desktop\\3rdYearProject\\UnityExtension\\RoomAlive\\RoomAliveToolkit-master\\ProCamCalibration\\KinectServer\\bin\\Debug\\KinectServer.exe");
    }

    [MenuItem("RoomAlive/Start Projector Server", false, 2)]
    private static void RunProjectorServer()
    {
        System.Diagnostics.Process.Start("C:\\Users\\Adam\\Desktop\\3rdYearProject\\UnityExtension\\RoomAlive\\RoomAliveToolkit-master\\ProCamCalibration\\ProjectorServer\\bin\\Debug\\ProjectorServer.exe");

    }

    [MenuItem("RoomAlive/Create New Setup", false, 51)]
    private static void CreateSetup()
    {
        fileSetupComplete = false;
        calibrationComplete = false;
        //Discover other Kinect Servers and Projector Servers
        //Create XML file with modified ip address, names and display indexes.
        fileSetupComplete = true;
    }

    [MenuItem("RoomAlive/Run Calibration", false, 101)]// Requires Validation
    private static void Calibrate()
    {
        //Run Acquire
        //Run Solve
        calibrationComplete = true;
    }

    [MenuItem("RoomAlive/Run Calibration", true)]
    private static bool CalibrationValidation()
    {
        return fileSetupComplete;
    }
    
    [MenuItem("RoomAlive/Import Room",false, 151)]
    private static void ImportRoom()
    {
        EditorApplication.ExecuteMenuItem("Assets/Import New Asset...");
    }

    [MenuItem("RoomAlive/Import Room", true)]
    private static bool ImportRoomValidation()
    {
        return calibrationComplete;
    }



    [MenuItem("RoomAlive/Parse/XML", false, 151)]
    private static void ParseXML()
    {
        ParseWindow = (ParseWindow)ScriptableObject.CreateInstance("ParseWindow");
        ParseWindow.ShowWindow();

    }
}
