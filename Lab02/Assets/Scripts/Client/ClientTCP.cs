using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
//using UnityEngine.tvOS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ClientTCP : MonoBehaviour
{
    //public GameObject UItextObj;
    public TextMeshProUGUI UIText;
    string clientText;
    Socket server;
    bool goToSampleScene = false;

    
    
    void Start()
    {
    }

    void Update()
    {
        if (goToSampleScene == true)
        {
            SceneManager.LoadScene (sceneName:"GameScene");
        }
    }

    public void StartClient()
    {
        Thread connect = new Thread(Connect);
        connect.Start();
    }
    void Connect()
    {
        Debug.Log("connecting to server");

        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Connect(ipep);

        Thread sendThread = new Thread(Send);
        sendThread.Start();

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();
        goToSampleScene = true;
    }
    void Send()
    {
        byte[] data = new byte[1024];
        Debug.Log("sending welcome");
        data = Encoding.ASCII.GetBytes("userName");
        server.Send(data);
    }

    void Receive()
    {
        byte[] data = new byte[1024];
        int recv = 0;
        recv = server.Receive(data);
    }
}
