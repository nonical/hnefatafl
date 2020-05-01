using System;
using System.Collections.Generic;
using UnityEngine;

public class TakingLogic : MonoBehaviour {
    public List<GameObject> FigureList { get; set; } = new List<GameObject>(3);
    public List<GameObject> TilesList { get; set; } = new List<GameObject>(3);

    void Start() {
        MovementLogic.FigureMoved += CheckTaking;
    }

    void CheckTaking(GameObject player, (int, int) indices) {
        Func<GameObject, GameObject, bool> TakeLogic = (GameObject tile, GameObject figure) => {
            // tile must be either: a) occupied, b) not occupied and be a refugee tile
            if (!tile.GetComponent<Tile>().isOccupied && !tile.CompareTag("DeathTile") && !tile.CompareTag("KingTile")) {
                return false;
            }

            // the attacking team can only use the King's tile as an anvil if it's not occupied
            if (player.name.StartsWith("playerA") && tile.CompareTag("KingTile") && tile.GetComponent<Tile>().isOccupied) {
                return false;
            }

            // taking can be done with: a) two pieces of the same team, b) one piece and a refugee tile,
            // c) one piece of the defending team and the King
            if (figure?.name == player.name || tile.CompareTag("DeathTile") || tile.CompareTag("KingTile")
                || (player.name.StartsWith("playerB") && figure.CompareTag("King"))
                || (player.name.StartsWith("playerKing") && figure.name.StartsWith("playerB"))) {
                FigureList.ForEach(enemyFigure => Destroy(enemyFigure));
                TilesList.ForEach(enemyTile => enemyTile.GetComponent<Tile>().isOccupied = false);

                return false;
            }
            // up to three pieces can be taken
            FigureList.Add(figure);
            TilesList.Add(tile);

            return true;
        };

        // horizontal - left
        for (int i = 1; i <= 4 && indices.Item2 - i >= 0; i++) {
            var tile = GameMemory.Tiles[indices.Item1, indices.Item2 - i];
            var figure = GameMemory.Figures[indices.Item1, indices.Item2 - i];

            if (!TakeLogic(tile, figure)) break;
        }

        FigureList.Clear();
        TilesList.Clear();

        // horizontal - right
        for (int i = 1; i <= 4 && indices.Item2 + i < 11; i++) {
            var tile = GameMemory.Tiles[indices.Item1, indices.Item2 + i];
            var figure = GameMemory.Figures[indices.Item1, indices.Item2 + i];

            if (!TakeLogic(tile, figure)) break;
        }

        FigureList.Clear();
        TilesList.Clear();

        // vertical - up
        for (int i = 1; i <= 4 && indices.Item1 - i >= 0; i++) {
            var tile = GameMemory.Tiles[indices.Item1 - i, indices.Item2];
            var figure = GameMemory.Figures[indices.Item1 - i, indices.Item2];

            if (!TakeLogic(tile, figure)) break;
        }

        FigureList.Clear();
        TilesList.Clear();


        // vertical - down
        for (int i = 1; i <= 4 && indices.Item1 + i < 11; i++) {
            var tile = GameMemory.Tiles[indices.Item1 + i, indices.Item2];
            var figure = GameMemory.Figures[indices.Item1 + i, indices.Item2];

            if (!TakeLogic(tile, figure)) break;
        }

        FigureList.Clear();
        TilesList.Clear();
    }
}
