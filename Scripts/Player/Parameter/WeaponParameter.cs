using Player.ScriptableObject;
using UniRx;
using UnityEngine;

namespace Player.Parameter
{
    public class WeaponParameter
    {
        private readonly ReactiveProperty<string> _weaponName = new();
        public IReadOnlyReactiveProperty<string> WeaponName => _weaponName.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<string> _weaponDescription = new();
        public IReadOnlyReactiveProperty<string> WeaponDescription => _weaponDescription.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<int> _wayNumber = new();
        public IReadOnlyReactiveProperty<int> WayNumber => _wayNumber.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<int> _accuracy = new();
        public IReadOnlyReactiveProperty<int> AccuracyValue => _accuracy.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<int> _fireRate = new();
        public IReadOnlyReactiveProperty<int> FireRateValue => _fireRate.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<int> _reloadTime = new();
        public IReadOnlyReactiveProperty<int> ReloadTimeValue => _reloadTime.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<GameObject> _weaponPrefab = new();
        public IReadOnlyReactiveProperty<GameObject> WeaponPrefab => _weaponPrefab.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<GameObject> _bulletPrefab = new();
        public IReadOnlyReactiveProperty<GameObject> BulletPrefab => _bulletPrefab.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<GameObject> _shotEffectPrefab = new();
        public IReadOnlyReactiveProperty<GameObject> ShotEffectPrefab => _shotEffectPrefab.ToReadOnlyReactiveProperty();

        // レベル
        private readonly ReactiveProperty<int> _attackPowerLevel = new();
        public IReadOnlyReactiveProperty<int> AttackPowerLevel => _attackPowerLevel.ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _bulletSpeedLevel = new();
        public IReadOnlyReactiveProperty<int> BulletSpeedLevel => _bulletSpeedLevel.ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _magazineLevel = new();
        public IReadOnlyReactiveProperty<int> MagazineLevel => _magazineLevel.ToReadOnlyReactiveProperty();
     
        // レベルに対応して計算されるステータス
        private readonly ReactiveProperty<int> _attackPower = new();
        public IReadOnlyReactiveProperty<int> AttackPower => _attackPower
            .CombineLatest(AttackPowerLevel, CalculateAttackPower)
            .ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<float> _bulletSpeed = new();
        public IReadOnlyReactiveProperty<float> BulletSpeed => _bulletSpeed
            .CombineLatest(BulletSpeedLevel, CalculateBulletSpeed)
            .ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _magazine = new();
        public IReadOnlyReactiveProperty<int> Magazine => _magazine
            .CombineLatest(MagazineLevel, CalculateMagazine)
            .ToReadOnlyReactiveProperty();
       public static int MaxLevel => 40;

       public WeaponParameter(WeaponSO weaponSO)
        {
            const int initialLevel = 1;
            SetLevel(initialLevel, initialLevel, initialLevel);
            SetSO(weaponSO);
        }
        
        public void SetSO(WeaponSO weaponSO)
        {
            _weaponName.Value = weaponSO.DisplayName;
            _weaponDescription.Value = weaponSO.Description;
            _wayNumber.Value = weaponSO.WayNumber;
            _accuracy.Value = weaponSO.Accuracy;
            _fireRate.Value = weaponSO.FireRate;
            _reloadTime.Value = weaponSO.ReloadTime;
            _weaponPrefab.Value = weaponSO.WeaponPrefab;
            _bulletPrefab.Value = weaponSO.BulletPrefab;
            _shotEffectPrefab.Value = weaponSO.ShotEffectPrefab;
            
            _attackPower.Value = weaponSO.AttackPower;
            _bulletSpeed.Value = weaponSO.BulletSpeed;
            _magazine.Value = weaponSO.Magazine;
        }

        public void SetLevel(int attack, int bulletSpeed, int magazine)
        {
            _attackPowerLevel.Value = attack;
            _bulletSpeedLevel.Value = bulletSpeed;
            _magazineLevel.Value = magazine;
        }
        
        // ステータス計算式
        private static int CalculateAttackPower(int baseAttackPower, int level)
        {
            var def = level * baseAttackPower* 0.2f;
            Debug.Log($"weaponATKDef: {def}");
            def = Mathf.Max(1, def);
            return baseAttackPower + (int)def;
        }
        private static float CalculateBulletSpeed(float baseBulletSpeed, int level)
        {
            var def = (float)level / 30 * baseBulletSpeed;
            Debug.Log($"weaponBSDef: {def}");
            return baseBulletSpeed + def;
        }
        private static int CalculateMagazine(int baseMagazine, int level)
        {
            var def = level * baseMagazine * 0.2f;
            Debug.Log($"weaponMGDef: {def}");
            def = Mathf.Max(1, def);
            return baseMagazine + (int)def;
        }
    }
}