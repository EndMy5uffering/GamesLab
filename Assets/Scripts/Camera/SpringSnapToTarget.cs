using UnityEngine;

public class SpringSnapToTarget : MonoBehaviour
{
    [Header("Spring Position Settings")]
    [SerializeField] Transform positionTarget;       // Target object to snap position to
    [SerializeField] float positionStiffness = 10f;  // Strength of the position spring
    [SerializeField] float positionDamping = 2f;     // Smoothness of position movement

    [Header("Spring Rotation Settings")]
    [SerializeField] bool enableRotationSpring = false; // Toggle for rotation spring
    [SerializeField] Transform rotationTarget;        // Target for rotation
    [SerializeField] float rotationStiffness = 10f;   // Strength of rotation spring
    [SerializeField] float rotationDamping = 2f;      // Smoothness of rotation movement

    [Header("Rotation Axes")]
    [SerializeField] Vector3 rotationAxis = Vector3.up;             // Primary world-space axis to rotate around
    [SerializeField] Vector3 internalRotationAxis = Vector3.right;  // Secondary local-space axis for additional rotation

    private Vector3 positionVelocity; // Velocity for position spring movement
    private float angularVelocityPrimary;  // Velocity for primary rotation
    private float angularVelocityInternal; // Velocity for internal rotation

    private void Awake()
    {
        if(positionTarget == null)
        {
            //positionTarget = FindAnyObjectByType<PlayerController>()?.transform;
            Debug.LogWarning("SpringSnapToTarget was not assigned a positionTarget.");
        }
    }

    private void Update()
    {
        if (positionTarget != null)
        {
            SpringPosition();
        }

        /* if (enableRotationSpring && rotationTarget != null)
        {
            SpringRotation();
        } */
    }

    private void SpringPosition()
    {
        Vector3 displacement = positionTarget.position - transform.position;
        Vector3 springForce = displacement * positionStiffness;

        positionVelocity += springForce * Time.deltaTime;
        positionVelocity *= Mathf.Exp(-positionDamping * Time.deltaTime); // Exponential decay for damping

        transform.position += positionVelocity * Time.deltaTime;
    }

    private void SpringRotation()
    {
        Vector3 direction = (rotationTarget.position - transform.position).normalized;

        // Compute primary rotation in world space
        Vector3 projectedDirection = Vector3.ProjectOnPlane(direction, rotationAxis).normalized;
        if (projectedDirection == Vector3.zero) return;

        float targetAnglePrimary = Mathf.Atan2(
            Vector3.Dot(projectedDirection, Quaternion.AngleAxis(90, rotationAxis) * transform.forward),
            Vector3.Dot(projectedDirection, transform.forward)
        ) * Mathf.Rad2Deg;

        // Compute internal rotation in local space
        Vector3 localDirection = transform.InverseTransformDirection(direction);
        float targetAngleInternal = Mathf.Atan2(
            Vector3.Dot(localDirection, Quaternion.AngleAxis(90, internalRotationAxis) * Vector3.forward),
            Vector3.Dot(localDirection, Vector3.forward)
        ) * Mathf.Rad2Deg;

        // Apply spring physics to both angles
        float springTorquePrimary = targetAnglePrimary * rotationStiffness;
        angularVelocityPrimary += springTorquePrimary * Time.deltaTime;
        angularVelocityPrimary *= Mathf.Exp(-rotationDamping * Time.deltaTime);

        float springTorqueInternal = targetAngleInternal * rotationStiffness;
        angularVelocityInternal += springTorqueInternal * Time.deltaTime;
        angularVelocityInternal *= Mathf.Exp(-rotationDamping * Time.deltaTime);

        // Apply rotation around the primary (world) axis
        transform.rotation = Quaternion.AngleAxis(angularVelocityPrimary * Time.deltaTime, rotationAxis) * transform.rotation;

        // Apply rotation around the internal (local) axis
        transform.rotation = transform.rotation * Quaternion.AngleAxis(angularVelocityInternal * Time.deltaTime, internalRotationAxis);
    }

    public void SetTarget(Transform target)
    {
        this.positionTarget = target;
    }
}
