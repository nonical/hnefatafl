using Tags;
using UnityEngine;

public class InputManagement : MonoBehaviour {
    private Transform selectedPiece;
    private bool pieceMoving = false;
    private NetworkHandlers networkHandlers;

    private void Start() {
        MovementLogic.FigureMoved += (GameObject _, (int, int) __) => {
            pieceMoving = false;
            selectedPiece = null;
        };

        networkHandlers = GetComponent<NetworkHandlers>();
    }

    void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            var selection = hit.transform;

            if (Input.GetMouseButtonDown(0)) {
                if (pieceMoving) return;

                // on figure click
                if (selection.CompareTag(FigureTags.TeamA) || selection.CompareTag(FigureTags.TeamB) || selection.CompareTag(FigureTags.King)) {
                    if (GameMemory.AttackerTurn && !selection.CompareTag(FigureTags.TeamA) ||
                        !GameMemory.AttackerTurn && selection.CompareTag(FigureTags.TeamA)) {
                        return;
                    }

                    if (GameMemory.Multiplayer) {
                        if (GameMemory.teamTag == TeamTag.Attackers && !selection.CompareTag(FigureTags.TeamA)) return;
                        if (GameMemory.teamTag == TeamTag.Defenders && selection.CompareTag(FigureTags.TeamA)) return;
                    }

                    selectedPiece = selection;
                    MovementLogic.HighlightViableMoves(selectedPiece.gameObject);
                    return;
                }

                // halt if no piece selected
                if (selectedPiece == null) return;

                // on tile click
                if (selection.GetComponent<Tile>().isHighlighted) {
                    pieceMoving = true;

                    if (GameMemory.Multiplayer) {
                        networkHandlers.SendMoveMessage(GameMemory.GetIndices(selectedPiece.gameObject), GameMemory.GetIndices(selection.gameObject));
                    } else {
                        MovementLogic.MovePiece(selectedPiece.gameObject, selection.gameObject);
                    }
                };
            }
        }
    }
}
