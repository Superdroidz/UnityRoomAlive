using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using sd = System.Diagnostics;
using Assets.Parsing;

public class RoomAliveMenuItem : EditorWindow{
    public static ParseWindow ParseWindow;
    public static SettingsWindow SettingsWindow;

    private static string currentXMLFilePath;

    private static bool fileSetupComplete = false;
    private static bool calibrationComplete = false;
    private static bool fileLoaded = false;

    static sd.Process cameraServer, projectorServer;

    static sd.Process ProcessStart(string filepath)
    {
        return ProcessStart(filepath, "");
    }

    static sd.Process ProcessStart(string filepath, string args)
    {
        /*
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = processPath;
        startInfo.Arguments = args;
        //startInfo.RedirectStandardOutput = false;
        //startInfo.RedirectStandardError = false;
        //startInfo.UseShellExecute = false;
        //startInfo.CreateNoWindow = true;
        startInfo.WindowStyle = ProcessWindowStyle.Minimized;

        proc = new Process();
        proc.StartInfo = startInfo;
        proc.EnableRaisingEvents = true;
        try
        {
            proc.Start();
        }
        catch (Exception)
        {
            throw;
        }
        */
        return sd.Process.Start(filepath, args);
    }

    [MenuItem("RoomAlive/Start Kinect Server", false, 1)]
    private static void RunKinectServer()
    {
        if (cameraServer == null || cameraServer.HasExited)
        {
            string path = Directory.GetCurrentDirectory();
            string kinectServerPath = SettingsWindow.KinectServerPath;
            if (kinectServerPath.Equals("") || kinectServerPath == null)
            {
               kinectServerPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\KinectServer\bin\Debug\KinectServer.exe");
            }
            try
            {
                cameraServer = ProcessStart(kinectServerPath);
                Debug.Log("Started Kinect server");
            }
            catch (Exception e)
            {
                Debug.Log("Could not start Kinect server: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Kinect server is already running!");
        }
    }

    [MenuItem("RoomAlive/Start Projector Server", false, 2)]
    private static void RunProjectorServer()
    {
        if (projectorServer == null || projectorServer.HasExited)
        {
            string path = Directory.GetCurrentDirectory();
            string projectorServerPath = SettingsWindow.ProjectorServerPath;
            if (projectorServerPath.Equals("") || projectorServerPath == null)
            {
                projectorServerPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ProjectorServer\bin\Debug\ProjectorServer.exe");
            }
            try
            {
                projectorServer = ProcessStart(projectorServerPath);
                Debug.Log("Started projector server");
            }
            catch (Exception e)
            {
                Debug.Log("Failed to start projector server: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Projector server is already running!");
        }

    }

    [MenuItem("RoomAlive/Stop Servers", false, 3)]
    private static void StopServers()
    {
        if (cameraServer != null && !cameraServer.HasExited)
        {
            try
            {
                cameraServer.Kill();
                Debug.Log("Stopped Kinect server.");
            }
            catch (Exception e)
            {
                Debug.Log("Could not stop the kinect server: " + e.Message);
            }
        }
        if (projectorServer != null && !projectorServer.HasExited)
        {
            try
            {
                projectorServer.Kill();
                Debug.Log("Stopped projector server.");
            }
            catch (Exception e)
            {
                Debug.Log("Could not stop the projector server: " + e.Message);
            }
        }
    }

    [MenuItem("RoomAlive/Create New Setup", false, 51)]
    private static void CreateSetup()
    {
        fileSetupComplete = false;
        calibrationComplete = false;
        currentXMLFilePath = EditorUtility.SaveFilePanel("Save Setup File", "", "cal", "xml");
        if (currentXMLFilePath.Equals("") || currentXMLFilePath == null) return;

        string folderPath = Path.GetDirectoryName(currentXMLFilePath);
        string fileName = Path.GetFileName(currentXMLFilePath);
        string path = Directory.GetCurrentDirectory();
        string consoleApplicationPath = SettingsWindow.ConsoleApplicationPath;
        if (consoleApplicationPath.Equals("") || consoleApplicationPath == null)
        {
            consoleApplicationPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ConsoleCalibration\bin\Debug\ConsoleCalibration");
        }
        string arguments = "create " + "\"" + @folderPath + "\"" + " " + fileName;
        ProcessStart(consoleApplicationPath, arguments);
        fileSetupComplete = true;
    }

    [MenuItem("RoomAlive/Edit Setup", false, 52)]
    private static void ParseXML()
    {
        displayParseWindow();
    }

    [MenuItem("RoomAlive/Edit Setup", true)]
    private static bool validateEditSetup()
    {
        return fileSetupComplete;
    }

    [MenuItem("RoomAlive/Load Existing Setup",false,53)]
    private static void LoadXML()
    {
        currentXMLFilePath = EditorUtility.OpenFilePanel("Load Existing Setup", "", "xml");
        if (currentXMLFilePath.Equals("") || currentXMLFilePath == null) return;
        fileSetupComplete = true;
        fileLoaded = true;
        displayParseWindow();
        
    }
    ////Validation for editing the current setup file. Stops user from editing a non-existent XML file.
    //[MenuItem("RoomAlive/Edit Setup", false)] // TODO:  Change back to true once testing is complete.
    //private static bool ParseXMLValidation()
    //{
    //    return fileSetupComplete;
    //}
    [MenuItem("RoomAlive/Run Calibration", false, 101)]
    private static void Calibrate()
    {
        calibrationComplete = false;
        fileSetupComplete = false;
        string folderPath = Path.GetDirectoryName(currentXMLFilePath);
        string fileName = Path.GetFileName(currentXMLFilePath);
        string path = Directory.GetCurrentDirectory();
        string consoleApplicationPath = SettingsWindow.ConsoleApplicationPath;
        Debug.Log(consoleApplicationPath);
        if (consoleApplicationPath.Equals("") || consoleApplicationPath == null)
        {
            consoleApplicationPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ConsoleCalibration\bin\Debug\ConsoleCalibration");
        }
        string arguments = "calibrate " + "\"" + @folderPath + "\"" + " " + fileName;
        Debug.Log(arguments);
        ProcessStart(consoleApplicationPath, arguments);
        fileSetupComplete = true;
        calibrationComplete = true;
    }

    //Validation for Running a calibration. Stops user running the calibration unless a setup file has been created.
    [MenuItem("RoomAlive/Run Calibration", true)]// TODO : Change back to true once testing is complete.
    private static bool CalibrationValidation()
    {
        return fileSetupComplete;
    }

    [MenuItem("RoomAlive/Import Room",false, 102)]
    private static void ImportRoom()
    {
        string objectPath;
        if (File.Exists(currentXMLFilePath))
        {
            string objectName = Path.GetFileNameWithoutExtension(currentXMLFilePath);
            string objectDirectory = Path.GetDirectoryName(currentXMLFilePath);
            objectPath = Path.Combine(objectDirectory, objectName + ".obj");
            if (File.Exists(objectPath))
            {
                importAssetFromPath(objectPath);
                return;
            }
        }

        objectPath = EditorUtility.OpenFilePanel("Import scene object file", "", "obj");
        if (File.Exists(objectPath))
        {
            importAssetFromPath(objectPath);
            return;
        }
    }

    [MenuItem("RoomAlive/Adapt XML", false, 103)]
    static void adaptXml() {
        if (File.Exists(currentXMLFilePath)) {
            GameObject managerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Parsing/EnsembleManager.prefab");
            Debug.Log(managerPrefab);
            EnsembleManager manager = (PrefabUtility.InstantiatePrefab(managerPrefab) as GameObject).GetComponent<EnsembleManager>();
            manager.data = new EnsembleData(currentXMLFilePath);
            PrefabUtility.ReplacePrefab(manager.gameObject, managerPrefab);
        }
    }

    [MenuItem("RoomAlive/Adapt XML", true)]
    static bool validateAdaptXml() {
        return File.Exists(currentXMLFilePath);
    }

    static void importAssetFromPath(string path)
    {
        string name = Path.GetFileNameWithoutExtension(path);
        string ext = Path.GetExtension(path);
        string newPath = @"Assets/" + Path.GetFileName(path);
        int index = 0;
        while (File.Exists(newPath))
        {
            newPath = @"Assets/" + name + index.ToString() + ext;
        }
        name = name + index.ToString();
        try
        {
            File.Copy(path, newPath);
            AssetDatabase.ImportAsset(newPath);
            var scene = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
            Instantiate(scene);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not import object from path " + path + ";\n" + e.Message);
        }
    }

    //Validation for Importing an Object File into Unity. Stops the user from importing a room before running the calibration.
    [MenuItem("RoomAlive/Import Room", true)]
    private static bool ImportRoomValidation()
    {
        return calibrationComplete || fileLoaded;
    }

    [MenuItem("RoomAlive/Settings", false, 153)]
    private static void OpenSettings()
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = (SettingsWindow)CreateInstance("SettingsWindow");
        }
        SettingsWindow.ShowWindow();
    }

    private static void displayParseWindow()
    {
        if (ParseWindow == null)
        {
            ParseWindow = (ParseWindow)CreateInstance("ParseWindow");
        }
        ParseWindow.setFilePath(currentXMLFilePath);
        ParseWindow.LoadFile();
        ParseWindow.ParseFile();
        ParseWindow.ShowWindow();
    }
}
