using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEditor;

[ExecuteInEditMode]
public class BroadcastSender {
    static UdpClient sender;

    static BroadcastSender()
    {
        try
        {
            sender = new UdpClient(PortSetup.broadcasterPort, AddressFamily.InterNetwork);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, PortSetup.listenerPort);
            sender.Connect(groupEP);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    public static void SendData()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        Debug.Log("Sending broadcast");
        string customMessage = Network.player.ipAddress;
        if (customMessage != "")
        {
            sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
        }
    }
}
