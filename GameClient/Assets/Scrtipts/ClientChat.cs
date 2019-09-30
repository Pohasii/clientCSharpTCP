using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

class Client
{
    static string userName;
    public static readonly string host = "127.0.0.1";
    public static readonly int port = 8888;
    static TcpClient client;
    static NetworkStream stream;

    public static event Action<string> OnMesageRecive = delegate { };

    public static async void Connect(string _userName)
    {
        userName = _userName;
        client = new TcpClient();

        client.Connect(host, port);
        stream = client.GetStream();

        string message = userName;
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);

        await Task.Run(() => ReceiveMessage());
        //Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
        //receiveThread.Start();
    }

    public static void SendMessage(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    public static void ReceiveMessage()
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

    public static void Disconnect()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
    }
}
