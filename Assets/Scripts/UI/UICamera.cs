using UnityEngine;

public class UICamera : MonoBehaviour {
    public Transform Target;
    [Range(0, 10)]
    public float rotationSpeed = 5;

    private void Start() {
        transform.LookAt(Target);
    }

    private void LateUpdate() {
        transform.Translate(Vector3.left * Time.deltaTime * rotationSpeed);
        transform.LookAt(Target);
    }
}
