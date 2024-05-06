using UnityEngine;

namespace Player.Bullet
{
    public class DistanceRestrictionBullet: Bullet
    {
        //一定距離で消える弾
        
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