using Scene;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "AutoSkillLevelUpSO", menuName = "ScriptableObjects/AutoSkillLevelUpSO")]
    public class AutoSkillLevelUpSO : CardSO
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
        
        //レベルアップする際に現在の自動スキルが以下の自動スキルに変更される
        [SerializeField]
        private AutoSkillSO autoSkillSO;
        public AutoSkillSO AutoSkillSO => autoSkillSO;
        
        public override void ApplyToGameData(
            TemporaryGameDataSo temporaryGameData
        )
        {
            temporaryGameData.SetAutoSkillSO(AutoSkillSO);
            temporaryGameData.IncreaseAutoSkillLevel(
                Attack, 
                MaxScale,
                CoolTime
                );
        }
    }
}