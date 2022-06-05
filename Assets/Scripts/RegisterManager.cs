using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private Button registerButton;
    [SerializeField] private TextMeshProUGUI userText;
    [SerializeField] private TextMeshProUGUI passText;
    [SerializeField] TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Race race in DataManager.instance.races)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = race.raceName });
        }

        registerButton.onClick.AddListener(CheckRegisterData);
    }

    private void CheckRegisterData()
    {
        Network_Manager._NETWORK_MANAGER.RegisterAccount(userText.text.ToString(), passText.text.ToString(), dropdown.value + 1);
        SceneManager.LoadScene("Login_Scene");
    }

    private int GetIDByName(string _name)
    {
        List <Race> races = DataManager.instance.races;
        Debug.Log(_name);

        int idReturn = -1;

        for(int i = 0; i < races.Count; i++)
        {
            if (races[i].raceName == _name)
            {
                idReturn = races[i].race_id;
            }
        }

        return idReturn;
    }
}
