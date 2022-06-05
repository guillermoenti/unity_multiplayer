using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPlayer1;

    [SerializeField]
    private GameObject spawnPlayer2;

    private void Awake()
    {
        
            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.Log("YO SOY :" + PhotonNetwork.NickName + "CON RAZA: " + DataManager.instance.playerRace.raceName + "Y MI ENEMIGO ES: " + DataManager.instance.enemyNickname + " CON LA RAZA: " + DataManager.instance.enemyRace.raceName);
                PhotonNetwork.Instantiate(DataManager.instance.playerRace.raceName, spawnPlayer1.transform.position, Quaternion.identity);
            }
            else
            {
                //Debug.Log("YO SOY :" + PhotonNetwork.NickName + "CON RAZA: " + DataManager.instance.playerRace.raceName);
                PhotonNetwork.Instantiate(DataManager.instance.playerRace.raceName, spawnPlayer2.transform.position, Quaternion.identity);
            } 
        
    }
}
