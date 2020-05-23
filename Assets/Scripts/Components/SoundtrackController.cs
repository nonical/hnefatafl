using UnityEngine;

public class SoundtrackController : MonoBehaviour {
    private static SoundtrackController instance = null;
    private static AudioHighPassFilter highPassFilter;
    private static AudioLowPassFilter lowPassFilter;
    private static AudioSource audioSource;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);

            highPassFilter = GetComponent<AudioHighPassFilter>();
            lowPassFilter = GetComponent<AudioLowPassFilter>();
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void ActivateFilters(bool areFiltersActive) {
        if (areFiltersActive) {
            highPassFilter.cutoffFrequency = 2000;
            lowPassFilter.cutoffFrequency = 2000;
        } else {
            highPassFilter.cutoffFrequency = 10;
            lowPassFilter.cutoffFrequency = 22000;
        }
    }

    public void PlayOneShot(AudioClip clip) => audioSource.PlayOneShot(clip);
}
