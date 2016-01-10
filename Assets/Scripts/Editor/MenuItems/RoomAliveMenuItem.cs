using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;
using System.IO;

public class RoomAliveMenuItem : EditorWindow{
    public static ParseWindow ParseWindow;
    public static SettingsWindow SettingsWindow;

    private static string currentXMLFilePath;

    private static bool fileSetupComplete = false;
    private static bool calibrationComplete = false;
    private static bool fileLoaded = false;

    [MenuItem("RoomAlive/Start Kinect Server", false, 1)]
    private static void RunKinectServer()
    {
        string path = Directory.GetCurrentDirectory();
        string kinectServerPath = SettingsWindow.KinectServerPath;
        if (kinectServerPath.Equals("") || kinectServerPath == null)
        {
           kinectServerPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\KinectServer\bin\Debug\KinectServer.exe");
        }
        Process.Start(kinectServerPath);
    }

    [MenuItem("RoomAlive/Start Projector Server", false, 2)]
    private static void RunProjectorServer()
    {
        string path = Directory.GetCurrentDirectory();
        string projectorServerPath = SettingsWindow.ProjectorServerPath;
        if (projectorServerPath.Equals("") || projectorServerPath == null)
        {
            projectorServerPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ProjectorServer\bin\Debug\ProjectorServer.exe");
        }
        Process.Start(projectorServerPath);

    }

    [MenuItem("RoomAlive/Create New Setup", false, 51)]
    private static void CreateSetup()
    {
        fileSetupComplete = false;
        calibrationComplete = false;
        currentXMLFilePath = EditorUtility.SaveFilePanel("Save Setup File", "", "cal", "xml");

        string folderPath = Path.GetDirectoryName(currentXMLFilePath);
        string fileName = Path.GetFileName(currentXMLFilePath);
        string path = Directory.GetCurrentDirectory();
        string consoleApplicationPath = SettingsWindow.ConsoleApplicationPath; // Catch Null Reference Exception
        if (consoleApplicationPath.Equals("") || consoleApplicationPath == null)
        {
            consoleApplicationPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ConsoleCalibration\bin\Debug\ConsoleCalibration");
        }
        string arguments = "create " + "\"" + @folderPath + "\"" + " " + fileName;
        Process.Start(consoleApplicationPath, arguments);
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
        UnityEngine.Debug.Log(consoleApplicationPath);
        if (consoleApplicationPath.Equals("") || consoleApplicationPath == null)
        {
            consoleApplicationPath = Path.Combine(path, @"RoomAlive\ProCamCalibration\ConsoleCalibration\bin\Debug\ConsoleCalibration");
        }
        string arguments = "calibrate " + "\"" + @folderPath + "\"" + " " + fileName;
        UnityEngine.Debug.Log(arguments);
        Process.Start(consoleApplicationPath, arguments);
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
        EditorApplication.ExecuteMenuItem("Assets/Import New Asset...");
    }

    //Validation for Importing an Object File into Unity. Stops the user from importing a room before running the calibration.
    [MenuItem("RoomAlive/Import Room", true)] //TODO : Change back to true once testing is complete.
    private static bool ImportRoomValidation()
    {
        return calibrationComplete || fileLoaded;
    }

    [MenuItem("RoomAlive/Settings", false, 153)]
    private static void OpenSettings()
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = (SettingsWindow)ScriptableObject.CreateInstance("SettingsWindow");
        }
        SettingsWindow.ShowWindow();
    }

    private static void displayParseWindow()
    {
        UnityEngine.Debug.Log(currentXMLFilePath);
        if (ParseWindow == null)
        {
            ParseWindow = (ParseWindow)ScriptableObject.CreateInstance("ParseWindow");
        }
        ParseWindow.setFilePath(currentXMLFilePath);
        ParseWindow.LoadFile();
        ParseWindow.ParseFile();
        ParseWindow.ShowWindow();
    }

    private static void StartProcess(out Process proc, string processPath, string args)
    {
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
    }
}
