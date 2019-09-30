using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class NetworkClient : MonoBehaviour
{
    [SerializeField]
    Text msgText;

    void Start()
    {
        Connect("FONTOOMAS");
        Client.OnMesageRecive += OnReciveMessage;
    }

    public void Connect(string name)
    {
        Client.Connect(name);
    }

    public void SendMesasge()
    {
        Client.SendMessage(msgText.text);
    }

    void OnReciveMessage(string message)
    {
        Debug.Log(message);
    }

    void OnDisable()
    {
        Client.Disconnect();
    }
}