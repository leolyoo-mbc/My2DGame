using System;
using UnityEngine;

namespace My2DGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        public Rigidbody2D rb { get; private set; }
        public Animator anim { get; private set; }

        [Header("스탯")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float jumpForce = 10f;

        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
        public float JumpForce => jumpForce;

        // 1. 상태 객체들을 직접 참조 (성능 최적화)
        public CharacterGroundState GroundState { get; private set; }
        //public CharacterAirState AirState { get; private set; }
        //public CharacterAttackState AttackState { get; private set; }
        //public CharacterHitState HitState { get; private set; }
        //public CharacterDeathState DeathState { get; private set; }

        public CharacterBaseState currentState { get; private set; }

        // 2. 뇌(Controller)가 주입하는 '의도(Intent)' 데이터
        public float MoveIntent { get; private set; }
        public bool RunIntent { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            // 상태 인스턴스 생성 및 초기화
            GroundState = new CharacterGroundState(this);
            //AirState = new CharacterAirState(this);
            //AttackState = new CharacterAttackState(this);
            //HitState = new CharacterHitState(this);
            //DeathState = new CharacterDeathState(this);

            ChangeState(GroundState);
        }

        private void Update() => currentState?.LogicUpdate();
        private void FixedUpdate() => currentState?.PhysicsUpdate();

        public void ChangeState(CharacterBaseState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        public void SetMoveIntent(Vector2 intent)
        {
            MoveIntent = intent.x;
        }

        public void SetRunIntent(bool intent)
        {
            RunIntent = intent;
        }

        public void OnJump()
        {
            currentState?.HandleJump();
        }

        public void OnHit()
        {
            currentState?.HandleHit();
        }

        public void OnAttack()
        {
            currentState?.HandleAttack();
        }
    }
}
