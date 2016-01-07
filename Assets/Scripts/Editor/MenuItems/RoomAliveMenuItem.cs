using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;

public class RoomAliveMenuItem : EditorWindow{
    public static ParseWindow ParseWindow;
    private static string currentXMLFilePath;

    private static bool fileSetupComplete = false;
    private static bool calibrationComplete = false;

    [MenuItem("RoomAlive/Start Kinect Server", false, 1)]
    private static void RunKinectServer()
    {
        string kinectServerPath = @"C:\Users\Adam\Desktop\3rdYearProject\UnityExtension\RoomAlive\RoomAliveToolkit-master\ProCamCalibration\KinectServer\bin\Debug\KinectServer.exe";
        Process.Start(kinectServerPath);
    }

    [MenuItem("RoomAlive/Start Projector Server", false, 2)]
    private static void RunProjectorServer()
    {
        string projectorServerPath = @"C:\Users\Adam\Desktop\3rdYearProject\UnityExtension\RoomAlive\RoomAliveToolkit-master\ProCamCalibration\ProjectorServer\bin\Debug\ProjectorServer.exe";
        Process.Start(projectorServerPath);
    }

    [MenuItem("RoomAlive/Create New Setup", false, 51)]
    private static void CreateSetup()
    {
        fileSetupComplete = false;
        calibrationComplete = false;
        currentXMLFilePath = EditorUtility.SaveFilePanel("Save Setup File", "", "cal", "xml");
        string folderPath = Path.GetDirectoryName(currentXMLFilePath);
        string consoleApplicationPath = @"C:\Users\Adam\Desktop\3rdYearProject\RoomAliveTK\ProCamCalibration\CalibrateEnsembleViaConsole\bin\Debug\CalibrateEnsembleViaConsole";
        string arguments = "create " + "\"" + @folderPath + "\"";
        UnityEngine.Debug.Log(arguments);
        Process.Start(consoleApplicationPath, arguments);
        fileSetupComplete = true;
    }

    [MenuItem("RoomAlive/Edit Setup", false, 52)]
    private static void ParseXML()
    {
        ParseWindow = (ParseWindow)ScriptableObject.CreateInstance("ParseWindow");
        ParseWindow.setFilePath(currentXMLFilePath);
        ParseWindow.ShowWindow();   

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
        Process process;
        string consoleApplicationPath = @"C:\Users\Adam\Desktop\3rdYearProject\RoomAliveTK\ProCamCalibration\CalibrateEnsembleViaConsole\bin\Debug\CalibrateEnsembleViaConsole";
        StartProcess(out process, consoleApplicationPath, "calibrate C:\\Users\\Adam\\Desktop\\3rdYearProject\\TestFolder");
        fileSetupComplete = true;
        calibrationComplete = true;
    }

    //Validation for Running a calibration. Stops user running the calibration unless a setup file has been created.
    [MenuItem("RoomAlive/Run Calibration", false)]// TODO : Change back to true once testing is complete.
    private static bool CalibrationValidation()
    {
        return fileSetupComplete;
    }
    
    [MenuItem("RoomAlive/Import Room",false, 151)]
    private static void ImportRoom()
    {
        EditorApplication.ExecuteMenuItem("Assets/Import New Asset...");
    }

    //Validation for Importing an Object File into Unity. Stops the user from importing a room before running the calibration.
    [MenuItem("RoomAlive/Import Room", false)] //TODO : Change back to true once testing is complete.
    private static bool ImportRoomValidation()
    {
        return calibrationComplete;
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
