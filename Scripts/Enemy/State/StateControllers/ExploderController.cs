using Enemy.State.BaseState.GoStraightState;
using Enemy.State.BaseState.GoStraightState.ExplodeState;

namespace Enemy.State.StateControllers
{
    public class ExploderController:BaseController
    {
        public override IState.State CheckTransitions()
        {
            //hpが0以下なら爆発状態に遷移
            return Enemy.HitPoint.IsDead ? IState.State.Explode : IState.State.GoStraight;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.Explode, new ExplodeState(Enemy));
            StateContext.AddState(IState.State.GoStraight, new GoStraightState(Enemy));
        }
    }
}