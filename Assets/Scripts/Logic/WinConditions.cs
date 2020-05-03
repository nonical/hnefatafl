using System;
using System.Linq;
using UnityEngine;

public class WinConditions : MonoBehaviour {
    public static Action<string> AttackersWin;
    public static Action<string> DefendersWin;

    void Start() {
        MovementLogic.FigureMoved += CheckHavenTiles;
    }

    public static void CheckHavenTiles(GameObject _, (int i, int j) indices) {
        var havenTileIndices = new[] { (0, 0), (0, 10), (10, 0), (10, 10) };

        if (havenTileIndices.Contains(indices)) {
            DefendersWin?.Invoke("King reached a haven tile!");
        }
    }
}
