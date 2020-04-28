using System;
using System.Linq;
using UnityEngine;

public class MovementLogic : MonoBehaviour {
    [SerializeField]
    Material MaterialA, MaterialB, MaterialKing, MaterialTile, MaterialDeath, MaterialHighlight;

    public static Action<GameObject, Tuple<int, int>> FigureMoved;

    static Tuple<int, int> FindFigureIndex(GameObject figure) {
        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                if (GameMemory.Figures[i, j] == figure) {
                    return Tuple.Create(i, j);
                }
            }
        }

        return null;
    }

    static Tuple<int, int> FindTileIndex(GameObject tile, Tuple<int, int> origin) {
        for (int i = 0; i < 11; i++) {
            if (GameMemory.Tiles[origin.Item1, i] == tile) {
                return Tuple.Create(origin.Item1, i);
            }
        }

        for (int i = 0; i < 11; i++) {
            if (GameMemory.Tiles[i, origin.Item2] == tile) {
                return Tuple.Create(i, origin.Item2);
            }
        }

        return null;
    }

    public static void MovePiece(GameObject figure, GameObject destinationTile) {
        // don't allow moving to occupied tile
        if (destinationTile.GetComponent<Tile>().isOccupied) return;

        // note: figures and tiles matrices have same indexes
        Tuple<int, int> origin = FindFigureIndex(figure);
        Tuple<int, int> dest = FindTileIndex(destinationTile, origin);

        // a piece may move horizontally or vertically but not diagonally
        if (dest?.Item1 != origin.Item1 && dest?.Item2 != origin.Item2) return;

        // move figure in matrix
        GameMemory.Figures[origin.Item1, origin.Item2] = null;
        GameMemory.Figures[dest.Item1, dest.Item2] = figure;

        // change tile state
        GameMemory.Tiles[origin.Item1, origin.Item2].GetComponent<Tile>().isOccupied = false;
        destinationTile.GetComponent<Tile>().isOccupied = true;

        // change figure postion
        figure.transform.position = new Vector3(destinationTile.transform.position.x, figure.transform.position.y, destinationTile.transform.position.z);

        // finally, clear highlights
        resetHighlight();

        FigureMoved(figure, dest);
    }

    public static void HighlightViableMoves(GameObject figure) {
        Tuple<int, int> origin = FindFigureIndex(figure);
        bool isKing = figure.CompareTag("King");

        Action<GameObject> ColorTile = (GameObject tile) => {
            // only the King may visit the refugee tiles
            if ((tile.CompareTag("DeathTile") || tile.CompareTag("KingTile")) && isKing == false) return;

            tile.GetComponent<Renderer>().material.color = Color.green;
            tile.gameObject.tag = "Highlight";
        };

        // reset any existing highlighting
        resetHighlight();

        // vertical - down
        for (int i = origin.Item1 + 1; i < 11; i++) {
            var tile = GameMemory.Tiles[i, origin.Item2];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // vertial - up
        for (int i = origin.Item1 - 1; i >= 0; i--) {
            var tile = GameMemory.Tiles[i, origin.Item2];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // horizontal - left
        for (int j = origin.Item2 + 1; j < 11; j++) {
            var tile = GameMemory.Tiles[origin.Item1, j];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }

        // horizontal - right
        for (int j = origin.Item2 - 1; j >= 0; j--) {
            var tile = GameMemory.Tiles[origin.Item1, j];

            if (tile.GetComponent<Tile>().isOccupied) break;

            ColorTile(tile);
        }
    }

    public static void resetHighlight() {
        var higlightedTiles = GameObject.FindGameObjectsWithTag("Highlight").ToList();
        var materialProvider = GameObject.Find("MaterialProvider").GetComponent<MaterialProvider>();

        higlightedTiles.ForEach(tile => {
            if (tile.name.StartsWith("ATile")) {
                tile.GetComponent<Renderer>().material = materialProvider.MaterialA;
            } else if (tile.name.StartsWith("BTile")) {
                tile.GetComponent<Renderer>().material = materialProvider.MaterialB;
            } else if (tile.name.StartsWith("KingTile")) {
                tile.GetComponent<Renderer>().material = materialProvider.MaterialKing;
                tile.tag = "KingTile";
            } else if (tile.name.StartsWith("DeathTile")) {
                tile.GetComponent<Renderer>().material = materialProvider.MaterialDeath;
                tile.tag = "DeathTile";
            } else {
                tile.GetComponent<Renderer>().material = materialProvider.MaterialTile;
            }

            if (tile.CompareTag("Highlight")) {
                tile.tag = "AT";
            }
        });
    }
}
