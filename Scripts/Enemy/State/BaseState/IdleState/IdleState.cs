namespace Enemy.State.BaseState.IdleState
{
    public class IdleState : BaseState
    {
        public IdleState(IEnemy enemy)
            : base(enemy) { }
        
        public override IState.State State => IState.State.Idle;

        protected override void OnEntry()
        {
        }
        protected override void OnUpdate() { }

        protected override void OnFixedUpdate()
        {
        }
        protected override void OnExit() { }
    }
}
