using Scene;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObjects/SkillSO")]
    public class SkillSO : CardSO
    {
        [SerializeField]
        private GameObject skillPrefab;
        public GameObject SkillPrefab => skillPrefab;
        
        [SerializeField]
        private GameObject damageZonePrefab;
        public GameObject DamageZonePrefab => damageZonePrefab;
        
        [SerializeField]
        private int damage;
        public int Damage => damage;
        
        [SerializeField]
        private int maxScale;
        public int MaxScale => maxScale;
        
        [SerializeField]
        private float coolTime;
        public float CoolTime => coolTime;
        
        [SerializeField]
        private int lifeTime;
        public int LifeTime => lifeTime;
        
        
        [SerializeField]
        private SkillEffectType skillEffectType;
        public SkillEffectType SkillEffectType => skillEffectType;
        
        
        public override void ApplyToGameData(
            TemporaryGameDataSo temporaryGameData
        )
        {
            temporaryGameData.SetSkillSO(this);
        }
    }
    
    public enum SkillEffectType
    {
        Normal,
        Deceleration,
        DamageBoost
    }
}
