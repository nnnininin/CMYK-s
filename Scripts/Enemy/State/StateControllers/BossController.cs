using Enemy.State.BaseState.SummonState;

namespace Enemy.State.StateControllers
{
    public class BossController: BaseController
    {
        public override IState.State CheckTransitions()
        {
            var state = HitPoint.IsDead ? IState.State.Death : IState.State.Summon;
            return state;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.Summon, new SummonState(Enemy));
        }
    }
}