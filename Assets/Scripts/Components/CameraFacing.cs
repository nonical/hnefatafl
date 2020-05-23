using UnityEngine;

public class CameraFacing : MonoBehaviour {
    public Camera CameraUI;
    public Camera CameraMain;

    private Camera currentCamera;

    private void Start() {
        currentCamera = CameraUI;
    }

    private void Update() {
        if (!CameraUI.isActiveAndEnabled) currentCamera = CameraMain;

        Vector3 v = currentCamera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(currentCamera.transform.position - v);
    }
}
