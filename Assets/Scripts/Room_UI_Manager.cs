using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Room_UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private TextMeshProUGUI createText;

    [SerializeField]
    private TextMeshProUGUI joinText;


    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {
        Photon_Manager._PHOTON_MANAGER.CreateRoom(createText.text.ToString());
    }

    private void JoinRoom()
    {
        Photon_Manager._PHOTON_MANAGER.JoinRoom(joinText.text.ToString());
    }
}
