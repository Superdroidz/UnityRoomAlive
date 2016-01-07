using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

[ExecuteInEditMode]
public class BroadcastReceiver {
    UdpClient receiver;

    public void ListenForBroadcast()
    {
        try
        {
            if (receiver == null)
            {
                receiver = new UdpClient(PortSetup.listenerPort);
                receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
            }
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
        if (receiver != null)
        {
            received = receiver.EndReceive(result, ref receiveIPGroup);
            Debug.Log("Got ip: " + Encoding.ASCII.GetString(received));
        }
    }
}
