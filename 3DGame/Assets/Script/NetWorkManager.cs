using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    public InputField nickName;
    public GameObject disconnect;
    public GameObject respawnPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(950, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public void Connet() => PhotonNetwork.ConnectUsingSettings();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = nickName.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }
    public override void OnJoinedRoom()
    {
        disconnect.SetActive(false);
        Spawn();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnect.SetActive(true);
        respawnPanel.SetActive(false);
    }
    public void Spawn() 
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        respawnPanel.SetActive(false);

    }
}
