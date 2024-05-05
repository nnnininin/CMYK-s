using Player.Skill;
using Scene;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Player.ScriptableObject
{
    [CreateAssetMenu(fileName = "AutoSkillSO", menuName = "ScriptableObjects/AutoSkillSO")]
    public class AutoSkillSO : CardSO
    {
        [SerializeField]
        private GameObject damageFieldPrefab;
        public GameObject DamageFieldPrefab => damageFieldPrefab;
        
        [SerializeField]
        private int attack;
        public int Attack => attack;
        
        [SerializeField]
        private int initialScale;
        public int InitialScale => initialScale;
        
        [SerializeField]
        private int coolTime;
        public int CoolTime => coolTime;
        
        [SerializeField]
        private float lifeTime;
        public float LifeTime => lifeTime;
        
        [SerializeField]
        private int numberOfSkill;
        public int NumberOfSkill => numberOfSkill;
        
        //自動発動スキルの種類に対応したfieldActivatorをインスペクタ上で選択
        [Header("AutoSkillType")]
        [SerializeReference][SubclassSelector]
        private IFieldActivator fieldActivator;
        // ReSharper disable once ConvertToAutoProperty
        public IFieldActivator CreateFieldActivatorInstance()
        {
            if (fieldActivator == null)
            {
                Debug.LogError("AutoSkillActivator is null");
                return null;
            }
            
            var autoSkillActivatorType = fieldActivator.GetType();
            var autoSkillActivatorInstance = System.Activator.CreateInstance(autoSkillActivatorType);
            
            if (autoSkillActivatorInstance is IFieldActivator newAutoSkillActivator) return newAutoSkillActivator;
            Debug.LogError("AutoSkillActivator is not IAutoSkillActivator");
            return null;
        }
        
        public override void ApplyToGameData(
            TemporaryGameDataSo temporaryGameData
        )
        {
            temporaryGameData.SetAutoSkillSO(this);
        }
    }
}
