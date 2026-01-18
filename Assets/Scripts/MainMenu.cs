using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelSettings;
    public GameObject panelInfo;
    public GameObject panelWin;
    public GameObject panelLose;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void StartGame(int playersCount)
    {

       
        if (playersCount == 1)
        {
            SaveScript.isPlayingAgainstAI = true;
        }
        else
        {
            SaveScript.isPlayingAgainstAI = false;
        }
        SceneManager.LoadScene("GameP1");
    }

   

    public void OpenSettings()
    {
        Time.timeScale = 0.0f;
        panelSettings.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        Time.timeScale = 1.0f;
        panelSettings.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void OpenInfo()
    {
        Time.timeScale = 0.0f;
        panelInfo.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void CloseInfo()
    {
        Time.timeScale = 1.0f;
        panelInfo.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void FinishGame(int player1Score, int player2Score)
    {
        panelMenu.SetActive(false);
        if (player1Score > player2Score)
        {
            panelWin.SetActive(true);
        }
        else 
        {
            panelLose.SetActive(true);
        }
      
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}