using System;
using SD = System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Text;

public class RoomAliveWrapper : ITKWrapper
{
    SD.Process cameraServer, projectorServer;
    bool cameraServerIsRunning, projectorServerIsRunning;
    string debugFlag = " -b";
    string verboseFlag = " -v";
    string filenameFlag = " -f";
    string directoryFlag = " -d";

    /*
    * Starts a process from filepath, with args.
    *
    * If redirectOutput is true, redirects stdout and stderr to Debug.Log and Debug.Error.
    */
    SD.Process ProcessStart(string filepath, string args="", bool redirectOutput=false)
    {
        var startInfo = new SD.ProcessStartInfo
        {
            FileName = filepath,
            Arguments = args,
            CreateNoWindow = false,
            UseShellExecute = false,
            WindowStyle = SD.ProcessWindowStyle.Minimized,
        };
        var process = new SD.Process
        {
            StartInfo = startInfo
        };


        if (redirectOutput)
        {
            //redirect console output
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            process.OutputDataReceived += ReadProcessOutputEventHandler;
            process.ErrorDataReceived += ReadProcessErrorEventHandler;

            //raise an event when the console exits
            process.EnableRaisingEvents = true;
            process.Exited += ConsoleExitEventHandler;
        }

        try
        {
            process.Start();
            if (redirectOutput)
            {
                //attach console output readers
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

            }
            return process;
        }
        catch (Exception e)
        {
            Debug.LogError("Could not start external process: " + e.Message);
            return null;
        }
    }

    static void ReadProcessOutputEventHandler(object sender, SD.DataReceivedEventArgs e)
    {
        if (e.Data == null) return;
        Debug.Log(e.Data);
    }

    static void ReadProcessErrorEventHandler(object sender, SD.DataReceivedEventArgs e)
    {
        if (e.Data == null) return;
        Debug.LogError(e.Data);
    }

    void ConsoleExitEventHandler(object sender, EventArgs e)
    {
        Debug.Log("Process has finished.");
    }

    public void CreateNewSetup(string XMLFilepath)
    {
        var consoleApplicationPath = SettingsWindow.Settings.ConsoleApplicationPath;
        if (File.Exists(consoleApplicationPath))
        {
            var folderPath = Path.GetDirectoryName(XMLFilepath);
            var fileName = Path.GetFileNameWithoutExtension(XMLFilepath);
            var args = new StringBuilder();
            args.Append("--setup");
            args.Append(directoryFlag).Append(folderPath);
            args.Append(filenameFlag).Append(fileName);
            if (SettingsWindow.Settings.IsVerbose)
            {
                args.Append(verboseFlag);
            }
            if (SettingsWindow.Settings.IsDebug)
            {
                args.Append(debugFlag);
            }

            ProcessStart(consoleApplicationPath, args.ToString(), true);
        }
        else
        {
            Debug.LogError("Could not find RoomaAlive wrapper executable at " + consoleApplicationPath);
        }
    }

    public void RunCalibration(string filepath)
    {
        var consoleApplicationPath = SettingsWindow.Settings.ConsoleApplicationPath;
        if (File.Exists(consoleApplicationPath))
        {
            var folderPath = Path.GetDirectoryName(filepath);
            var fileName = Path.GetFileNameWithoutExtension(filepath);
            var args = new StringBuilder();
            args.Append("--calibrate");
            args.Append(directoryFlag).Append(folderPath);
            args.Append(filenameFlag).Append(fileName);
            if (SettingsWindow.Settings.IsVerbose)
            {
                args.Append(verboseFlag);
            }
            if (SettingsWindow.Settings.IsDebug)
            {
                args.Append(debugFlag);
            }
            ProcessStart(consoleApplicationPath, args.ToString(), true);
        }
        else
        {
            Debug.LogError("Could not find RoomaAlive wrapper executable at " + consoleApplicationPath);
        }
    }


    public bool ServersAreRunning()
    {
        return cameraServerIsRunning || projectorServerIsRunning;
    }

    public void StartCameraServer()
    {
        if (cameraServerIsRunning)
        {
            Debug.LogWarning("Kinect server is already running!");
        }
        else
        {
            var kinectServerPath = SettingsWindow.Settings.KinectServerPath;
            if (File.Exists(kinectServerPath))
            {
                try
                {
                    cameraServer = ProcessStart(kinectServerPath);
                    cameraServerIsRunning = true;
                    Debug.Log("Started Kinect server");
                }
                catch (Exception e)
                {
                    Debug.LogError("Could not start Kinect server: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Could not find Kinect server executable at " + kinectServerPath);
            }
        }
    }

    public void StartProjectorServer()
    {
        if (projectorServerIsRunning)
        {
            Debug.LogWarning("Projector server is already running!");
        }
        else
        {
            var projectorServerPath = SettingsWindow.Settings.ProjectorServerPath;
            if (File.Exists(projectorServerPath))
            {
                try
                {
                    projectorServer = ProcessStart(projectorServerPath);
                    projectorServerIsRunning = true;
                    Debug.Log("Started projector server");
                }
                catch (Exception e)
                {
                    Debug.LogError("Could to start projector server: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Could not find projector server executable at " + projectorServerPath);
            }
        }
    }

    public void StopServers()
    {
        if (cameraServerIsRunning)
        {
            try
            {
                cameraServer.Kill();
                Debug.Log("Stopped Kinect server.");
            }
            catch (Exception e)
            {
                Debug.LogError("Could not stop the kinect server: " + e.Message);
            }
            cameraServerIsRunning = false;
        }
        if (projectorServerIsRunning)
        {
            try
            {
                projectorServer.Kill();
                Debug.Log("Stopped projector server.");
            }
            catch (Exception e)
            {
                Debug.LogError("Could not stop the projector server: " + e.Message);
            }
            projectorServerIsRunning = false;
        }
    }
}
