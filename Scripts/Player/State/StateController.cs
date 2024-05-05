using System;
using UnityEngine;

namespace Player.State
{
    public class StateController: MonoBehaviour
    {
        private IPlayer _player;
        private void Awake()
        {
            _player = GetComponent<IPlayer>();
        }
        private void Update() => CheckState();

        //現在のStateを判断し、適切なStateに遷移する
        private void CheckState()
        {
            //最初にactionのswitch文によって遷移先のstateを決定
            switch (_player.ButtonPressedAction)
            {
                case ButtonPressedAction.None:
                    break;
                case ButtonPressedAction.Idle:
                    _player.Context.ChangeState(IState.State.Idle);
                    _player.SetButtonPressedAction(ButtonPressedAction.None);
                    return;
                case ButtonPressedAction.Skill:
                    _player.Context.ChangeState(IState.State.Skill);
                    _player.SetButtonPressedAction(ButtonPressedAction.None);
                    return;
                case ButtonPressedAction.Reload:
                    _player.Context.ChangeState(IState.State.Reload);
                    _player.SetButtonPressedAction(ButtonPressedAction.None);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            //操作可能な状態でなければreturn
            if(!_player.IsActionInputEnabled) return;
            ShotInput();
        }

        private void ShotInput()    //ボタンを押し続けている間の処理
        {
            switch (_player.IsShotInput)
            {
                //押下中かつShotStateではない:ShotStateに遷移
                case true when _player.Context.CurrentState.State != IState.State.Shot:
                    _player.Context.ChangeState(IState.State.Shot);
                    return;
                //押下していないかつShotState:IdleStateに遷移
                case false when _player.Context.CurrentState.State == IState.State.Shot:
                    _player.Context.ChangeState(IState.State.Idle);
                    break;
            }
        }
        
    }
}