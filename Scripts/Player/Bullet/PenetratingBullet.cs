using UnityEngine;
using System.Collections.Generic;

namespace Player.Bullet
{
    public class PenetratingBullet: Bullet
    {
        private readonly List<Enemy.IEnemy> _hitEnemies = new();
        
        protected override void CastRayToCurrentPosition(Vector3 worldPosition)
        {
            IsNearEnemy = false;
            
            var screenPosOfThis = MainCamera.WorldToScreenPoint(worldPosition);
            
            var hitInfo = SphereCasterFromScreen.GetRayCastHit(screenPosOfThis, Color.green);
            var hitEnemy = hitInfo?.collider.GetComponent<Enemy.IEnemy>();
            if (hitEnemy != null && _hitEnemies.Contains(hitEnemy))
                return;
            if (hitEnemy == null) return;
            if (!IsNotifyDamage) return;
            hitEnemy.HitPoint.ReceiveDamage(Damage);
            _hitEnemies.Add(hitEnemy);
        }
    }
}