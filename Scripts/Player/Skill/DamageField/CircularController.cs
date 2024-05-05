using UnityEngine;

namespace Player.Skill.DamageField
{
    public class CircularController : MonoBehaviour, IMoveController
    {
        private bool _isMoving;
        private float _currentRadius = 0f;
        private const float ExpansionSpeed = 3.0f;
        private const float MaxRadius = 5.0f; // 最大半径の定数
        private float _rotationSpeed;
        private float _angle;

        public void ActivateMove(Vector3 direction, float speed)
        {
            _isMoving = true;
            _rotationSpeed = speed; // 回転速度を設定
            _currentRadius = direction.magnitude; // directionの大きさを初期半径とする
            _angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg; // 初期角度を設定
        }

        private void PerformCircularMotion()
        {
            if (!_isMoving) return;

            _angle += _rotationSpeed * Time.fixedDeltaTime; // 角度を更新

            if (_currentRadius < MaxRadius) // 半径が最大に達していなければ
            {
                _currentRadius += ExpansionSpeed * Time.fixedDeltaTime; // 半径を拡大
                _currentRadius = Mathf.Min(_currentRadius, MaxRadius); // 半径が最大値を超えないようにする
            }

            // 新しい相対位置を計算
            var radian = _angle * Mathf.Deg2Rad;
            var newLocalPosition = new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)) * _currentRadius;

            transform.localPosition = newLocalPosition; // オブジェクトの局所位置を更新
        }

        public void FixedUpdate()
        {
            PerformCircularMotion();
        }
    }
}