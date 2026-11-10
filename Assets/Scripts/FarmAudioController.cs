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
        private float ambientStartSeconds = 0f;

        [SerializeField, Min(0f)]
        private float backgroundStartSeconds = 0f;

        [Header("Hit Sound Duration")]
        [SerializeField, Min(0.05f)]
        private float hitDuration = 0.45f;

        [SerializeField, Min(0.01f)]
        private float hitFadeOut = 0.08f;

        [SerializeField]
        private bool continuousAmbient = true;

        private float hitEndTime;

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
            UpdateHitSound();
            UpdateBackgroundPause();

            if (!backgroundPaused &&
                backgroundMusic != null &&
                !musicSource.isPlaying) {
                PlayBackground();
            }

            if (!ambientEnabled) {
                ambientSource.Stop();
                return;
            }

            if (backgroundPaused || chickenAmbient == null)
                return;

            if (!continuousAmbient && Time.time < nextAmbientTime)
                return;

            if (ambientSource.isPlaying)
                return;

            ambientSource.pitch = Random.Range(0.95f, 1.05f);

            ambientSource.clip = chickenAmbient;

            ambientSource.time = Mathf.Clamp(
                ambientStartSeconds,
                0f,
                Mathf.Max(0f, chickenAmbient.length - 0.01f)
            );

            ambientSource.Play();

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

        public void PlayChickenHit() {
            if (!isActiveAndEnabled || Time.timeScale <= 0f)
                return;

            AudioClip clip = chickenHit != null
                ? chickenHit
                : chickenAmbient;

            if (clip == null)
                return;

            //ambientSource.Stop();
            hitSource.Stop();

            hitSource.loop = false;
            hitSource.volume = hitVolume;
            hitSource.pitch = Random.Range(0.95f, 1.05f);

            hitEndTime = Time.unscaledTime + hitDuration;
            hitSource.PlayOneShot(clip);

            ScheduleNextAmbient();
        }

        private void UpdateHitSound() {
            if (!hitSource.isPlaying)
                return;

            float remaining = hitEndTime - Time.unscaledTime;

            if (remaining <= 0f) {
                hitSource.Stop();
                return;
            }

            float fadeDuration = Mathf.Max(
                0.01f,
                Mathf.Min(hitFadeOut, hitDuration)
            );

            hitSource.volume = hitVolume
                * Mathf.Clamp01(remaining / fadeDuration);
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