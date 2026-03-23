using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 5f;
    public float viewAngle = 90f;
    public float scaleSpeed = 5f;       

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;

        dir.y = 0;
        float distance = dir.magnitude;

        Vector3 dirNormalized = dir.normalized;

        Vector3 forward = transform.forward;
      

        float dot = Vector3.Dot(forward, dirNormalized);

        float halfAngle = viewAngle / 2f ;

        float limit = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

        Vector3 targetScale;

        if(distance <= viewDistance && dot >= limit)
        {
            targetScale = Vector3.one * 2f; 
        }
        else
        {
            targetScale = Vector3.one;
        }
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
        
    }
}
