using Player.ScriptableObject;
using UniRx;
using UnityEngine;

namespace Player.Parameter
{
    public class SkillParameter
    {
        private readonly ReactiveProperty<string> _skillName = new();
        public IReadOnlyReactiveProperty<string> SkillName => _skillName.ToReadOnlyReactiveProperty();
       
        private readonly ReactiveProperty<string> _skillDescription = new();
        public IReadOnlyReactiveProperty<string> SkillDescription => _skillDescription.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<GameObject> _skillPrefab = new();
        public IReadOnlyReactiveProperty<GameObject> SkillPrefab => _skillPrefab.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<GameObject> _damageZonePrefab = new();
        public IReadOnlyReactiveProperty<GameObject> DamageZonePrefab => _damageZonePrefab.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<float> _lifeTime = new();
        public IReadOnlyReactiveProperty<float> LifeTime => _lifeTime.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<float> _coolTimeCount = new();
        public IReadOnlyReactiveProperty<float> CoolTimeCount => _coolTimeCount.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<SkillEffectType> _skillEffectType = new();
        public IReadOnlyReactiveProperty<SkillEffectType> SkillEffectType => _skillEffectType.ToReadOnlyReactiveProperty();

        //レベル
        private readonly ReactiveProperty<int> _attackLevel = new();
        public IReadOnlyReactiveProperty<int> AttackLevel => _attackLevel.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<int> _maxScaleLevel = new();
        public IReadOnlyReactiveProperty<int> MaxScaleLevel => _maxScaleLevel.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<int> _coolTimeLevel = new();
        public IReadOnlyReactiveProperty<int> CoolTimeLevel => _coolTimeLevel.ToReadOnlyReactiveProperty();
        
        // レベルに対応して計算されるステータス
        private readonly ReactiveProperty<int> _attack = new();
        public IReadOnlyReactiveProperty<int> Attack => _attack
            .CombineLatest(_attackLevel, CalculateAttack)
            .ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _maxScale = new();
        public IReadOnlyReactiveProperty<float> MaxScale => _maxScale
            .CombineLatest(_maxScaleLevel, CalculateMaxScale)
            .ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<float> _coolTime = new();
        public IReadOnlyReactiveProperty<float> CoolTime => _coolTime
            .CombineLatest(_coolTimeLevel, CalculateCoolTime)
            .ToReadOnlyReactiveProperty();
        private int CalculateAttack(int damage, int damageLevel)
        {
            var def = damageLevel * damage * 0.2f;
            Debug.Log("SkillATKDef: " + def);
            def = Mathf.Max(1, def);
            return damage + (int)def;
        }

        private float CalculateCoolTime(float baseCoolTime, int coolTimeLevel)
        {
            const float multiplier = 50f;
            var def = coolTimeLevel * baseCoolTime * 0.1f;
            Debug.Log("SkillCTDef: " + def);
            return 1/(baseCoolTime + def) * multiplier;
        }

        private float CalculateMaxScale(int scale, int scaleLevel)
        {
            var def = scaleLevel * scale * 0.1f;
            Debug.Log("SkillScaleDef: " + def);
            return scale + def;
        }
        public static int MaxLevel => 40;
        public SkillParameter(SkillSO skillSo)
        {
            const int initialLevel = 1;
            SetLevel(initialLevel, initialLevel, initialLevel);
            SetSO(skillSo);
        }

        public void SetSO(SkillSO skillSo)
        {
            _skillName.Value = skillSo.DisplayName;
            _skillDescription.Value = skillSo.Description;
            _skillPrefab.Value = skillSo.SkillPrefab;
            _damageZonePrefab.Value = skillSo.DamageZonePrefab;
            _skillEffectType.Value = skillSo.SkillEffectType;
            _lifeTime.Value = skillSo.LifeTime;
            
            _attack.Value = skillSo.Damage;
            _maxScale.Value = skillSo.MaxScale;
            _coolTime.Value = skillSo.CoolTime;
            _coolTimeCount.Value = 0;
        }

        public void SetLevel(int attackLevel, int maxScaleLevel, int coolTimeLevel)
        {
            _attackLevel.Value = attackLevel;
            _maxScaleLevel.Value = maxScaleLevel;
            _coolTimeLevel.Value = coolTimeLevel; 
        }
        
        public bool CountDownCoolTime(float deltaTime)
        {
            _coolTimeCount.Value -= deltaTime;
            if (!(_coolTimeCount.Value < 0)) return false;
            _coolTimeCount.Value = 0;
            return true;
        }

        // CoolTimeCountをリセット
        public void ResetCoolTimeCount()
        {
            _coolTimeCount.Value = CoolTime.Value + 0.3f;
        }
    }
}