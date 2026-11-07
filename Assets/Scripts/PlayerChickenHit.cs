using UnityEngine;

public class PlayerChickenHit : MonoBehaviour
{
    public float freezeTime = 2f;

    float nextHitTime = 0f;

    void OnCollisionEnter(Collision c)
    {
        TryLog(c.gameObject);
    }

    void OnCollisionStay(Collision c)
    {
        TryLog(c.gameObject);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        TryLog(hit.gameObject);
    }

    void TryLog(GameObject other)
    {
        if (Time.time < nextHitTime) return;
        if (!other.CompareTag("Chicken")) return;

        Debug.Log("You hit your chicken!");
        nextHitTime = Time.time + freezeTime;
    }
}
