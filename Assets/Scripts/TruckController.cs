
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TruckController : MonoBehaviour {
    [Header("Speed")]
    public float maxForwardSpeed = 10f;
    public float maxReverseSpeed = 4f;

    [Header("Acceleration & Braking")]
    public float acceleration = 4f;
    public float deceleration = 3f;
    public float brakePower = 12f;

    [Header("Steering")]
    public float turnSpeed = 90f;
    public float highSpeedTurnMultiplier = 0.5f;
    public float steeringSmoothness = 5f;

    private Rigidbody rb;

    private float verticalInput;
    private float horizontalInput;
    private bool isBraking;

    private float currentSpeed = 0f;
    private float currentSteering = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void Update() {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        isBraking = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate() {
        HandleMovement();
        HandleSteering();
    }

    void HandleMovement() {
        float targetSpeed = 0f;

        if (!isBraking) {
            if (verticalInput > 0)
                targetSpeed = maxForwardSpeed;
            else if (verticalInput < 0)
                targetSpeed = -maxReverseSpeed;
        }

        float changeRate;

        if (isBraking) {
            changeRate = brakePower;
        }
        else if (Mathf.Abs(verticalInput) < 0.01f) {
            changeRate = deceleration;
        }
        else if (Mathf.Abs(currentSpeed) > 0.01f &&
                 Mathf.Sign(currentSpeed) != Mathf.Sign(targetSpeed)) {
            changeRate = brakePower;
        }
        else {
            changeRate = acceleration;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            changeRate * Time.fixedDeltaTime
        );

        Vector3 movement =
            transform.forward *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    void HandleSteering() {
        currentSteering = Mathf.MoveTowards(
            currentSteering,
            horizontalInput,
            steeringSmoothness * Time.fixedDeltaTime
        );

        float speedRatio = Mathf.Clamp01(
            Mathf.Abs(currentSpeed) / maxForwardSpeed
        );

        float steeringMultiplier = Mathf.Lerp(
            1f,
            highSpeedTurnMultiplier,
            speedRatio
        );

        if (Mathf.Abs(currentSpeed) < 0.05f)
            return;

        float direction = Mathf.Sign(currentSpeed);

        float rotation =
            currentSteering *
            turnSpeed *
            steeringMultiplier *
            direction *
            Time.fixedDeltaTime;

        Quaternion newRotation =
            rb.rotation * Quaternion.Euler(0f, rotation, 0f);

        rb.MoveRotation(newRotation);
    }
}
