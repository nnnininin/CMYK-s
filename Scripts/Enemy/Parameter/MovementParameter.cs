using Enemy.ScriptableObject;
using UnityEngine;

namespace Enemy.Parameter
{
    public class MovementParameter
    {
        public float MovingSpeed { get;private set;}
        //追跡時の精度
        public float ChasingAccuracy { get; private set; }
        private Transform Target { get; }
        private Rigidbody Rigidbody { get; }
        
        private float _originalSpeed;
        
        private const float DecelerationDuration = 5.0f;
        private float _timeToRestoreDeceleration;

        public Vector3 TargetPosition => Target.position;
        public Vector3 CurrentPosition=> Rigidbody.position;
        public Vector3 CurrentVelocity => Rigidbody.velocity;
        
        
        public MovementParameter( 
            EnemyScriptableObject enemyScriptableObject,
            Transform target,
            Rigidbody rigidbody
            )
        {
            MovingSpeed = enemyScriptableObject.MovingSpeed;
            ChasingAccuracy = enemyScriptableObject.ChasingAccuracy * 0.1f;
            Target = target;
            Rigidbody = rigidbody;
            _timeToRestoreDeceleration = 0;
        }
        
        public void SetCurrentVelocity(Vector3 velocity)
        {
            Rigidbody.velocity = velocity;
        }

        private void SetSpeed(float speed)
        {
            MovingSpeed = speed;
        }
        
        public void DecelerateSpeed()
        {
            Debug.Log("DecelerateVelocity");
            if(_timeToRestoreDeceleration> 0) return;
            const float decelerationRate = 0.2f;
            _originalSpeed = MovingSpeed;
            var newVelocity = _originalSpeed * decelerationRate;
            _timeToRestoreDeceleration = DecelerationDuration;
            SetSpeed(newVelocity);
        }

        public void RestoreCountDown(float deltaTime)
        {
            if (_timeToRestoreDeceleration <= 0) return;
            _timeToRestoreDeceleration -= deltaTime;
            if (_timeToRestoreDeceleration > 0) return;
            _timeToRestoreDeceleration = 0;
            SetSpeed(_originalSpeed);
        }
    }
}