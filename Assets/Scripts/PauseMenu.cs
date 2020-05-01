using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;

    public GameObject pieceManager;

    public GameObject pauseMenuUI;
    // Update is called once per frame

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void toggleInputScript(bool val)
    {
        pieceManager.GetComponent<InputManagement>().enabled = val;

    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        toggleInputScript(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        toggleInputScript(false);
        
    }

    public void Exit()
    {
        toggleInputScript(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }
}
