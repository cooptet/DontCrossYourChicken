using System;
using UnityEngine;

public class PlayerChickenHit : MonoBehaviour
{
    public float freezeTime = 1f;

    float nextHitTime = 0f;
    
    [SerializeField] HealthbarController healthbarController;

    private void Start()
    {
        if (healthbarController == null)
        {
            Debug.Log("Help");
        }
    }

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
        
        healthbarController.ReduceHearts();
    }
}
