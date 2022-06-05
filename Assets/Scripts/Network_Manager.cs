using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool connected = false;

    //Server variables
    const string host = "127.0.0.1";
    const int port = 6543;

    private void Awake()
    {
        //Si ya existe una instancia del manager y es diferente de la instancia creada en este script destruyo por duplicado
        if (_NETWORK_MANAGER != null && _NETWORK_MANAGER != this)
        {
            Destroy(_NETWORK_MANAGER);
        }
        else
        {
            //Defino esta instancia como network manager y la asigno como dont destroy para evitar que se borre al cambiar de escena
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        //Si estoy conectado reviso si existen datos
        if (connected)
        {
            //Si hay datos disponibles para leer
            if (stream.DataAvailable)
            {
                //Leo una linea de datos
                string data = reader.ReadLine();
                
                //Si los datos no son nulos los trabajo
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }

    public void ConnectToServer(string nick, string password)
    {
        try
        {   
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            
            //Si hay streaming de datos hay conexion
            connected = true;
            
            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            
            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("Login" + "/" + nick + "/" + password);
            
            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void RegisterAccount(string nick, string password, int id_race)
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("Register/" + nick + "/" + password + "/" + id_race.ToString());

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void GetRacesData()
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("RacesData");

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void GetRaceByUsername(string user)
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("GetRace" + "/" + user);

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ManageData(string data)
    {
        string[] dataSplit = data.Split('/');
        //Si recibo ping devuelvo 1 como respuesta al servidor
        if (data == "ping")
        {
            Debug.Log("Recibo ping");
            writer.WriteLine("1");
            writer.Flush();
        }
        if (dataSplit[0] == "Login response")
        {
            if (dataSplit[1] == "True")
            {
                DataManager.instance.playerNickname = dataSplit[2];
                PhotonNetwork.NickName = dataSplit[2];
                DataManager.instance.playerRaceId = int.Parse(dataSplit[3]);
                DataManager.instance.playerRace = DataManager.instance.GetRaceById(DataManager.instance.playerRaceId);
                SceneManager.LoadScene("Room_Scene");
            }
            else
            {
                GameObject.FindObjectOfType<Login_Screen>().WrongUserPass();
            }
        }
        if(dataSplit[0] == "RacesData")
        {
            DataManager.instance.SetRacesData(dataSplit[1]);
        }
        if (dataSplit[0] == "RaceResponse")
        {
            DataManager.instance.enemyRaceId = int.Parse(dataSplit[1]);
            DataManager.instance.enemyRace = DataManager.instance.GetRaceById(DataManager.instance.enemyRaceId);
        }
    }
}
