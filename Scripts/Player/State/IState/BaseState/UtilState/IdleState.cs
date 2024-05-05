namespace Player.State.IState.BaseState.UtilState
{
    public class IdleState : BaseState
    {

         public override State State => State.Idle;
         public IdleState(IPlayer player) : base(player) { }

         protected override void OverriddenEntry()
         {
             
         }

         public override void Update() { }

         public override void OverriddenFixedUpdate()
         {
         }

         public override void Exit()
         {
         }
    }
}