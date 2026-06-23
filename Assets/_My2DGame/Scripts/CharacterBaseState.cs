namespace My2DGame
{
    public abstract class CharacterBaseState
    {
        protected Character character;

        public CharacterBaseState(Character character)
        {
            this.character = character;
        }

        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
        public abstract void HandleJump();
        public abstract void HandleHit();
        public abstract void HandleAttack();
    }
}