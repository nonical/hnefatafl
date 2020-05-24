using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Menus : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject pauseMenuUI;
    public GameObject gameFinishedUI;
    public GameObject mainMenuUI;
    public GameObject onlineMenuUI;
    public GameObject teamPickUI;
    public GameObject UPNPErrorUI;
    public GameObject ipAddressInput;
    public GameObject turnMessagesUI;
    public GameObject turnMessageText;
    public bool isGamePaused = false;
    public NetworkManager networkManager;
    public GameObject UICamera;
    public GameObject mainCamera;
    public TMP_Text turnMessageField;
    public TMP_Text ipAddressPlaceholder;
    public AudioClip pageSwitchSound;
    public SoundtrackController soundtrackController;


    private void Start() {
        toggleInputScript(false);
        MovementLogic.FigureMoved += changeTurnMessage;
        NetworkManagerHnefatafl.ClientDisconnect += Exit;
    }

    private void OnDestroy() {
        MovementLogic.FigureMoved -= changeTurnMessage;
        NetworkManagerHnefatafl.ClientDisconnect -= Exit;
    }

    private void changeTurnMessage(GameObject arg1, (int i, int j) arg2) {
        turnMessageField.text = turnMessageField.text.Contains("Attackers Turn") ? "Defenders Turn" : "Attackers Turn";
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                Resume();
            } else {
                if (!gameFinishedUI.activeSelf && !mainMenuUI.activeSelf && !onlineMenuUI.activeSelf && !teamPickUI.activeSelf) {
                    Pause();
                }
            }
        }
    }

    private void toggleInputScript(bool val) {
        pieceManager.GetComponent<InputManagement>().enabled = val;
    }

    public void Resume() {
        soundtrackController.ActivateFilters(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        toggleInputScript(true);
    }

    public void Pause() {
        soundtrackController.ActivateFilters(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        toggleInputScript(false);
    }

    public void Exit() {
        if (networkManager.isNetworkActive) {
            networkManager.StopHost();
        }

        soundtrackController.ActivateFilters(false);
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("SampleScene");
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
        UICamera.SetActive(false);
        turnMessagesUI.SetActive(true);
        mainCamera.SetActive(true);
        soundtrackController.PlayOneShot(pageSwitchSound);
    }

    public void playOnline() {
        GameMemory.AttackerTurn = true;
        mainMenuUI.SetActive(false);
        onlineMenuUI.SetActive(true);
        GameMemory.Multiplayer = true;
        soundtrackController.PlayOneShot(pageSwitchSound);
    }

    public void backToMenu() {
        mainMenuUI.SetActive(true);
        onlineMenuUI.SetActive(false);
        soundtrackController.PlayOneShot(pageSwitchSound);
    }

    public void hostGame() {
        onlineMenuUI.SetActive(false);
        teamPickUI.SetActive(true);
        soundtrackController.PlayOneShot(pageSwitchSound);
    }

    private void startHosting() {
        teamPickUI.SetActive(false);
        try {
            networkManager.StartHost();
        } catch (System.Exception) {
            UPNPErrorUI.SetActive(true);
            return;
        }
        toggleInputScript(true);
        UICamera.SetActive(false);
        RenderSettings.fog = false;
        turnMessagesUI.SetActive(true);
        mainCamera.SetActive(true);
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
        soundtrackController.PlayOneShot(pageSwitchSound);
    }

    public void joinGame() {
        var regex = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
        var ip = ipAddressInput.GetComponent<TMP_InputField>().text;
        bool ismatch = regex.IsMatch(ip);

        if (string.IsNullOrEmpty(ip) || !ismatch) {
            ipAddressInput.GetComponent<TMP_InputField>().text = "";
            ipAddressPlaceholder.color = Color.red;
            return;
        }
        if (ipAddressPlaceholder.color == Color.red) {
            ipAddressPlaceholder.color = turnMessageField.color;
        }
        onlineMenuUI.SetActive(false);
        toggleInputScript(true);

        networkManager.networkAddress = ip;
        networkManager.StartClient();
        UICamera.SetActive(false);
        turnMessagesUI.SetActive(true);
        mainCamera.SetActive(true);
    }

}
