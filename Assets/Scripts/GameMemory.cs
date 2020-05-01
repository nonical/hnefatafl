using System.Collections.Generic;
using UnityEngine;

public class GameMemory : MonoBehaviour {
    public static GameObject[,] Figures { get; private set; } = new GameObject[11, 11];
    public static GameObject[,] Tiles { get; private set; } = new GameObject[11, 11];

    Transform Inner, TeamA, TeamB;

    [SerializeField]
    GameObject PlayerA_Prefab, PlayerB_Prefab, PlayerKing_Prefab;

    [SerializeField, Range(1, 10)]
    float spawnHeightOffset;

    // Start is called before the first frame update
    void Start() {
        Inner = transform.GetChild(0).GetChild(0);
        TeamA = transform.GetChild(1);
        TeamB = transform.GetChild(2);

        AssignAllTiles();
        AssignAllFigures();

        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                if (Tiles[i, j].CompareTag("Spawn_Team_A") || Tiles[i, j].CompareTag("Spawn_Team_B")) {
                    Tiles[i, j].tag = "AT";
                }
            }
        }

        MovementLogic.FigureMoved += MoveFigure;
    }

    List<GameObject> GetAllChildren(GameObject parent) {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++) {
            children.Add(parent.transform.GetChild(i).gameObject);
        }

        return children;
    }

    void AssignAllTiles() {
        List<GameObject> rows = GetAllChildren(Inner.gameObject);

        for (int i = 0; i < 11; i++) {
            List<GameObject> temp = GetAllChildren(rows[i]);

            for (int j = 0; j < 11; j++) {
                temp[j].GetComponent<Tile>().i = i;
                temp[j].GetComponent<Tile>().j = j;
                Tiles[i, j] = temp[j];
            }
        }
    }

    GameObject MakeNewFigure(GameObject tile) {
        GameObject figure = null;

        if (tile.CompareTag("DeathTile") || tile.CompareTag("AT")) {
            return null;
        }

        if (tile.CompareTag("KingTile")) {
            figure = Instantiate(PlayerKing_Prefab);
        }

        if (tile.CompareTag("Spawn_Team_A")) {
            figure = Instantiate(PlayerA_Prefab);
        }

        if (tile.CompareTag("Spawn_Team_B")) {
            figure = Instantiate(PlayerB_Prefab);
        }

        figure.transform.SetParent(tile.CompareTag("Spawn_Team_A") ? TeamA : TeamB);
        figure.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + spawnHeightOffset, tile.transform.position.z);

        figure.GetComponent<IndexedObject>().i = tile.GetComponent<Tile>().i;
        figure.GetComponent<IndexedObject>().j = tile.GetComponent<Tile>().j;

        return figure;
    }

    void AssignAllFigures() {
        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                Figures[i, j] = MakeNewFigure(Tiles[i, j]);
            }
        }
    }

    public static (int, int) GetIndices(GameObject obj) {
        var indexedObj = obj.GetComponent<IndexedObject>();

        return (indexedObj.i, indexedObj.j);
    }

    static void MoveFigure(GameObject figure, (int i, int j) dest) {
        (int i, int j) = GetIndices(figure);

        // move figure in matrix
        Figures[i, j] = null;
        Figures[dest.i, dest.j] = figure;

        // change tile state
        Tiles[i, j].GetComponent<Tile>().isOccupied = false;
        Tiles[dest.i, dest.j].GetComponent<Tile>().isOccupied = true;

        // change indices in figure's IndexedObject component
        figure.GetComponent<IndexedObject>().i = dest.i;
        figure.GetComponent<IndexedObject>().j = dest.j;
    }
}
