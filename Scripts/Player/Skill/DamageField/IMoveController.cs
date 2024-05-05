using UnityEngine;

namespace Player.Skill.DamageField
{
    public interface IMoveController
    {
        public void ActivateMove(Vector3 direction, float speed);
        public void FixedUpdate();
    }
}