using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 캐릭터의 움직임과 행동을 제어하는 클래스
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private Rigidbody2D rb;
        private Vector2 moveInput;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // New Input System의 "Move" 액션이 연결된 이벤트 함수
        public void OnMove(InputAction.CallbackContext context)
        {
            // ReadValue를 통해 입력값(Vector2)을 가져옵니다.
            moveInput = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            // 리지드바디의 속도를 입력받은 x값 * 속도로 설정
            // y값은 현재 속도를 유지하여 점프나 중력 등에 영향받지 않게 함
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }
}