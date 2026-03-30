using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float detectionRange = 8f;
    public float dashSpeed = 15f;
    public float stopDistance = 1.2f;
    public bool isDashing = false;
    public float viewAngle = 60;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        Vector3 dirToPlayer = toPlayer.normalized;

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        
        float dot = forward.x * dirToPlayer.x
                  + forward.y * dirToPlayer.y
                  + forward.z * dirToPlayer.z;

        dot = Mathf.Clamp(dot, -1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (!isDashing)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            if (angle <= viewAngle && distance <= detectionRange)
            {
                isDashing = true;
            }
        }
        else
        { 
            if(distance  > stopDistance)
            {
                Vector3 dashDir = player.position - transform.position;
                dashDir.y = 0f;
                dashDir.Normalize();

                transform.rotation = Quaternion.LookRotation(dashDir);
                transform.position += dashDir * dashSpeed * Time.deltaTime;
            }
            else
            {
                CheckParry();
                isDashing = false;
            }
        }
    }

    void CheckParry()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc == null) return;

        Vector3 PlayerForward = player.forward;
        PlayerForward.y = 0f;
        PlayerForward.Normalize();

        Vector3 toEnemy = transform.position - player.position;
        toEnemy.y = 0f;
        toEnemy.Normalize();

        Vector3 cross = new Vector3(PlayerForward.y * toEnemy.z - PlayerForward.z * toEnemy.y,
                                    PlayerForward.z * toEnemy.x - PlayerForward.x * toEnemy.z,
                                    PlayerForward.x * toEnemy.y - PlayerForward.y * toEnemy.x);
        float crossY = cross.y;

        if (crossY > 0f)
        {
            if (pc.isRightParrying)
            {
                Destroy(gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            if(pc.isLeftParrying)
            {
                Destroy(gameObject);

            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
        }
        
    }
}
