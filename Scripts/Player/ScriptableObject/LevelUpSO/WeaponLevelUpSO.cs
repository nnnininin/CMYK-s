using Scene;
using UnityEngine;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "WeaponLevelUpSO", menuName = "ScriptableObjects/WeaponLevelUpSO")]
    public class WeaponLevelUpSO : CardSO
    {
        //上昇するレベル
        [SerializeField]
        private int attack;
        public int Attack => attack;
        
        [SerializeField]
        private int bulletSpeed;
        public int BulletSpeed => bulletSpeed;
        
        [SerializeField]
        private int magazineSize;
        public int MagazineSize => magazineSize;
        
        //レベルアップする際に現在の武器が以下の武器に変更される
        [SerializeField]
        private WeaponSO weaponSO;
        public WeaponSO WeaponSO => weaponSO;
        
        public override void ApplyToGameData(
            TemporaryGameDataSo temporaryGameData
        )
        {
            temporaryGameData.SetWeaponSO(WeaponSO);
            temporaryGameData.IncreaseWeaponLevels(
                Attack, 
                BulletSpeed,
                MagazineSize
                );
        }
    }
}