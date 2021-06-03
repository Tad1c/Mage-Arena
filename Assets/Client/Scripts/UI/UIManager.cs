using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientSide;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        usernameField.text = CurrentPlayerData.playerDisplayName;
        Client.instance.SetIP(Matchmaker.ip);
        Client.instance.SetPorts(Matchmaker.tcp_port, Matchmaker.udp_port);
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }
}
