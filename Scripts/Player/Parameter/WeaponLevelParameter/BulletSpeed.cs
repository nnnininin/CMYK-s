using UniRx;
using UnityEngine;

namespace Player.Parameter
{
    public class BulletSpeed
    {
        public IReadOnlyReactiveProperty<float> BulletSpeedValue { get; }

        public BulletSpeed(WeaponParameter weaponParameter)
        {
            Debug.Log("BulletSpeedLevelUp");
            BulletSpeedValue = weaponParameter.BulletSpeed
                .Select(CalculateFinalBulletSpeed)
                .ToReactiveProperty();
        }
        private static float CalculateFinalBulletSpeed(float baseBulletSpeed)
        {
            return baseBulletSpeed;
        }
    }
}