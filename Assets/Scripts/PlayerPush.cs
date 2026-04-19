using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPush : MonoBehaviour
{
    [Header("Push Settings")]
    [SerializeField] private float pushDistance = 1.2f;
    [SerializeField] private float pushCooldown = 0.25f;

    private PlayerMovement playerMovement;
    private float lastPushTime;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerMovement == null || !playerMovement.IsMovingForward)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (Time.time - lastPushTime < pushCooldown)
        {
            return;
        }

        Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (otherRigidbody == null)
        {
            return;
        }

        Vector3 pushDirection = transform.forward.normalized;
        Vector3 targetPosition = otherRigidbody.position + pushDirection * pushDistance;

        if (otherRigidbody.isKinematic)
        {
            otherRigidbody.position = targetPosition;
        }
        else
        {
            otherRigidbody.MovePosition(targetPosition);
            otherRigidbody.linearVelocity = Vector3.zero;
            otherRigidbody.angularVelocity = Vector3.zero;
        }

        lastPushTime = Time.time;
    }
}