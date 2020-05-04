using System;
using System.Linq;
using UnityEngine;

public class WinConditions : MonoBehaviour {
    public GameObject pieceManager;
    public GameObject gameFinishedUI;

    public static Action AttackersWin;
    public static Action DefendersWin;

    void Start() {
        MovementLogic.FigureMoved += CheckHavenTiles;
        AttackersWin += AttackersWinMessage;
        DefendersWin += DefendersWinMessage;
    }

    public static void CheckHavenTiles(GameObject _, (int i, int j) indices) {
        var havenTileIndices = new[] { (0, 0), (0, 10), (10, 0), (10, 10) };

        if (havenTileIndices.Contains(indices)) {
            DefendersWin?.Invoke();
        }
    }

    private void toggleInputScript(bool val) {
        pieceManager.GetComponent<InputManagement>().enabled = val;
    }

    public void DefendersWinMessage() {
        gameFinishedUI.transform.GetChild(0).gameObject.SetActive(true); //display defenders win text
        gameFinishedUI.SetActive(true);
        Time.timeScale = 0f;
        toggleInputScript(false);
        AttackersWin -= AttackersWinMessage;
        DefendersWin -= DefendersWinMessage;
    }

    public void AttackersWinMessage() {
        gameFinishedUI.transform.GetChild(1).gameObject.SetActive(true); //display attackers win text
        gameFinishedUI.SetActive(true);
        Time.timeScale = 0f;
        toggleInputScript(false);
        AttackersWin -= AttackersWinMessage;
        DefendersWin -= DefendersWinMessage;
    }
}
