using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //? Main Menu buttons
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject StartGameBtn;
    [SerializeField] private GameObject SettingsBtn;
    [SerializeField] private GameObject CreditsBtn;
    [SerializeField] private GameObject QuitBtn;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject CreditsScreen;
    [SerializeField] private GameObject SettingsScreen;
    public GameObject LogInScreen;

    private bool waitingForButtonPress = false;

    void Update()
    {
        if (waitingForButtonPress)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                waitingForButtonPress = false;
                PauseMenu.isPaused = false;
                Debug.Log("Mouse or key pressed");
                SceneManager.LoadScene(1);// Load Scene 1
            }
        }
    }

    //? Different button functionalities
    public void OnStartButton()
    {
        loadingScreen.SetActive(true);
        waitingForButtonPress = true;
    }

    public void OnSettingsButton()
    {
        SettingsScreen.SetActive(true);
    }

    public void OnCreditsButton()
    {
        CreditsScreen.SetActive(true);
    }

    public void OnLogInButton(){
        LogInScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!!");
        Application.Quit();
    }

    public void OnBackButton()
    {
        CreditsScreen.SetActive(false);
        SettingsScreen.SetActive(false);
    }
}
