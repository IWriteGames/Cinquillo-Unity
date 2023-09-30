using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPlay;
    [SerializeField] private GameObject buttonCredits;
    [SerializeField] private GameObject buttonQuitGame;
    [SerializeField] private GameObject optionsIA;

    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private TMP_Dropdown numberIADropdown;
    [SerializeField] private TMP_Text selectedText;


    public static int numberIA;
    public static bool playerPlay;

    private void Awake() 
    {
        buttonsPlay.SetActive(true);
        optionsIA.SetActive(false);
        buttonCredits.SetActive(true);
        buttonQuitGame.SetActive(true);
        CloseCredits();      
        playerPlay = false;
    }

    //Manage UI
    public void SelectGameMode(bool player)
    {
        playerPlay = player;

        numberIADropdown.ClearOptions();

        var option1 = new TMP_Dropdown.OptionData("1");
        var option2 = new TMP_Dropdown.OptionData("2");
        var option3 = new TMP_Dropdown.OptionData("3");
        var option4 = new TMP_Dropdown.OptionData("4");
        var option5 = new TMP_Dropdown.OptionData("5");
        var option6 = new TMP_Dropdown.OptionData("6");

        if(player)
        {
            selectedText.text = "Player Selected";

            numberIADropdown.options.Add(option1);
            numberIADropdown.options.Add(option2);
            numberIADropdown.options.Add(option3);
            numberIADropdown.options.Add(option4);
            numberIADropdown.options.Add(option5);
        } else {

            selectedText.text = "Only IA";

            numberIADropdown.options.Add(option2);
            numberIADropdown.options.Add(option3);
            numberIADropdown.options.Add(option4);
            numberIADropdown.options.Add(option5);
            numberIADropdown.options.Add(option6);
        }

        numberIADropdown.RefreshShownValue();

        buttonsPlay.SetActive(false);
        optionsIA.SetActive(true);
        buttonCredits.SetActive(false);
        buttonQuitGame.SetActive(false);
    }

    public void ReturnMenu()
    {
        buttonsPlay.SetActive(true);
        optionsIA.SetActive(false);
        buttonCredits.SetActive(true);
        buttonQuitGame.SetActive(true);
    }

    //Play Game
    public void PlayGame()
    {
        numberIA = numberIADropdown.value;

        if(playerPlay)
        {
            numberIA += 1;
        } else {
            numberIA += 2;
        }

        PlayerPrefs.SetInt("PlayerPlay", (playerPlay) ? 1 : 0);
        PlayerPrefs.SetInt("numberIA", numberIA);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Credits
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}


