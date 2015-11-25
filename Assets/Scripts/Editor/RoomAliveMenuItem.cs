using UnityEngine;
using UnityEditor;
using System.Collections;

public class RoomAliveMenuItem : EditorWindow{

	public static PrimaryServerWindow PrimaryWindow;
	public static SecondaryServerWindow SecondaryWindow;
	//public static SlaveServerWindow window;

	[MenuItem("RoomAlive/Start Server")]
	
	[MenuItem("RoomAlive/Start Server/Primary", false, 1)] // Should require Validation
	private static void OpenPrimaryServer()
	{
		PrimaryWindow = (PrimaryServerWindow)ScriptableObject.CreateInstance("PrimaryServerWindow");
		PrimaryWindow.ShowWindow();

	}

	[MenuItem("RoomAlive/Start Server/Secondary", false, 2)] // Should require Validation
	private static void OpenSecondaryServer()
	{
		SecondaryWindow = (SecondaryServerWindow)ScriptableObject.CreateInstance("SecondaryServerWindow");
		SecondaryWindow.ShowWindow();
	}

	[MenuItem("RoomAlive/Acquire", false, 51)] // Should require Validation
	private static void Acquire()
	{

	}

	[MenuItem("RoomAlive/Solve", false, 52)] // Should require Validation
	private static void Solve()
	{

	}

	[MenuItem("RoomAlive/Import Room", false, 101)] // Should require Validation
	private static void ImportRoom()
	{

	}
}
