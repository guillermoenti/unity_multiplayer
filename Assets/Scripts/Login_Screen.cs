using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Login_Screen : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private TextMeshProUGUI passwordText;

    [SerializeField] private GameObject wrongText;

    private void Awake()
    {
        //Defino el listener para cada vez que se haga click al boton
        loginButton.onClick.AddListener(CheckUser);
        registerButton.onClick.AddListener(GoToRegister);
    }

    private void CheckUser()
    {       
        //Llamo a la funcion del network manager para conectarme al servidor pasando nick y contraseña
        Network_Manager._NETWORK_MANAGER.ConnectToServer(loginText.text.ToString(), passwordText.text.ToString());
    }

    private void GoToRegister()
    {
        SceneManager.LoadScene("Register_Scene");
    }

    public void WrongUserPass()
    {
        wrongText.SetActive(true);
    }

    
}
