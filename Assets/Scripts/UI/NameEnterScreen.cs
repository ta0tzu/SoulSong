using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NameEnterScreen : MonoBehaviour
{
    string grieveName = "Grieve";
    public TextMeshProUGUI nameDisplay;
    public GameObject[] letterButtons;
    public GameObject confirmButton;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(confirmButton);
    }

    public void EnterLetter(string letter)
    {
        if (grieveName.Length < 10)
        {
            grieveName += letter;
            nameDisplay.text += letter;
        }
        else
        {
            return;
        }
    }
    public void ClearName()
    {
        grieveName = string.Empty;
        nameDisplay.text = string.Empty;
    }

    public void Back()
    {
        if (grieveName.Length > 0)
        {
            grieveName = grieveName.Remove(grieveName.Length - 1);
            Debug.Log(grieveName);
            nameDisplay.text = grieveName;
        }
        else
        {
            return;
        }
    }

    public void ConfirmName()
    {
        if (nameDisplay.text != string.Empty)
        {
            Engine.e.gameStart = true;
            //SceneManager.UnloadSceneAsync("GrieveNameInput");
            Engine.e.loadIntoGameTransition.SetActive(true);

            Engine.e.party[0].GetComponent<Character>().characterName = grieveName;


        }
        else
        {
            return;
        }
    }
}
