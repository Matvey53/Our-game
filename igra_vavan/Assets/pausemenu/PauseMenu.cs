using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public GameObject Stats;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        Stats.SetActive(true);
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1.0f;
        PauseGame = false;
    }
    public void Pause()
    {
        Stats.SetActive(false);
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
    }
    public void loadmenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MAIN");
    }
}
