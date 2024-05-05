using System;
using Scene;
using UnityEngine;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/WeaponSO")]
    public class WeaponSO : CardSO
    {
        [SerializeField]
        private OddWayNumber wayNumberOdds;
        public int WayNumber
        {
            get
            {
                return wayNumberOdds switch
                {
                    OddWayNumber._1 => 1,
                    OddWayNumber._3 => 3,
                    OddWayNumber._5 => 5,
                    OddWayNumber._7 => 7,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        //倍率ステータス
        [SerializeField]
        private int attackPower;
        public int AttackPower => attackPower;
        [SerializeField]
        private int magazine;
        public int Magazine => magazine;
        
        [SerializeField]
        private int bulletSpeed;
        public int BulletSpeed => bulletSpeed;
        
        [SerializeField]
        private int fireRate;
        public int FireRate => fireRate;
        [SerializeField]
        private int reloadTime;
        public int ReloadTime => reloadTime;
        [SerializeField]
        private int accuracy;
        public int Accuracy => accuracy;
        [SerializeField]
        private GameObject weaponPrefab;
        public GameObject WeaponPrefab => weaponPrefab;

        [SerializeField]
        private GameObject bulletPrefab;
        public GameObject BulletPrefab => bulletPrefab;

        [SerializeField]
        private GameObject shotEffectPrefab;
        public GameObject ShotEffectPrefab => shotEffectPrefab;
        
        public override void ApplyToGameData(TemporaryGameDataSo temporaryGameData)
        {
            temporaryGameData.SetWeaponSO(this);
        }
    }
    public enum OddWayNumber
    {
        _1,
        _3,
        _5,
        _7,
    }
}
