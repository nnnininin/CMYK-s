using UnityEngine;

namespace Player.Bullet
{
    public class DistanceRestrictionBullet: Bullet
    {
        [SerializeField]
        private float disappearDistance;
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            var sqrDistance = (transform.position - SpawnPosition).sqrMagnitude;
            if (sqrDistance > disappearDistance * disappearDistance)
            {
                ReturnToPool(this);
            }
        }
    }
}