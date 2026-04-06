using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;

    public float viewAngle = 60f;
    
    private void Update()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if(angle < viewAngle /2)
        {
            Debug.Log("플레이어가 시야 안에 있음");    
        }
    }
}
