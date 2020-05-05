﻿using UnityEngine;

// AnimationState:
//     0 - idle (loop)
//     1 - walking (loop)
//     2 - attack

public class Piece : IndexedObject {
    [Range(1, 10)]
    public float speed;
    private GameObject moveTarget;
    private Vector3 movePos;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (moveTarget == null) return;

        animator.SetInteger("AnimationState", 1);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movePos, step);

        // when piece has finished moving fire the FigureMoved event
        if (Vector3.Distance(transform.position, movePos) < 0.001f) {
            animator.SetInteger("AnimationState", 0);
            MovementLogic.FigureMoved(gameObject, GameMemory.GetIndices(moveTarget));
            moveTarget = null;
        }
    }

    public void AttackAnimation() {
        animator.SetInteger("AnimationState", 2);
    }

    public void MovePiece(GameObject target) {
        var (targetI, targetJ) = GameMemory.GetIndices(target);

        if (targetI == i) {
            transform.eulerAngles = targetJ < j ? FacingDirection.Left : FacingDirection.Right;
        } else {
            transform.eulerAngles = targetI < i ? FacingDirection.Up : FacingDirection.Down;
        }

        moveTarget = target;

        movePos = target.transform.position;
        movePos.y = transform.position.y;
    }
}