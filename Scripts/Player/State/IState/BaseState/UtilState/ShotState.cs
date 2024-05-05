using UnityEngine;

namespace Player.State.IState.BaseState.UtilState
{
    public class ShotState : BaseState
    {
        private float _shotTimer;
        private float _intervalSeconds;
        
        public override State State => State.Shot;

        public ShotState(IPlayer player) : base(player) { }

        protected override void OverriddenEntry()
        {
            EventManager.TriggerOnEntryShotState();
            _shotTimer = 0f;
            _intervalSeconds = Player.FireRate.FireRateValue.Value;
            Player.MoveSpeed.SetIsMoveLow(true);
        }

        public override void Update() { }

        public override void OverriddenFixedUpdate()
        {
            _shotTimer += Time.fixedDeltaTime;
            if (!(_shotTimer >= _intervalSeconds))
                return;
            _shotTimer -= _intervalSeconds; //発射タイマーリセット
            
            //弾がなければリロード
            if (Player.Magazine.CurrentBullet.Value <= 0)
            {
                Player.SetButtonPressedAction(ButtonPressedAction.Reload);
                return;   
            }
            EventManager.TriggerOnShotStart();
        }

        public override void Exit()
        {
            EventManager.TriggerOnExitShotState();
            Player.MoveSpeed.SetIsMoveLow(false);
        }
    }
}