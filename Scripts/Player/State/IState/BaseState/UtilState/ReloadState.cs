using UnityEngine;

namespace Player.State.IState.BaseState.UtilState
{
    public class ReloadState : BaseState
    {
        public override State State => State.Reload;
        public ReloadState(IPlayer player) : base(player) { }

        private float _reloadTimer;
        private float _accumulator;

        protected override void OverriddenEntry()
        {
            _accumulator = 0f;
            Player.SetIsActionInputEnabled(false);
            _reloadTimer = Player.ReloadTime.ReloadTimeValue.Value;
            EventManager.TriggerOnEntryReloadState(_reloadTimer);
        }

        public override void Update()
        {
        }

        public override void OverriddenFixedUpdate()
        {
            _accumulator += Time.fixedDeltaTime;
            if (!(_accumulator >= _reloadTimer)) return;
            Player.Magazine.Reload();
            Player.SetButtonPressedAction(ButtonPressedAction.Idle);
        }

        public override void Exit()
        {
            Player.SetIsActionInputEnabled(true);
        }
    }
}