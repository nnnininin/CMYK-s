using Enemy.State.BaseState.GoStraightState;

namespace Enemy.State.StateControllers
{
    public class StraightLinerController: BaseController
    {
        public override IState.State CheckTransitions()
        {
            //hpが0以下なら死亡状態に遷移
            return Enemy.HitPoint.IsDead ? IState.State.Death : IState.State.GoStraight;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.GoStraight, new GoStraightState(Enemy));
        }
    }
}