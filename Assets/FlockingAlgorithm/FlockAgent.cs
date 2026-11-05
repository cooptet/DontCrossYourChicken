using UnityEngine;

[RequireComponent (typeof(Collider))]

public class FlockAgent : MonoBehaviour
{
    Collider agentCollider;
    public Collider AgentCollider {  get { return agentCollider; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Move(Vector3 velocity)
    {
        velocity.y = 0f;
        transform.forward = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
