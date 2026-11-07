using UnityEngine;

[RequireComponent(typeof(FlockAgent))]
public class ChickenRepel : MonoBehaviour
{
    public float enterDuration = 1.5f;
    public float stayDuration = 1.5f;

    FlockAgent agent;

    void Awake()
    {
        agent = GetComponent<FlockAgent>();
    }

    bool IsObstacle(Collision c)
    {
        return c.gameObject.CompareTag("Player") || c.gameObject.CompareTag("Obstacle");
    }

    void OnCollisionEnter(Collision c)
    {
        if (IsObstacle(c))
            agent.Repel(c.GetContact(0).point, enterDuration);
    }

    void OnCollisionStay(Collision c)
    {
        if (IsObstacle(c))
            agent.Repel(c.GetContact(0).point, stayDuration);
    }
}
