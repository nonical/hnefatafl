using System.Collections.Generic;
using Tags;
using UnityEngine;

public class GameMemory : MonoBehaviour {
    public static GameObject[,] Figures { get; } = new GameObject[11, 11];
    public static GameObject[,] Tiles { get; } = new GameObject[11, 11];
    private static Dictionary<(int i, int j), Vector3> SpawnDirections { get; set; }

    Transform Inner, TeamA, TeamB;

    public GameObject PlayerA_Prefab, PlayerB_Prefab, PlayerKing_Prefab;

    [Range(0.5f, 10)]
    public float spawnHeightOffset = 5;

    public static bool Multiplayer = false;
    public static bool AttackerTurn = true;
    void Start() {
        LoadSpawnDirections();

        Inner = transform.GetChild(0).GetChild(0);
        TeamA = transform.GetChild(1);
        TeamB = transform.GetChild(2);

        AssignAllTiles();
        AssignAllFigures();

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

        if (tile.CompareTag(TileTags.Haven) || tile.CompareTag(TileTags.Accessible)) {
            return null;
        }

        if (tile.CompareTag(TileTags.King)) {
            figure = Instantiate(PlayerKing_Prefab);
        }

        if (tile.CompareTag(TileTags.SpawnA)) {
            figure = Instantiate(PlayerA_Prefab);
        }

        if (tile.CompareTag(TileTags.SpawnB)) {
            figure = Instantiate(PlayerB_Prefab);
        }

        figure.transform.SetParent(figure.CompareTag(FigureTags.TeamA) ? TeamA : TeamB);
        figure.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + spawnHeightOffset, tile.transform.position.z);

        figure.GetComponent<Piece>().i = tile.GetComponent<Tile>().i;
        figure.GetComponent<Piece>().j = tile.GetComponent<Tile>().j;

        figure.transform.eulerAngles = SpawnDirections[GetIndices(figure)];

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
        figure.GetComponent<Piece>().i = dest.i;
        figure.GetComponent<Piece>().j = dest.j;

        // change turn
        AttackerTurn = !AttackerTurn;
    }

    static void LoadSpawnDirections() {
        SpawnDirections = new Dictionary<(int i, int j), Vector3> {
            // Row 0
            { (0, 3), FacingDirection.Down },
            { (0, 4), FacingDirection.Down },
            { (0, 5), FacingDirection.Down },
            { (0, 6), FacingDirection.Down },
            { (0, 7), FacingDirection.Down },

            // Row 1
            { (1, 5), FacingDirection.Down },

            // Row 3
            { (3, 0), FacingDirection.Right },
            { (3, 5), FacingDirection.Up },
            { (3, 10), FacingDirection.Left },

            // Row 4
            { (4, 0), FacingDirection.Right },
            { (4, 4), FacingDirection.Left / 2 },
            { (4, 5), FacingDirection.Up },
            { (4, 6), FacingDirection.Right / 2 },
            { (4, 10), FacingDirection.Left },

            // Row 5
            { (5, 0), FacingDirection.Right },
            { (5, 1), FacingDirection.Right },
            { (5, 3), FacingDirection.Left },
            { (5, 4), FacingDirection.Left },
            { (5, 5), FacingDirection.Down },
            { (5, 6), FacingDirection.Right },
            { (5, 7), FacingDirection.Right },
            { (5, 9), FacingDirection.Left },
            { (5, 10), FacingDirection.Left },

            // Row 6
            { (6, 0), FacingDirection.Right },
            { (6, 4), FacingDirection.Down + FacingDirection.Right / 2 },
            { (6, 5), FacingDirection.Down },
            { (6, 6), FacingDirection.Down - FacingDirection.Right / 2 },
            { (6, 10), FacingDirection.Left },

            // Row 7
            { (7, 0), FacingDirection.Right },
            { (7, 5), FacingDirection.Down },
            { (7, 10), FacingDirection.Left },

            // Row 9
            { (9, 5), FacingDirection.Up },

            // Row 10
            { (10, 3), FacingDirection.Up },
            { (10, 4), FacingDirection.Up },
            { (10, 5), FacingDirection.Up },
            { (10, 6), FacingDirection.Up },
            { (10, 7), FacingDirection.Up }
        };
    }
}
