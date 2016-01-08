using UnityEngine;
using UnityEditor;
using System.Collections;

public class NetworkingMenuItem : EditorWindow {
    static Broadcaster broadcaster = new Broadcaster();

    [MenuItem("Networking/Start Broadcast", priority=1)]
    private static void StartBroadcast()
    {
        broadcaster.BeginBroadcast();
    }

    [MenuItem("Networking/End Broadcast", priority = 2)]
    private static void StopBroadcast()
    {
        broadcaster.EndBroadcast();
    }

    [MenuItem("Networking/Listen for Broadcast", priority = 3)]
    private static void ListenForBroadcast()
    {
        broadcaster.Listen();
    }

    [MenuItem("Networking/Show local IP", priority = 51)]
    private static void ShowCurrentIP()
    {
        Debug.Log(Network.player.ipAddress);
    }
}
