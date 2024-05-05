using UnityEngine;

namespace Player.Util
{
    public class MoveController : MonoBehaviour
    {
        private IPlayer _player;
        [SerializeField]
        private AudioSource audioSource; 
        
        private const int Degree = 0;
        private const float Theta = Degree * Mathf.Deg2Rad;
        private float _cosTheta;
        private float _sinTheta;
        private float _moveSpeed;
        
        private void Awake()
        {
            _player = GetComponent<IPlayer>();
            audioSource = GetComponent<AudioSource>(); // AudioSourceを取得
        }
        
        private void Start()
        {
            _cosTheta = Mathf.Cos(Theta);
            _sinTheta = Mathf.Sin(Theta);
        }
        
        private void FixedUpdate()
        {
            var moveDirectionX = _player.InputMoveDirection.x;
            var moveDirectionY = _player.InputMoveDirection.y;
            var rotatedX = moveDirectionX * _cosTheta - moveDirectionY * _sinTheta;
            var rotatedZ = moveDirectionX * _sinTheta + moveDirectionY * _cosTheta;

            var speed = _player.MoveSpeed.IsMoveLow ? 
                _player.MoveSpeed.MoveSpeedValueLow.Value : 
                _player.MoveSpeed.MoveSpeedValue.Value;
            
            var velocity = new Vector3(rotatedX, 0, rotatedZ) * speed;
            
            if (_player.Context.CurrentState.State == State.IState.State.Shot)
            {
                var mouseInputDirection = new Vector3(_player.MouseInputDirection.x, 0, _player.MouseInputDirection.y);
                _player.ChildTransform.rotation = Quaternion.LookRotation(mouseInputDirection);
            }
            else
            {
                _player.ChildTransform.rotation = velocity != Vector3.zero ? Quaternion.LookRotation(velocity) : _player.ChildTransform.rotation;
            }

            _player.SetRunAnimation(velocity.sqrMagnitude > 0);
            _player.SetVelocity(velocity);

            // velocityが0より大きく、現在サウンドが再生されていない場合にサウンドを再生
            if (velocity.magnitude > 0 && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}