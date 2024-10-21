using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ClientUDP : MonoBehaviour
{
    public TextMeshProUGUI UIText;
    string clientText;
    Socket server;
    Socket socWorker;
    bool goToSampleScene = false;
    private AutoResetEvent _waitHandle = new AutoResetEvent(false);
    public AsyncCallback pfnCallBack;
    IAsyncResult m_asynResult;

    void Start()
    {

    }
    public void StartClient()
    {
   
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        if (goToSampleScene == true)
        {
            SceneManager.LoadScene(sceneName: "GameScene");
        }
    }

    void Send()
    {
        Debug.Log("connecting to server");
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] data = new byte[1024];
        string handshake = "UserName";

        server.SendTo(data = Encoding.ASCII.GetBytes(handshake), 0, SocketFlags.None, ipep);

        Thread receive = new Thread(Receive);
        receive.Start();
    }

    void Receive()
    {
        Debug.Log("recieving message");
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        byte[] data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);
        goToSampleScene = true;
   

    }
}

