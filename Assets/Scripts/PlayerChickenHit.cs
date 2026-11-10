using UnityEngine;
using ChickenGame.Audio;

public class PlayerChickenHit : MonoBehaviour {
    public float freezeTime = 2f;

    private float nextHitTime = 0f;

    [SerializeField]
    private HealthbarController healthbarController;

    [SerializeField]
    private FarmAudioController farmAudio;

    private void Start() {
        if (healthbarController == null) {
            Debug.LogWarning(
                "PlayerChickenHit: Healthbar Controller atanmadı.",
                this
            );
        }

        if (farmAudio == null) {
            Debug.LogWarning(
                "PlayerChickenHit: Farm Audio atanmadı.",
                this
            );
        }
    }

    private void OnCollisionEnter(Collision c) {
        TryLog(c.gameObject);
    }

    private void OnCollisionStay(Collision c) {
        TryLog(c.gameObject);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        TryLog(hit.gameObject);
    }

    private void TryLog(GameObject other) {
        if (Time.time < nextHitTime)
            return;

        if (!other.CompareTag("Chicken"))
            return;

        if (healthbarController == null)
            return;

        Debug.Log("You hit your chicken!");

        nextHitTime = Time.time + freezeTime;

        // Hasara izin verilen temas için tavuk sesini çal.
        if (farmAudio != null)
            farmAudio.PlayChickenHit();

        healthbarController.ReduceHearts();
    }
}