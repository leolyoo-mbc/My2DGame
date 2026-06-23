using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("이동 설정")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField, Range(0f, 1f)] private float airControlMultiplier = 0.5f;

    [Header("바닥 감지 설정")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isRunning = false;
    private bool isGrounded = false;

    // 파라미터 해시 캐싱
    private static readonly int IsMoveHash = Animator.StringToHash("IsMove");
    private static readonly int IsRunHash = Animator.StringToHash("IsRun");
    private static readonly int JumpHash = Animator.StringToHash("JumpTrigger");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int YVelocityHash = Animator.StringToHash("YVelocity");
    private static readonly int AttackHash = Animator.StringToHash("AttackTrigger");
    #endregion

    #region Unity Event Method
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
        FlipCharacter();
        UpdateAnimation();
    }

    //유니티 에디터에서 바닥 감지 반경을 시각적으로 보여주는 기능
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    #endregion

    #region Custom Method
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) isRunning = true;
        else if (context.canceled) isRunning = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger(JumpHash);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            animator.SetTrigger(AttackHash);
        }
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            // groundCheck 위치를 기준으로 반지름 반경 내에 groundLayer가 있는지 검사합니다.
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Move()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //바닥에 닿아있지 않다면(공중이라면) 속도를 감소시킴
        if (!isGrounded)
        {
            currentSpeed *= airControlMultiplier;
        }

        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
    }

    private void FlipCharacter()
    {
        if (Mathf.Abs(moveInput.x) <= 0.01f) return;

        Vector3 currentScale = transform.localScale;
        currentScale.x = moveInput.x > 0 ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
        transform.localScale = currentScale;
    }

    private void UpdateAnimation()
    {
        bool isMoving = Mathf.Abs(moveInput.x) > 0.01f;

        animator.SetBool(IsMoveHash, isMoving);
        animator.SetBool(IsRunHash, isMoving && isRunning);

        animator.SetBool(IsGroundedHash, isGrounded);
        animator.SetFloat(YVelocityHash, rb.linearVelocity.y);
    }
    #endregion
}