using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject pauseMenuUI;
    public GameObject gameFinishedUI;
    public GameObject mainMenuUI;
    public GameObject onlineMenuUI;
    public GameObject teamPickUI;
    public GameObject ipAddressInput;
    public bool isGamePaused = false;


    private void Awake() {
        Time.timeScale = 0f;
    }
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
        SceneManager.LoadScene("SampleScene");
        mainMenuUI.SetActive(true);
        NetworkManager.singleton.StopHost();
    }

    public void Quit() {
        Application.Quit();
    }

    public void playLocal() {
        GameMemory.AttackerTurn = true;
        Time.timeScale = 1f;//unfreeze time
        GameMemory.Multiplayer = false;
        mainMenuUI.SetActive(false);
        toggleInputScript(true);
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
        Time.timeScale = 1f;
        toggleInputScript(true);
        NetworkManager.singleton.StartHost();
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

        var uriii = new System.Uri($"tcp4://77.221.10.20:7777");

        NetworkManager.singleton.StartClient(uriii);

        onlineMenuUI.SetActive(false);
        Time.timeScale = 1f;

        toggleInputScript(true);

        //Debug.Log(ipAddressInput.GetComponent<TextMeshProUGUI>().text);
    }
}
