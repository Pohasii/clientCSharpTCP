using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;

public class NetworkClient : MonoBehaviour
{
    string userName;
    public string host = "127.0.0.1";
    public int port = 8888;

    TcpClient client;
    NetworkStream stream;

    [SerializeField]
    InputField inputField;

    static event Action<string> OnMesageRecive = delegate { };

    public void SetNick()
    {
        userName = inputField.text;
        inputField.placeholder.GetComponent<Text>().text = "Enter your message";
    }

    public async void Connect()
    {
        client = new TcpClient();

        client.Connect(host, port);
        stream = client.GetStream();

        string message = userName;
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);

        await Task.Run(() => ReceiveMessage());
    }

    public void ReceiveMessage()
    {
        while (true)
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            var message = builder.ToString();
            OnMesageRecive(message);
        }
    }

    public void SendMsg(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    public void Disconnect()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
    }

    void OnReciveMessage(string message)
    {
        Debug.Log(message);
    }

    void OnEnable()
    {
        OnMesageRecive += OnReciveMessage;
    }

    void OnDisable()
    {
        OnMesageRecive -= OnReciveMessage;
        Disconnect();
    }
}