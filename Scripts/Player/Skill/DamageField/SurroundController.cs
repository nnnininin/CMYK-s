using UnityEngine;

namespace Player.Skill.DamageField
{
    public class SurroundController : MonoBehaviour, IMoveController
    {
        private bool _isMoving;

        public void ActivateMove(Vector3 direction, float speed)
        {
            _isMoving = true;
        }

        private void PerformCircularMotion()
        {
            if (!_isMoving) return;

            transform.localPosition = Vector3.zero;
        }

        public void FixedUpdate()
        {
            PerformCircularMotion();
        }
    }
}