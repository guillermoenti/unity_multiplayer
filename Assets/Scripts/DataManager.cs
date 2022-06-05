using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public List<Race> races = new List<Race>();

    public string tmpData = "";

    public string playerNickname = "";
    public string enemyNickname = "";


    public int playerRaceId;
    public int enemyRaceId;

    public Race playerRace;
    public Race enemyRace;


    private void Awake()
    {
        //Si ya existe una instancia del manager y es diferente de la instancia creada en este script destruyo por duplicado
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            //Defino esta instancia como network manager y la asigno como dont destroy para evitar que se borre al cambiar de escena
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        Network_Manager._NETWORK_MANAGER.GetRacesData();
    }


    public void SetRacesData(string data)
    {
        

        string[] tmpString = data.Split('&');

        Debug.Log(tmpString.Length);

        for(int i = 0; i < tmpString.Length - 1; i++)
        {
            Race tmpRace = new Race();

            string[] splitData = tmpString[i].Split('|');

            Debug.Log(tmpString[i]);

            tmpRace.SetData(int.Parse(splitData[0]), int.Parse(splitData[1]), int.Parse(splitData[2]), int.Parse(splitData[3]),
                            int.Parse(splitData[4]), int.Parse(splitData[5]), splitData[6]);

            races.Add(tmpRace);
        }


        

        for(int i = 0; i < races.Count; i++)
        {
            Debug.Log(races[i].raceName + ", " + races[i].health + ", " +  races[i].damage + ", " + races[i].speed);
        }


        SceneManager.LoadScene("Login_Scene");
    }

    public Race GetRaceById(int _id)
    {
        foreach(Race race in races)
        {
            if(race.race_id == _id)
            {
                return race;
            }
        }

        return null;
    }


}
