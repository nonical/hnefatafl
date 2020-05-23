using UnityEngine;

// AnimationState:
//     0 - idle (loop)
//     1 - walking (loop)
//     2 - attack

public class Piece : IndexedObject {
    [Range(1, 10)]
    public float speed;
    public AudioClip slashSound;
    public AudioClip hitSound;

    private GameObject moveTarget;
    private Vector3 movePos;
    private Animator animator;
    private AudioSource audioSource;

    private void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            audioSource.loop = false;
        }
    }

    public void AttackAnimation() {
        animator.SetInteger("AnimationState", 2);
        audioSource.PlayOneShot(slashSound);
        audioSource.PlayOneShot(hitSound);
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

        audioSource.loop = true;
        audioSource.Play();
    }
}
