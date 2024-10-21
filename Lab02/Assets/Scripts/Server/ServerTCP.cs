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

public class ServerTCP : MonoBehaviour
{
    private AutoResetEvent _waitHandle = new AutoResetEvent(false);
    Socket socket;
    Socket socWorker;
    Thread mainThread = null;
    public AsyncCallback pfnCallBack;
    IAsyncResult m_asynResult;

    string serverText;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
    }


    void Update()
    {

    }


    public void startServer()
    {
        Debug.Log("initializing startserver()");

        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 9050);

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(localEp);
        socket.Listen(10);

        Debug.Log("Socket worked");

        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    async void CheckNewConnections()
    {
        while (true)
        {
            int recv = 0;
            Debug.Log("starting checkNewConnections()");
            User newUser = new User();
            newUser.name = "";

            newUser.socket = socket.Accept();

            IPEndPoint clientep = (IPEndPoint)socket.RemoteEndPoint;

            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
    }


    async void Receive(User user)
    {
        byte[] data = new byte[1024];
        if (pfnCallBack == null) pfnCallBack = new AsyncCallback(OnDataReceived);

        m_asynResult = user.socket.BeginReceive(data, 0, data.Length, SocketFlags.None, pfnCallBack, user);
        m_asynResult.AsyncWaitHandle.WaitOne();

        Debug.Log(Encoding.ASCII.GetString(data, 0, data.Length));
    }
    public void OnDataReceived(IAsyncResult state)
    {
        try
        {
            socWorker = socket.EndAccept(state);

            Debug.Log(socWorker.LocalEndPoint);

            Thread answer = new Thread(() => Send(socWorker));
            answer.Start();

            CheckNewConnections();
        }
        catch (Exception ex)
        {

        }
    }

    void Send(Socket socket_)
    {
        byte[] data_ = new byte[1024];
        Debug.Log("sending ping");
        data_ = Encoding.ASCII.GetBytes("serverName");
        socket_.Send(data_);
    }
}
