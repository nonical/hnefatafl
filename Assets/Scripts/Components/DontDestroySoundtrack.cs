using UnityEngine;

public class DontDestroySoundtrack : MonoBehaviour {
    static DontDestroySoundtrack instance = null;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
