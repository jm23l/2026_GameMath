using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 30f;
    private Vector2 moveInput;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;

    public void OnLeftParry(InputValue value)
    {
        isLeftParrying = value.isPressed;
    }

    public void OnRightParry(InputValue value)
    {
        isRightParrying = value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        Vector3 moveDir = moveInput.y * moveSpeed * Time.deltaTime * transform.forward;
        transform.position += moveDir;
    }
}
