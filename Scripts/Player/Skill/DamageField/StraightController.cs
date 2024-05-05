using System;
using UnityEngine;

namespace Player.Skill.DamageField
{
    public class StraightController : MonoBehaviour, IMoveController
    {
        private bool _isMoving;
        private Vector3 _initialDirection;
        private float _speed;
        private float _startTime;
        private Vector3 _normalRight;
        public float accelerationCoefficient = 20.0f; // 加速度係数

        public void ActivateMove(Vector3 direction, float speed)
        {
            _isMoving = true;
            _initialDirection = direction.normalized; // 方向を正規化
            _speed = speed;
            _startTime = Time.fixedTime; // 移動開始時の時間
            _normalRight = Vector3.Cross(_initialDirection, Vector3.up).normalized; // 右手法線ベクトル
        }

        public void FixedUpdate()
        {
            if (!_isMoving) return;

            // 経過時間の計算
            var elapsedTime = Time.fixedTime - _startTime;
            
            // 基本の移動速度ベクトル
            var baseVelocity = _initialDirection * _speed;
            
            // 時間によって変化する速度成分の計算（二次関数）
            var dynamicSpeed = accelerationCoefficient * elapsedTime * elapsedTime;
            var dynamicVelocity = _normalRight * dynamicSpeed;
            
            // 合成速度の計算
            var compositeVelocity = baseVelocity + dynamicVelocity;
            
            // 位置の更新
            transform.position += compositeVelocity * Time.fixedDeltaTime;
        }
    }
}