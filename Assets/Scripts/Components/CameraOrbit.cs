using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    public Transform Target;
    [Range(0.01f, 1.00f)]
    public float SmoothFactor = 0.1f;
    [Range(1f, 10f)]
    public float RotationSpeed = 2.5f;

    private Vector3 CameraOffset;

    private void Start() {
        CameraOffset = transform.position - Target.transform.position;
        transform.LookAt(Target);
    }

    private void LateUpdate() {
        if (Input.GetMouseButton(1)) {
            RotateCamera();
        }
    }

    private void RotateCamera() {
        Vector3 position = Target.transform.position + CameraOffset;
        Quaternion angle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);

        CameraOffset = angle * CameraOffset;
        transform.position = Vector3.Slerp(transform.position, position, SmoothFactor);

        transform.LookAt(Target);
    }
}
