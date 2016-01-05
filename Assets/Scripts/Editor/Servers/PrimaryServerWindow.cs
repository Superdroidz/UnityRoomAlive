using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System;

public class PrimaryServerWindow : EditorWindow {
	
	private static string windowTitle = "Primary Server";
	private static int buttonWidth = 130;
	
	string localKinects = "0";
	string localProjectors = "0";

	string totalKinects = "0";
	string totalProjectors = "0";

    // local server process holders
    static Process projectorServer;
    static Process kinectServer;

    // TODO: should be relative, not absolute
    static private string projectorServerPath = @"C:\foxbox\dev\RoomAliveTK\ProCamCalibration\ProjectorServer\bin\Debug\ProjectorServer.exe";
    static private string kinectServerPath = @"";

    // begins a process from path with arguments and stores it in out
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

    public PrimaryServerWindow(){
	}

	private static void StartProjectorServers()
	{
        StartProcess(out projectorServer, projectorServerPath, "");
	}

    private static void KillProjectorServer()
    {
        if (projectorServer != null && !projectorServer.HasExited)
        {
            projectorServer.Kill();
        }
    }

	private static void StartKinectServers()
	{
        StartProcess(out kinectServer, projectorServerPath, "");
    }

    private static void KillKinectServers()
    {
        if (kinectServer != null && !kinectServer.HasExited)
        {
            kinectServer.Kill();
        }
    }

    private static void FindKinects()
	{

	}

	private static void FindProjectors()
	{

	}

	public void ShowWindow()
	{
		PrimaryServerWindow window = (PrimaryServerWindow)EditorWindow.GetWindow(typeof(PrimaryServerWindow)); //Creates Window;
		GUIContent titleContent = new GUIContent();
		titleContent.text = windowTitle;
		window.titleContent = titleContent;
		window.Show();
	}
	
	void OnGUI () {
		//Local Kinect Section
		EditorGUILayout.BeginHorizontal();
		localKinects = EditorGUILayout.TextField ("Local Kinects: ", localKinects);
		if (GUILayout.Button("Start Kinect Server",GUILayout.Width(buttonWidth)))
		{
			StartKinectServers();
		}
        if (GUILayout.Button("Stop Kinect Server", GUILayout.Width(buttonWidth)))
        {
            KillKinectServers();
        };
        EditorGUILayout.EndHorizontal();

		//Local Projectors Section
		EditorGUILayout.BeginHorizontal();
		localProjectors = EditorGUILayout.TextField ("Local Projectors: ", localProjectors);
		if (GUILayout.Button("Start Server",GUILayout.Width(buttonWidth)))
		{
			StartProjectorServers();
		}
        if (GUILayout.Button("Stop Server", GUILayout.Width(buttonWidth)))
        {
            KillProjectorServer();
        }
        EditorGUILayout.EndHorizontal();

		//Total Kinect Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Total Number of Kinects: ", totalKinects);
		if (GUILayout.Button("Find Kinects",GUILayout.Width(buttonWidth)))
		{
			FindKinects();
		}
		EditorGUILayout.EndHorizontal();

		//Total Projectors Section
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Total Number of Projectors: ", totalProjectors);
		if (GUILayout.Button("Find Projectors", GUILayout.Width(buttonWidth)))
		{
			FindProjectors();
		}
		EditorGUILayout.EndHorizontal();
	}
}
