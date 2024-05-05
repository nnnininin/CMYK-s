using UnityEngine;

namespace Player.State.IState.BaseState.UtilState
{
    public class SkillState : BaseState
    {
        public override State State => State.Skill;
        
        //private const string StateName = "IsShot";
        
        public SkillState(IPlayer player) : base(player) { }

        private const float RecoveryTime = 0.5f;
        private float _countTimeInThisState;

        protected override void OverriddenEntry()
        {
            Player.SetIsActionInputEnabled(false);
            _countTimeInThisState = 0f;
            Player.SkillParameter.ResetCoolTimeCount();
            EventManager.TriggerOnEnterSkillState();
        }

        public override void Update()
        {
        }
        
        public override void CountSkillCoolTime()
        {
            //スキル中はクールタイムは減らさない
        }
        
        public override void OverriddenFixedUpdate()
        {
            //_countTimeがRecoveryTimeを超過したらIdleStateに遷移
            _countTimeInThisState += Time.deltaTime;
            if (!(_countTimeInThisState >= RecoveryTime))
                return;
            Player.SetButtonPressedAction(ButtonPressedAction.Idle);
        }

        public override void Exit()
        {
            Player.SetIsActionInputEnabled(true);
        }
    }
}