using UnityEngine;

public class Piece : IndexedObject {
    [Range(1, 10)]
    public float speed;
    private GameObject moveTarget;
    private Vector3 movePos;

    void Update() {
        if (moveTarget == null) return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movePos, step);

        // when piece has finished moving fire the FigureMoved event
        if (Vector3.Distance(transform.position, movePos) < 0.001f) {
            MovementLogic.FigureMoved(gameObject, GameMemory.GetIndices(moveTarget));
            moveTarget = null;
        }
    }

    public void MovePiece(GameObject target) {
        moveTarget = target;

        movePos = target.transform.position;
        movePos.y = transform.position.y;
    }
}
