using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
 public class ButtonHoldInput : MonoBehaviour
    {
        //unity input systemを使用しコントローラー・キーボードマウス両方に対応
        private IPlayer _player;
        
        //上下どちらかは押されているか？
        //押されていない場合は0、押されている場合は1か-1
        //1か-1のどちらかが入るかは最後に入力された方向による
        //上下入力で最後に入力された方向を保持する
        private bool _isUpLatestInput;
        private bool _isUpInput;
        private bool _isDownInput;
        
        //左右入力で最後に入力された方向を保持する
        private bool _isRightLatestInput;
        private bool _isRightInput;
        private bool _isLeftInput;
        
        public void Start()
        {
            _player = GetComponent<IPlayer>();
        }

        //ホールドしている間はtrue、離したらfalseになる
         public void OnShotInput(InputAction.CallbackContext context)
         {
             //押下されているかどうかのみを判定
             var isShotInput = context switch //switch式
             {
                 { phase: InputActionPhase.Performed } => true,
                 { phase: InputActionPhase.Canceled } => false,
                 _ => _player.IsShotInput
             };
             _player.SetIsShotInput(isShotInput);
         }
        public void OnUpInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isUpLatestInput = true;
                    _isUpInput = true;
                    break;
                case InputActionPhase.Canceled:
                    _isUpLatestInput = false;
                    _isUpInput = false;
                    break;
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Performed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var direction = new Vector2(_player.InputMoveDirection.x, GetVerticalInput());
            _player.SetInputMoveDirection(direction);
        }
        public void OnDownInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isUpLatestInput = false;
                    _isDownInput = true;
                    break;
                case InputActionPhase.Canceled:
                    _isUpLatestInput = true;
                    _isDownInput = false;
                    break;
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Performed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var direction = new Vector2(_player.InputMoveDirection.x, GetVerticalInput());
            _player.SetInputMoveDirection(direction);
        }
        
        public void OnRightInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isRightLatestInput = true;
                    _isRightInput = true;
                    break;
                case InputActionPhase.Canceled:
                    _isRightLatestInput = false;
                    _isRightInput = false;
                    break;
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Performed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var direction = new Vector2(GetHorizontalInput(), _player.InputMoveDirection.y);
            _player.SetInputMoveDirection(direction);
        }
        
        public void OnLeftInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isRightLatestInput = false;
                    _isLeftInput = true;
                    break;
                case InputActionPhase.Canceled:
                    _isRightLatestInput = true;
                    _isLeftInput = false;
                    break;
                case InputActionPhase.Disabled:
                    break;
                case InputActionPhase.Waiting:
                    break;
                case InputActionPhase.Performed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var direction = new Vector2(GetHorizontalInput(), _player.InputMoveDirection.y);
            _player.SetInputMoveDirection(direction);
        }

        private int GetVerticalInput()
        {
            if (!_isUpInput && !_isDownInput)
            {
                return 0;
            }
            return _isUpLatestInput ? 1 : -1;
        }

        private int GetHorizontalInput()
        {
            if (!_isRightInput && !_isLeftInput)
            {
                return 0;
            }
            return _isRightLatestInput ? 1 : -1;
        }
    }
}
