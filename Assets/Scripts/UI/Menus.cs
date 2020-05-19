using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject pauseMenuUI;
    public GameObject gameFinishedUI;
    public GameObject mainMenuUI;
    public GameObject onlineMenuUI;
    public GameObject teamPickUI;
    public GameObject ipAddressInput;
    public bool isGamePaused = false;
    public NetworkManager networkManager;
    public Camera UICamera;

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
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("SampleScene");
        networkManager.StopHost();
    }

    public void Quit() {
        Application.Quit();
    }

    public void playLocal() {
        RenderSettings.fog = false;
        GameMemory.AttackerTurn = true;
        GameMemory.Multiplayer = false;
        mainMenuUI.SetActive(false);
        toggleInputScript(true);
        UICamera.enabled = false;
    }

    public void playOnline() {
        GameMemory.AttackerTurn = true;
        mainMenuUI.SetActive(false);
        onlineMenuUI.SetActive(true);
        GameMemory.Multiplayer = true;
    }

    public void backToMenu() {
        mainMenuUI.SetActive(true);
        onlineMenuUI.SetActive(false);
    }

    public void hostGame() {
        onlineMenuUI.SetActive(false);
        teamPickUI.SetActive(true);
    }

    private void startHosting() {
        teamPickUI.SetActive(false);
        toggleInputScript(true);
        networkManager.StartHost();
        UICamera.enabled = false;
        RenderSettings.fog = false;
    }

    public void hostAsAttacker() {
        GameMemory.teamTag = Tags.TeamTag.Attackers;
        startHosting();
    }

    public void hostAsDefender() {
        GameMemory.teamTag = Tags.TeamTag.Defenders;
        startHosting();
    }

    public void backToOnlineUI() {
        teamPickUI.SetActive(false);
        onlineMenuUI.SetActive(true);
    }

    public void joinGame() {
        var ip = ipAddressInput.GetComponent<TMP_InputField>().text;

        if (string.IsNullOrEmpty(ip)) {
            return;
        }

        onlineMenuUI.SetActive(false);
        toggleInputScript(true);

        networkManager.networkAddress = ip;
        networkManager.StartClient();
        UICamera.enabled = false;
    }
}
