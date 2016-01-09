using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Broadcaster {
    UdpClient broadcaster;
    UdpClient listener;

    TimeSpan resendDelayMillis = new TimeSpan(0,0,0,0,500); // 500ms
    System.Diagnostics.Stopwatch stopwatch;

    public Broadcaster()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
        // setup broadcaster
        try
        {
            broadcaster = new UdpClient(PortSetup.broadcasterPort, AddressFamily.InterNetwork);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, PortSetup.listenerPort);
            broadcaster.Connect(groupEP);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BeginBroadcast()
    {
        if (stopwatch.IsRunning)
        {
            Debug.Log("Already broadcasting!");
        }
        else
        {
            Debug.Log("Starting broadcast");
            stopwatch.Start();
            EditorApplication.update += BroadcastMessage;
        }
    }

    public void EndBroadcast()
    {
        if (stopwatch.IsRunning)
        {
            Debug.Log("Ending broadcast");
            stopwatch.Stop();
            EditorApplication.update -= BroadcastMessage;
        }
        else
        {
            Debug.Log("No broadcast currently running");
        }
    }

    void BroadcastMessage()
    {
        
        string customMessage = Network.player.ipAddress;
        if (customMessage != "" && (stopwatch.Elapsed.CompareTo(resendDelayMillis) > -1))
        {
            Debug.Log("Sending broadcast");
            broadcaster.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
            stopwatch.Reset();
            stopwatch.Start();
        }
    }

    public void Listen()
    {
        try
        {
            listener = new UdpClient(PortSetup.listenerPort);
            listener.BeginReceive(new AsyncCallback(ReceiveData), null);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    void ReceiveData(IAsyncResult result)
    {
        IPEndPoint receiveIPGroup = new IPEndPoint(IPAddress.Any, PortSetup.listenerPort);
        byte[] received;
        if (listener != null)
        {
            received = listener.EndReceive(result, ref receiveIPGroup);
            Debug.Log("Got ip: " + Encoding.ASCII.GetString(received));
        }
        listener.Close();
    }
}
