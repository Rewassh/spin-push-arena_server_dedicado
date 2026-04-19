using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private ArenaBoundary arenaBoundary;

    private Rigidbody playerRigidbody;
    private bool isMovingForward;

    public bool IsMovingForward => isMovingForward;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isMovingForward = Input.GetKey(KeyCode.W);

        if (arenaBoundary != null && arenaBoundary.IsOutsideBounds(transform.position))
{
    Debug.Log(gameObject.name + " is out of bounds!");
}
    }
    

    private void FixedUpdate()
    {
        if (isMovingForward)
        {
            MoveForward();
            return;
        }

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        float rotationStep = rotationSpeed * Time.fixedDeltaTime;
        Quaternion rotationOffset = Quaternion.Euler(0f, rotationStep, 0f);

        playerRigidbody.MoveRotation(playerRigidbody.rotation * rotationOffset);
    }

    private void MoveForward()
    {
        Vector3 forwardStep = transform.forward * moveSpeed * Time.fixedDeltaTime;
        Vector3 targetPosition = playerRigidbody.position + forwardStep;

        playerRigidbody.MovePosition(targetPosition);
    }
}