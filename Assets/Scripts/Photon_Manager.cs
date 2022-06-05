using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{

    public static Photon_Manager _PHOTON_MANAGER;

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
        PhotonNetwork.LocalPlayer.NickName = DataManager.instance.playerName;
    }

    public void CreateRoom(string nameRoom) {

        PhotonNetwork.CreateRoom(nameRoom, new RoomOptions { MaxPlayers = 2});
    
    
    }
    public void JoinRoom(string nameRoom) {

        PhotonNetwork.JoinRoom(nameRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {

            Network_Manager._NETWORK_MANAGER.GetRacesOnGame(DataManager.instance.playerName, PhotonNetwork.PlayerListOthers[0].NickName);
            Debug.Log("ID: " + PhotonNetwork.PlayerListOthers[0].NickName);
            PhotonNetwork.LoadLevel("Ingame");
        }
    }

}
