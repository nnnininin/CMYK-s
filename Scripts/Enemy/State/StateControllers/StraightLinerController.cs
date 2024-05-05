using Enemy.State.BaseState.GoStraightState;

namespace Enemy.State.StateControllers
{
    public class StraightLinerController: BaseController
    {
        public override IState.State CheckTransitions()
        {
            return Enemy.HitPoint.IsDead ? IState.State.Death : IState.State.GoStraight;
        }
        public override void AddUniqueStates()
        {
            StateContext.AddState(IState.State.GoStraight, new GoStraightState(Enemy));
        }
    }
}