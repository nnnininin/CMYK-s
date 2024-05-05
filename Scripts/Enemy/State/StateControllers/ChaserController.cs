using Enemy.State.BaseState.ChaseState;

namespace Enemy.State.StateControllers
{
    public class ChaserController: BaseController
    {
        public override IState.State CheckTransitions()
        {
            var state = HitPoint.IsDead ? IState.State.Death : IState.State.Chase;
            return state;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.Chase, new ChaseState(Enemy));
        }
    }
}