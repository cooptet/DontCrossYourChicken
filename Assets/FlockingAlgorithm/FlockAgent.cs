using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

public class FlockAgent : MonoBehaviour
{
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    Rigidbody rb;

    Vector3 currentVelocity;
    Vector3 targetVelocity;

    [Range(1f, 20f)] public float acceleration = 4f;
    [Range(1f, 20f)] public float turnSpeed = 5f;

    float repelTimer;
    Vector3 repelDirection;

    void Awake()
    {
        agentCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Repel(Vector3 awayFromPosition, float duration = 1f)
    {
        repelDirection = transform.position - awayFromPosition;
        repelDirection.y = 0f;
        if (repelDirection.sqrMagnitude < 0.001f)
            repelDirection = -transform.forward;
        repelDirection.Normalize();
        repelTimer = duration;
    }

    public void Move(Vector3 velocity)
    {
        if (repelTimer > 0f)
        {
            repelTimer -= Time.deltaTime;
            velocity = repelDirection * 2f;
        }

        velocity.y = 0f;
        targetVelocity = velocity;
    }

    void FixedUpdate()
    {
        //Debug.Log(name + " target: " + targetVelocity + " rb: " + rb.linearVelocity);
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        if (currentVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(currentVelocity);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, turnSpeed * Time.fixedDeltaTime));
        }

        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }
}
