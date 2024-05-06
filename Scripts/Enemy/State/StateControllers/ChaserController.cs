using Enemy.State.BaseState.ChaseState;

namespace Enemy.State.StateControllers
{
    public class ChaserController: BaseController
    {
        public override IState.State CheckTransitions()
        {
            //hpが0以下なら死亡状態に遷移
            var state = HitPoint.IsDead ? IState.State.Death : IState.State.Chase;
            return state;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.Chase, new ChaseState(Enemy));
        }
    }
}