using UnityEngine;

public class InputManagement : MonoBehaviour {
    private Transform selectedPiece;
    private bool isAttackerTurn = true;
    private bool pieceMoving = false;

    private void Start() {
        MovementLogic.FigureMoved += (GameObject _, (int, int) __) => {
            isAttackerTurn = !isAttackerTurn;
            pieceMoving = false;
            selectedPiece = null;
        };
    }

    void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();

            if (selectionRenderer != null) {
                if (Input.GetMouseButtonDown(0)) {
                    if (pieceMoving) return;

                    // on figure click
                    if (selection.CompareTag("Selectable") || selection.CompareTag("King")) {
                        if (isAttackerTurn && !selection.name.StartsWith("playerA") || !isAttackerTurn && (selection.name.StartsWith("playerA"))) {
                            return;
                        }

                        selectedPiece = selection;
                        MovementLogic.HighlightViableMoves(selectedPiece.gameObject);
                    }

                    // halt if no piece selected
                    if (selectedPiece == null) return;

                    // on tile click
                    if (selection.CompareTag("Highlight") ||
                        ((selection.CompareTag("KingTile") || selection.CompareTag("DeathTile")) && selectedPiece.CompareTag("King"))) {
                        pieceMoving = true;
                        MovementLogic.MovePiece(selectedPiece.gameObject, selection.gameObject);
                    };
                }
            }
        }
    }
}
