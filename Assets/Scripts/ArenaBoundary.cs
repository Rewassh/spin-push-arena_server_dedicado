using UnityEngine;

public class ArenaBoundary : MonoBehaviour
{
    [SerializeField] private float arenaRadius = 6f;

    public bool IsOutsideBounds(Vector3 position)
    {
        Vector2 flatPosition = new Vector2(position.x, position.z);
        return flatPosition.magnitude > arenaRadius;
    }
}