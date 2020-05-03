using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject pauseMenuUI;
    public bool isGamePaused = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void toggleInputScript(bool val) {
        pieceManager.GetComponent<InputManagement>().enabled = val;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        toggleInputScript(true);
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        toggleInputScript(false);

    }

    public void Exit() {
        toggleInputScript(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

    public void Quit() {
        Application.Quit();
    }
}
