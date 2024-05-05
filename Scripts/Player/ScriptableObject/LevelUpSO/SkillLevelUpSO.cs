using Scene;
using UnityEngine;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "SkillLevelUpSO", menuName = "ScriptableObjects/SkillLevelUpSO")]
    public class SkillLevelUpSO : CardSO
    {
        //上昇するレベル
        [SerializeField]
        private int attack;
        public int Attack => attack;
        
        [SerializeField]
        private int maxScale;
        public int MaxScale => maxScale;
        
        [SerializeField]
        private int coolTime;
        public int CoolTime => coolTime;
        
        //レベルアップする際に現在のスキルが以下のスキルに変更される
        [SerializeField]
        private SkillSO skillSO;
        public SkillSO SkillSO => skillSO;
        
        public override void ApplyToGameData(
            TemporaryGameDataSo temporaryGameData
        )
        {
            temporaryGameData.SetSkillSO(SkillSO);
            temporaryGameData.IncreaseSkillLevel(
                Attack, 
                MaxScale,
                CoolTime
                );
        }
    }
}