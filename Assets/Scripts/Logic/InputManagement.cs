using UnityEngine;

public class InputManagement : MonoBehaviour {
    private Transform selectedPiece;
    private bool isAttackerTurn = true;

    // Update is called once per frame
    void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var selection = hit.transform;

            if (selection != null) {
                if (Input.GetMouseButtonDown(0)) {
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

                        MovementLogic.MovePiece(selectedPiece.gameObject, selection.gameObject);
                        selectedPiece = null;
                        isAttackerTurn = !isAttackerTurn;
                        DebugHelper.LogFigures();
                        DebugHelper.LogTiles();
                    };
                }
            }
        }
    }
}
