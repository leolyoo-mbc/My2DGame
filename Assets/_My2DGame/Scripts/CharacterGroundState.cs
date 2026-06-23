namespace My2DGame
{
    public class CharacterGroundState : CharacterBaseState
    {
        public CharacterGroundState(Character character) : base(character)
        {

        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleHit()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleJump()
        {
            throw new System.NotImplementedException();
        }

        public override void LogicUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void PhysicsUpdate()
        {
            float currentSpeed = character.RunIntent ? character.RunSpeed : character.WalkSpeed;
            character.rb.linearVelocityX = character.MoveIntent * currentSpeed;
        }
    }
}