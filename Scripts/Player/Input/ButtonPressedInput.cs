using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
 public class ButtonPressedInput : MonoBehaviour
    {
        //unity input systemを使用しコントローラー・キーボードマウス両方に対応
        private Player _player;
        public void Start()
        {
            _player = GetComponent<Player>();
        }
        //以下のメソッドはinput systemのコールバックとして登録
        public void OnReloadInput(InputAction.CallbackContext context)
        {
            //アクション可能な状態でなければreturn
            if(!_player.IsActionInputEnabled) return;
            var buttonPressedAction = context switch
            {
                { phase: InputActionPhase.Performed } => ButtonPressedAction.Reload,
                _ => _player.ButtonPressedAction
            };
            _player.SetButtonPressedAction(buttonPressedAction);
        }
        public void OnSkillInput(InputAction.CallbackContext context)
        {
            //アクション可能な状態でなければreturn
            if(!_player.IsActionInputEnabled) return;
            //クールタイム中はreturn
            if( _player.SkillParameter.CoolTimeCount.Value > 0) return;
            var buttonPressedAction = context switch
            {
                { phase: InputActionPhase.Performed } => ButtonPressedAction.Skill,
                _ => _player.ButtonPressedAction
            };
            _player.SetButtonPressedAction(buttonPressedAction);
        }
    }
}
