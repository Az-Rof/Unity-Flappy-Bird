using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //public GameObject StartMenu, SettingsMenu, ExitMenu;
    public List<GameObject> PopUp = new List<GameObject>();
    public Button StartButton, SettingsButton, ExitButton;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StartButton != null &&
         SettingsButton != null &&
         ExitButton != null)
        {
            StartButton.onClick.AddListener(PopUp_StartGame);
            ExitButton.onClick.AddListener(PopUp_quit);
            SettingsButton.onClick.AddListener(PopUp_Settings);
        }
    }


    // Popup is called
    public void PopUp_StartGame()
    {
        foreach (GameObject PopUp in PopUp)
        {
            if (PopUp.name == "StartGame")
            {
                PopUp.SetActive(true);
            }
            else
            {
                PopUp.SetActive(false);
            }
        }
    }
    public void PopUp_quit()
    {
        foreach (GameObject PopUp in PopUp)
        {
            if (PopUp.name == "ExitGame")
            {
                PopUp.SetActive(true);
            }
            else
            {
                PopUp.SetActive(false);
            }
        }
    }
    public void PopUp_Settings()
    {
        foreach (GameObject PopUp in PopUp)
        {
            if (PopUp.name == "SettingsGame")
            {
                PopUp.SetActive(true);
            }
            else
            {
                PopUp.SetActive(false);
            }
        }
    }

    //Function
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        // Dapatkan nama scene yang sedang berjalan
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Muat ulang scene yang sama
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1f;
    }
    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}