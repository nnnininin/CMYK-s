using Player.ScriptableObject;
using Player.Skill;
using UniRx;
using UnityEngine;

namespace Player.Parameter
{
    public class AutoSkillParameter
    {
        private readonly ReactiveProperty<string> _autoSkillName = new();
        public IReadOnlyReactiveProperty<string> AutoSkillName => _autoSkillName.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<string> _autoSkillDescription = new();
        public IReadOnlyReactiveProperty<string> AutoSkillDescription => _autoSkillDescription.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<GameObject> _damageFieldPrefab = new();
        public IReadOnlyReactiveProperty<GameObject> DamageFieldPrefab => _damageFieldPrefab.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<float> _lifeTime = new();
        public IReadOnlyReactiveProperty<float> LifeTime => _lifeTime.ToReadOnlyReactiveProperty();
        
        //0でスキル使用可能、CoolTimeが最大値
        private readonly ReactiveProperty<float> _coolTimeCount = new();
        public IReadOnlyReactiveProperty<float> CoolTimeCount => _coolTimeCount.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<int> _numberOfSkill = new();
        public IReadOnlyReactiveProperty<int> NumberOfSkill => _numberOfSkill.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<IFieldActivator> _fieldActivator = new();
        public IReadOnlyReactiveProperty<IFieldActivator> FieldActivator => _fieldActivator.ToReadOnlyReactiveProperty();
        
        //レベル
        private readonly ReactiveProperty<int> _attackLevel = new();
        public IReadOnlyReactiveProperty<int> AttackLevel => _attackLevel.ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _maxScaleLevel = new();
        public IReadOnlyReactiveProperty<int> MaxScaleLevel => _maxScaleLevel.ToReadOnlyReactiveProperty();
        private readonly ReactiveProperty<int> _coolTimeLevel = new();
        public IReadOnlyReactiveProperty<int> CoolTimeLevel => _coolTimeLevel.ToReadOnlyReactiveProperty();

        //レベルに対応して計算されるステータス
        private readonly ReactiveProperty<int> _attack = new();
        public IReadOnlyReactiveProperty<int> Attack => _attack
            .CombineLatest(AttackLevel, CalculateAttack)
            .ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<int> _maxScale = new();
        public IReadOnlyReactiveProperty<float> MaxScale => _maxScale
            .CombineLatest(MaxScaleLevel, CalculateMaxScale)
            .ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<int> _coolTime = new();
        public IReadOnlyReactiveProperty<float> CoolTime => _coolTime
            .CombineLatest(CoolTimeLevel, CalculateCoolTime)
            .ToReadOnlyReactiveProperty();
        
        public static int MaxLevel => 40;

        //レベルとステータスを用いた実数値の計算
        private static int CalculateAttack(int attack, int attackLevel)
        {
            var def = attackLevel* attack * 0.2f;
            Debug.Log($"AutoSkillATKDef: {def}");
            def = Mathf.Max(1, def);
            return attack + (int)def;
        }

        private static float CalculateMaxScale(int scale, int scaleLevel)
        {
            var def = (float)scaleLevel * scale * 0.1f;
            Debug.Log($"AutoSkillScaleDef: {def}");
            return scale + def;
        }

        private static float CalculateCoolTime(int coolTime, int coolTimeLevel)
        {
            const float multiplier = 50f;
            var def = (float)coolTimeLevel * coolTime * 0.1f;
            Debug.Log($"AutoSkillCTDef: {def}");
            return 1/(coolTime + def) * multiplier;
        }
        
        public AutoSkillParameter(AutoSkillSO autoSkillSo)
        {
            const int initialLevel = 1;
            SetLevel(initialLevel, initialLevel, initialLevel);
            SetSkillSO(autoSkillSo);
        }

        public void SetSkillSO(AutoSkillSO autoSkillSo)
        {
            _autoSkillName.Value = autoSkillSo.DisplayName;
            _autoSkillDescription.Value = autoSkillSo.Description;
            _damageFieldPrefab.Value = autoSkillSo.DamageFieldPrefab;
            
            _lifeTime.Value = autoSkillSo.LifeTime;
            
            _attack.Value = autoSkillSo.Attack;
            _maxScale.Value = autoSkillSo.InitialScale;
            _coolTime.Value = autoSkillSo.CoolTime;
            
            _coolTimeCount.Value = CoolTime.Value/3f;    //開始即発動してはダメなので、最大値/3をセット
            _numberOfSkill.Value = autoSkillSo.NumberOfSkill;
            
            _fieldActivator.Value = autoSkillSo.CreateFieldActivatorInstance();
            Debug.Log($"AutoSkillActivator: {_fieldActivator.Value}");
        }
        
        public void SetLevel(int attackLevel, int maxScaleLevel, int coolTimeLevel)
        {
            _attackLevel.Value = attackLevel;
            _maxScaleLevel.Value = maxScaleLevel;
            _coolTimeLevel.Value = coolTimeLevel;
        }
        
        public void CountDownCoolTime(float deltaTime)
        {
            _coolTimeCount.Value -= deltaTime;
            if (_coolTimeCount.Value < 0)
            {
                _coolTimeCount.Value = 0;
            }
        }
        
        public void ResetCoolTimeCount()
        {
            //lifetime分を加えた値にリセット
            _coolTimeCount.Value = CoolTime.Value+LifeTime.Value;
        }
    }
}