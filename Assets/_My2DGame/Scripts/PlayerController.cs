using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    /// <summary>
    /// 플레이어 캐릭터의 움직임과 행동을 제어하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables   
        private static readonly int CannotMoveHash = Animator.StringToHash("CannotMove");
        private static readonly int AttackTriggerHash = Animator.StringToHash("AttackTrigger");
        private static readonly int YVelocityHash = Animator.StringToHash("YVelocity");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        private static readonly int IsMoveHash = Animator.StringToHash("IsMove");
        private static readonly int IsRunHash = Animator.StringToHash("IsRun");

        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float jumpForce = 10f;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isMove = false;
        private bool isRun = false;
        private bool isFacingRight = true;
        private bool isGround = true;

        private Animator animator;
        private Collider2D playerCollider;

        [SerializeField] private ContactFilter2D contactFilter;
        private RaycastHit2D[] results = new RaycastHit2D[1];
        [SerializeField] private float groundDistance = 0.05f;
        #endregion

        #region Property
        public bool IsMove
        {
            get => isMove;
            private set
            {
                isMove = value;
                animator.SetBool(IsMoveHash, value);
            }
        }

        public bool IsRun
        {
            get => isRun;
            private set
            {
                isRun = value;
                animator.SetBool(IsRunHash, value);
            }
        }

        public bool IsFacingRight
        {
            get => isFacingRight;
            private set
            {
                if (isFacingRight != value)
                {
                    isFacingRight = value;
                    Vector3 currentScale = transform.localScale;
                    currentScale.x *= -1;
                    transform.localScale = currentScale;
                }
            }
        }

        public bool IsGround
        {
            get => isGround;
            private set
            {
                isGround = value;
                animator.SetBool(IsGroundedHash, value);
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerCollider = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            bool cannotMove = animator.GetBool(CannotMoveHash);

            // 1. 이동 및 애니메이션 상태 처리
            if (!cannotMove)
            {
                // 이동 속도 적용
                float targetSpeed = isRun ? runSpeed : walkSpeed;
                rb.linearVelocity = new Vector2(moveInput.x * targetSpeed, rb.linearVelocity.y);

                // 방향 및 걷기 애니메이션 상태 처리
                if (Mathf.Abs(moveInput.x) > 0.01f)
                {
                    IsMove = true;
                    IsFacingRight = moveInput.x > 0;
                }
                else
                {
                    IsMove = false;
                }
            }
            else
            {
                // 수정: X축 이동만 정지시키고 Y축(중력 및 현재 낙하 속도)은 유지합니다.
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                IsMove = false; // 이동 불가 상태에서는 걷기 애니메이션도 강제로 정지
            }

            // 2. 바닥 감지 처리
            IsGround = playerCollider.Cast(Vector2.down, contactFilter, results, groundDistance) > 0;

            // 3. 추락/상승 애니메이션 처리
            animator.SetFloat(YVelocityHash, rb.linearVelocity.y);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started) IsRun = true;
            else if (context.canceled) IsRun = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // 수정: 이동 불가 상태가 아닐 때만 점프가 가능하도록 조건 추가
            if (context.started && IsGround && !animator.GetBool(CannotMoveHash))
            {
                animator.SetTrigger(JumpTriggerHash);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            // 수정: 이동 불가 상태가 아닐 때만 공격이 가능하도록 조건 추가 (연속 공격 등 예외 상황 제외)
            if (context.started && IsGround && !animator.GetBool(CannotMoveHash))
            {
                animator.SetTrigger(AttackTriggerHash);
            }
        }
        #endregion
    }
}