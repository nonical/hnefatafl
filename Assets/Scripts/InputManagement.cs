using UnityEngine;

public class InputManagement : MonoBehaviour {
    private Transform selectedPiece;

    // Update is called once per frame
    void Update() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();

            if (selectionRenderer != null) {
                if (Input.GetMouseButtonDown(0)) {
                    // on figure click
                    if (selection.CompareTag("Selectable") || selection.CompareTag("King")) {
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

                        DebugHelper.LogFigures();
                        DebugHelper.LogTiles();
                    };
                }
            }
        }
    }
}
