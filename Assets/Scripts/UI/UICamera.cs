using UnityEngine;

public class UICamera : MonoBehaviour {
    [Range(1, 10)]
    public float rotationSpeed = 5f;

    void Update() {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
