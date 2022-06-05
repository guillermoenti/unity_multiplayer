using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{

    public static Photon_Manager _PHOTON_MANAGER;

    public int playerIdRace;
    public int enemyIdRace;

    public Race playerRace;
    public Race enemyRace;

    private void Awake()
    {
        if(_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(_PHOTON_MANAGER);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this);
            PhotonConnect();
        }
    }
    //Funcion propia para conectarme al servidor Photon
    public void PhotonConnect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    //Callback para cuando me he conectado con el servidor
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexion realizada correctamente");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //Callback para cuando hay desconexion
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("He implosionado because: " + cause);
    }

    //Callback para cuando me he unido al lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Accedido al lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala: " + PhotonNetwork.CurrentRoom.Name + "con " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados.");
        Debug.Log("Me he unido: " + PhotonNetwork.NickName);
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values){
            if (player.NickName != PhotonNetwork.NickName)
            {
                DataManager.instance.enemyNickname = player.NickName;
                Network_Manager._NETWORK_MANAGER.GetRaceByUsername(player.NickName);

            }
        }

        Debug.Log("Y mi enemigo es:" + DataManager.instance.enemyNickname);
    }

    public void CreateRoom(string nameRoom) {

        PhotonNetwork.CreateRoom(nameRoom, new RoomOptions { MaxPlayers = 2});
    
    
    }
    public void JoinRoom(string nameRoom) {

        PhotonNetwork.JoinRoom(nameRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            DataManager.instance.enemyNickname = newPlayer.NickName;
            Network_Manager._NETWORK_MANAGER.GetRaceByUsername(newPlayer.NickName);

            PhotonNetwork.LoadLevel("Ingame");
        }
    }



}
