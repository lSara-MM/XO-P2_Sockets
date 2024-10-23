using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

using UnityEngine.UI;
using System.Linq;
using System;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;

    public InputField inputField_IP;
    public InputField inputField_Text;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }
    public void StartClient()
    {
        if (ValidateIPv4(inputField_IP.text))
        {
            inputField_IP.gameObject.transform.parent.gameObject.SetActive(false);

            Thread mainThread = new Thread(Send);
            mainThread.Start();
        }
    }

    void Update()
    {
        UItext.text = clientText;
    }

    void Send()
    {
        //TO DO 2
        //Unlike with TCP, we don't "connect" first,
        //we are going to send a message to establish our communication so we need an endpoint
        //We need the server's IP and the port we've binded it to before
        //Again, initialize the socket
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(inputField_IP.text), 9050);

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //TO DO 2.1 
        //Send the Handshake to the server's endpoint.
        //This time, our UDP socket doesn't have it, so we have to pass it
        //as a parameter on it's SendTo() method

        byte[] data = new byte[1024];
        string handshake = "User connected";

        data = Encoding.ASCII.GetBytes(handshake);
        socket.SendTo(data, ipep);

        //TO DO 5
        //We'll wait for a server response,
        //so you can already start the receive thread
        Thread receive = new Thread(Receive);
        receive.Start();
    }

    //TO DO 5
    //Same as in the server, in this case the remote is a bit useless
    //since we already know it's the server who's communicating with us
    void Receive()
    {
        //IPEndPoint sender;
        //EndPoint Remote;

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref Remote);

        clientText = ("Message received from {0}: " + Remote.ToString());
        clientText = clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
    }

    // Validate the IP the user has introduced. Return true if valid IP
    public bool ValidateIPv4(string ipString)
    {
        if (String.IsNullOrEmpty(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }
}

