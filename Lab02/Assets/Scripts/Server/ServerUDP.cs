using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ServerUDP : MonoBehaviour
{
    Socket socket;
    private AutoResetEvent _waitHandle = new AutoResetEvent(false);
    Socket socWorker;
    public AsyncCallback pfnCallBack;
    IAsyncResult m_asynResult;


    void Start()
    {

    }
    public void startServer()
    {
        Debug.Log("starting shit");
        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 9050);

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(localEp);

        Thread newConnection = new Thread(Receive);
        newConnection.Start();
    }

    void Update()
    {
    }


    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        while (true)
        {
            data = new byte[1024];
            recv = socket.ReceiveFrom(data, ref Remote);
           
          
           // Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        
            Send(Remote);
        }
    }


    void Send(EndPoint Remote)
    {
        byte[] data = new byte[1024];
        string welcome = "ServerName";

        data = Encoding.ASCII.GetBytes(welcome);

        socket.SendTo(data, data.Length, SocketFlags.None, Remote);
        Debug.Log("dataSend");
    }


}
