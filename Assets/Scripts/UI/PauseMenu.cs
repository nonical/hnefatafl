using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject pauseMenuUI;
    public GameObject gameFinishedUI;
    public GameObject mainMenuUI;
    public GameObject onlineMenuUI;
    public bool isGamePaused = false;

    private void Start() {
        toggleInputScript(false);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                Resume();
            } else {
                if (!gameFinishedUI.activeSelf) {
                    Pause();
                }
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
        SceneManager.LoadScene("SampleScene");
        mainMenuUI.SetActive(true);
        toggleInputScript(false);
    }

    public void Quit() {
        Application.Quit();
    }

    public void playLocal() {
        Time.timeScale = 1f;//unfreeze time
        GameMemory.Multiplayer = false;
        mainMenuUI.SetActive(false);
        toggleInputScript(true);
    }

    public void playOnline() {
        onlineMenuUI.SetActive(true);

    }
    public void backToMenu() {
        onlineMenuUI.SetActive(false);
    }
}
