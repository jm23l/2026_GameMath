using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 mouseScreenPostion;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting;
    
   
    public void OnPoint(InputValue value)
    {
        mouseScreenPostion = value.Get<Vector2>(); // 마우스 위치 업데이트
    }
    public void OnClick(InputValue value)
    {
        if(value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPostion);
            RaycastHit[] hits = Physics.RaycastAll(ray);   // 레이저 경로에 있는 모든 물체를 탐색

            foreach (RaycastHit hit in hits)   // 모든 물체에 한해 반복
            {

                if (hit.collider.gameObject != gameObject)   // 부딪힌 물체가 나 자신이 아닐 때만
                {

                    targetPosition = hit.point; // Plane에 부딪힌 지점을 타겟
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break; //탐색 했으니 foreach 반복 중단
                }
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed; // 버튼을 누르고 있으면 true, 떼면 false
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float currentSpeed = moveSpeed;                     

            Vector3 direction = (targetPosition - transform.position);
            float sqrMag = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
            float Mag = Mathf.Sqrt(sqrMag);

            if (Mag < 0.1)
            {
                isMoving = false;
                return;
            }

            Vector3 moveDir = new Vector3(direction.x / Mag, direction.y / Mag, direction.z / Mag);

            if (isSprinting)
            {
                currentSpeed = moveSpeed * 2;
            }
            transform.Translate(moveDir * currentSpeed * Time.deltaTime);
            
          
        }
    }

}
// 내가 가는 방향 = 타겟 포지션 - 나의 위치