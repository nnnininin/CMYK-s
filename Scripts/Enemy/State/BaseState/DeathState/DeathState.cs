namespace Enemy.State.BaseState.DeathState
{
    public class DeathState : BaseState
    {
        public DeathState(IEnemy enemy)
            : base(enemy) { }

        public override IState.State State => IState.State.Death;
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnEntry()
        {
            Die();
        }
        protected override void OnUpdate() { }

        protected override void OnFixedUpdate() { }
        protected override void OnExit() { }
    }
}
