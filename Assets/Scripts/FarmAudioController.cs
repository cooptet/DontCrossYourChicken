using UnityEngine;

namespace ChickenGame.Audio {
    [DisallowMultipleComponent]
    public class FarmAudioController : MonoBehaviour {
        [Header("Background Music")]
        [SerializeField] private AudioClip backgroundMusic;

        [SerializeField, Range(0f, 1f)]
        private float musicVolume = 0.2f;

        [Header("Chicken Sounds")]
        [SerializeField] private AudioClip chickenAmbient;

        // Boş bırakılırsa ortam için atanan tavuk sesi kullanılır.
        [SerializeField] private AudioClip chickenHit;

        [SerializeField, Range(0f, 1f)]
        private float ambientVolume = 0.1f;

        [SerializeField, Range(0f, 1f)]
        private float hitVolume = 0.45f;

        [Header("Ambient Timing")]
        [SerializeField] private bool ambientEnabled = true;

        [SerializeField]
        private Vector2 ambientInterval = new Vector2(5f, 9f);

        [SerializeField, Min(0f)]
        private float backgroundStartSeconds = 0f;

        private AudioSource musicSource;
        private AudioSource ambientSource;
        private AudioSource hitSource;

        private float nextAmbientTime;
        private bool backgroundPaused;

        private void Awake() {
            musicSource = CreateSource("Farm Music", true);
            ambientSource = CreateSource("Chicken Ambient", false);
            hitSource = CreateSource("Chicken Hit", false);

            ApplyVolumes();
        }

        private void OnEnable() {
            backgroundPaused = false;
            
            PlayBackground();

            ScheduleNextAmbient();
            UpdateBackgroundPause();
        }

        private void Update() {
            ApplyVolumes();
            UpdateBackgroundPause();

            if (!backgroundPaused &&
                backgroundMusic != null &&
    !           musicSource.isPlaying) {
                PlayBackground();
            }

            if (!ambientEnabled) {
                ambientSource.Stop();
                return;
            }

            if (backgroundPaused || chickenAmbient == null)
                return;

            if (Time.time < nextAmbientTime)
                return;

            if (ambientSource.isPlaying || hitSource.isPlaying)
                return;

            ambientSource.pitch = Random.Range(0.95f, 1.05f);
            ambientSource.PlayOneShot(chickenAmbient);

            ScheduleNextAmbient();
        }

        public void PlayChickenHit() {
            if (!isActiveAndEnabled || Time.timeScale <= 0f)
                return;

            AudioClip clip = chickenHit != null
                ? chickenHit
                : chickenAmbient;

            if (clip == null)
                return;

            // Ortam sesiyle çarpma sesi üst üste binmesin.
            ambientSource.Stop();
            hitSource.Stop();

            hitSource.volume = hitVolume;
            hitSource.pitch = Random.Range(0.95f, 1.05f);
            hitSource.PlayOneShot(clip);

            ScheduleNextAmbient();
        }

        private void PlayBackground() {
            if (backgroundMusic == null)
                return;

            // Tekrar başladığında belirlediğimiz saniyeden oynatacağız.
            musicSource.loop = false;
            musicSource.clip = backgroundMusic;

            musicSource.time = Mathf.Clamp(
                backgroundStartSeconds,
                0f,
                Mathf.Max(0f, backgroundMusic.length - 0.01f)
            );

            musicSource.Play();
        }

        private AudioSource CreateSource(string sourceName, bool loop) {
            GameObject child = new GameObject(sourceName);
            child.transform.SetParent(transform, false);

            AudioSource source = child.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.loop = loop;
            source.spatialBlend = 0f;
            source.dopplerLevel = 0f;

            return source;
        }

        private void ApplyVolumes() {
            musicSource.volume = musicVolume;
            ambientSource.volume = ambientVolume;
            hitSource.volume = hitVolume;
        }

        private void ScheduleNextAmbient() {
            nextAmbientTime = Time.time
                + Random.Range(ambientInterval.x, ambientInterval.y);
        }

        private void UpdateBackgroundPause() {
            bool shouldPause = Time.timeScale <= 0f;

            if (shouldPause == backgroundPaused)
                return;

            backgroundPaused = shouldPause;

            if (shouldPause) {
                musicSource.Pause();
                ambientSource.Pause();
            }
            else {
                musicSource.UnPause();
                ambientSource.UnPause();
            }
        }

        private void OnDisable() {
            if (musicSource != null)
                musicSource.Stop();

            if (ambientSource != null)
                ambientSource.Stop();

            if (hitSource != null)
                hitSource.Stop();
        }

        private void OnValidate() {
            ambientInterval.x = Mathf.Max(1f, ambientInterval.x);

            ambientInterval.y = Mathf.Max(
                ambientInterval.x,
                ambientInterval.y
            );
        }
    }


}