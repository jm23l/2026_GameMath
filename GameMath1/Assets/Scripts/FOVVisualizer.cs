using UnityEngine;

public class FOVVisualizer : MonoBehaviour
{
    public float viewAngle = 60f;

    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;


        Vector3 leftBoundry = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Vector3 rightBoundry = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundry);
        Gizmos.DrawRay(transform.position, rightBoundry);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);

    }
}
