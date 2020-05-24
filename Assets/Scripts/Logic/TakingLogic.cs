using System;
using System.Collections.Generic;
using System.Linq;
using Tags;
using UnityEngine;

public class TakingLogic : MonoBehaviour {
    public Transform CratePosition;
    public ParticlesHandler particlesHandler;

    private List<GameObject> FigureList = new List<GameObject>(3);
    private List<GameObject> TilesList = new List<GameObject>(3);

    void Start() {
        MovementLogic.FigureMoved += CheckTaking;
    }

    void FigureToCrate(GameObject figure) {
        float randomX = UnityEngine.Random.Range(-0.7f, 0.7f);
        float randomZ = UnityEngine.Random.Range(-0.3f, 0.3f);

        particlesHandler.DeathParticleEffect(figure);

        figure.transform.parent = CratePosition;
        figure.transform.localPosition = new Vector3(randomX, 0, randomZ);
        figure.tag = FigureTags.Captured;
    }

    void CheckTaking(GameObject player, (int, int) indices) {
        Func<GameObject, GameObject, bool> TakeLogic = (GameObject tile, GameObject figure) => {
            // tile must be either: a) occupied, b) not occupied and be a refugee tile
            if (!tile.GetComponent<Tile>().isOccupied && !tile.CompareTag(TileTags.Haven) && !tile.CompareTag(TileTags.King)) {
                return false;
            }

            // the attacking team can only use the King's tile as an anvil if it's not occupied
            if (player.CompareTag(FigureTags.TeamA) && tile.CompareTag(TileTags.King) && tile.GetComponent<Tile>().isOccupied) {
                return false;
            }

            // taking can be done with: a) two pieces of the same team, b) one piece and a refugee tile,
            // c) one piece of the defending team and the King
            if (figure == null || figure.CompareTag(player.tag) || tile.CompareTag(TileTags.Haven) || tile.CompareTag(TileTags.King) ||
               (player.CompareTag(FigureTags.TeamB) && figure.CompareTag(FigureTags.King)) ||
               (player.CompareTag(FigureTags.King) && figure.CompareTag(FigureTags.TeamB))) {
                if (FigureList.Count > 0) {
                    player.GetComponent<Piece>().AttackAnimation();

                    // if the King is captured - defenders lose
                    if (FigureList.Any(x => x.CompareTag(FigureTags.King))) {
                        WinConditions.AttackersWin?.Invoke();
                    }

                    FigureList.ForEach(enemyFigure => FigureToCrate(enemyFigure));
                    TilesList.ForEach(enemyTile => enemyTile.GetComponent<Tile>().isOccupied = false);
                }

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

    private void OnDestroy() {
        MovementLogic.FigureMoved -= CheckTaking;
    }
}
