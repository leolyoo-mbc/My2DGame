using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    /// <summary>
    /// 플레이어 캐릭터의 움직임과 행동을 제어하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isMove = false;
        private bool isRun = false;

        private Animator animator;
        #endregion


        #region Unity Event Method
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
        #endregion

        #region Custom Method
        // New Input System의 "Move" 액션이 연결된 이벤트 함수
        public void OnMove(InputAction.CallbackContext context)
        {
            // ReadValue를 통해 입력값(Vector2)을 가져옵니다.
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed) isRun = true;
            else isRun = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started) Debug.Log("점프키를 started");
            if (context.performed) Debug.Log("점프키를 performed");
            if (context.canceled) Debug.Log("점프키를 canceled");
        }

        private void FixedUpdate()
        {
            // 리지드바디의 속도를 입력받은 x값 * 속도로 설정
            // y값은 현재 속도를 유지하여 점프나 중력 등에 영향받지 않게 함
            if (isRun)
            {
                Debug.Log("왼쪽 쉬프트키를 누르고 있습니다");
                rb.linearVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);
            }
            else rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);

            if (moveInput != Vector2.zero)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }

            animator.SetBool("IsMove", isMove);
            animator.SetBool("IsRun", isRun);
        }
        #endregion
    }
}