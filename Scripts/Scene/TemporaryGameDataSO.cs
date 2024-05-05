using System;
using System.Collections.Generic;
using Player.ScriptableObject;
using UnityEngine;
using UniRx;

namespace Scene
{
    public enum ColorType
    {
        Cyan,
        Magenta,
        Yellow
    }

    // シーン間で引き継がれるデータを保持するScriptableObject
    [CreateAssetMenu(fileName = "TemporaryGameData", menuName = "SceneManagement/TemporaryGameData")]
    public class TemporaryGameDataSo : ScriptableObject
    {
        private readonly ReactiveProperty<GameObject> _playerPrefab = new();
        public ReadOnlyReactiveProperty<GameObject> PlayerPrefab => _playerPrefab.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<CharaSO> _charaSO = new();
        public ReadOnlyReactiveProperty<CharaSO> CharaSO => _charaSO.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<WeaponSO> _weaponSO = new();
        public ReadOnlyReactiveProperty<WeaponSO> WeaponSO => _weaponSO.ToReadOnlyReactiveProperty();

        private readonly ReactiveProperty<SkillSO> _skillSO = new();
        public ReadOnlyReactiveProperty<SkillSO> SkillSO => _skillSO.ToReadOnlyReactiveProperty();
        
        private readonly ReactiveProperty<AutoSkillSO> _autoSkillSO = new();
        public ReadOnlyReactiveProperty<AutoSkillSO> AutoSkillSO => _autoSkillSO.ToReadOnlyReactiveProperty();
        
        // 武器の持つ各ステータスのレベル配列
        private readonly ReactiveProperty<int[]> _weaponLevels = new(new int[3]);
        public ReadOnlyReactiveProperty<int[]> WeaponLevel => _weaponLevels.ToReadOnlyReactiveProperty();
        
        // スキルの持つ各ステータスのレベル配列
        private readonly ReactiveProperty<int[]> _skillLevels = new(new int[3]);
        public ReadOnlyReactiveProperty<int[]> SkillLevel => _skillLevels.ToReadOnlyReactiveProperty();
        
        // 自動スキルの持つ各ステータスのレベル配列
        private readonly ReactiveProperty<int[]> _autoSkillLevels = new(new int[3]);
        public ReadOnlyReactiveProperty<int[]> AutoSkillLevel => _autoSkillLevels.ToReadOnlyReactiveProperty();
        
        //色ごとの経験値
        private readonly Dictionary<ColorType, IntReactiveProperty> 
            _colorExp = new()
        {
            { ColorType.Cyan, new IntReactiveProperty(0) },
            { ColorType.Magenta, new IntReactiveProperty(0) },
            { ColorType.Yellow, new IntReactiveProperty(0) }
        };
        public IReadOnlyDictionary<ColorType, IntReactiveProperty> ColorExp => _colorExp;

        private readonly Dictionary<ColorType, IntReactiveProperty> 
            _colorLevels = new()
        {
            { ColorType.Cyan, new IntReactiveProperty(1) },
            { ColorType.Magenta, new IntReactiveProperty(1) },
            { ColorType.Yellow, new IntReactiveProperty(1) }
        };
        public IReadOnlyDictionary<ColorType, IntReactiveProperty> ColorLevels => _colorLevels;
        
        private readonly Dictionary<ColorType, int> 
            _requiredExpToColorLevelUp = new()
        {
            { ColorType.Cyan, LevelUpExp },
            { ColorType.Magenta, LevelUpExp },
            { ColorType.Yellow, LevelUpExp }
        };
        public const int LevelUpExp = 20; // 各LevelTypeのレベルアップに必要なEXPの共通値
        
        //色レベルアップ時に呼び出すSubject
        private Subject<Unit> _cyanLevelUp = new();
        public IObservable<Unit> OnCyanLevelUp => _cyanLevelUp;
        
        private Subject<Unit> _magentaLevelUp = new();
        public IObservable<Unit> OnMagentaLevelUp => _magentaLevelUp;
        
        private Subject<Unit> _yellowLevelUp = new();
        public IObservable<Unit> OnYellowLevelUp => _yellowLevelUp;
        
        public void Initialize(GameObject playerPrefab, 
            CharaSO charaSO,
            WeaponSO weaponSO,
            SkillSO skillSO, 
            AutoSkillSO autoSkillSO
            )
        {
            SetPlayerPrefab(playerPrefab);
            SetCharaSO(charaSO);
            InitializeWeaponSO(weaponSO);
            InitializeSkillSO(skillSO);
            InitializeAutoSkillSO(autoSkillSO);
            InitializeColorLevels();
            InitializeColorLevelExp();
        }
        
        //色ごとのレベルの初期化
        public void InitializeColorLevels()
        {
            const int initialLevel = 1;
            _colorLevels[ColorType.Cyan].Value = initialLevel;
            _colorLevels[ColorType.Magenta].Value = initialLevel;
            _colorLevels[ColorType.Yellow].Value = initialLevel;
        }
        
        //色ごとの経験値の初期化
        public void InitializeColorLevelExp()
        {
            const int initialExp = 0;
            _colorExp[ColorType.Cyan].Value = initialExp;
            _colorExp[ColorType.Magenta].Value = initialExp;
            _colorExp[ColorType.Yellow].Value = initialExp;
        }
        
        // Setter Methods
        public void SetPlayerPrefab(GameObject playerPrefab) => _playerPrefab.Value = playerPrefab;
        public void SetCharaSO(CharaSO charaSO) => _charaSO.Value = charaSO;
        
        // SOの初期化メソッド
        public void InitializeWeaponSO(WeaponSO weaponSO)
        {
            const int initialLevel = 1;
            SetWeaponSO(weaponSO);
    
            // 新しい配列を作成し、レベルの初期値を設定
            int[] initialLevels = { initialLevel, initialLevel, initialLevel };
            _weaponLevels.Value = initialLevels;
        }

        public void InitializeSkillSO(SkillSO skillSO)
        {
            const int initialLevel = 1;
            SetSkillSO(skillSO);
    
            // 新しい配列を作成し、レベルの初期値を設定
            int[] initialLevels = { initialLevel, initialLevel, initialLevel };
            _skillLevels.Value = initialLevels;
        }
        public void InitializeAutoSkillSO(AutoSkillSO autoSkillSO)
        {
            const int initialLevel = 1;
            SetAutoSkillSO(autoSkillSO);
    
            // 新しい配列を作成し、レベルの初期値を設定
            int[] initialLevels = { initialLevel, initialLevel, initialLevel };
            _autoSkillLevels.Value = initialLevels;
        }
        
        // Weapon, Skill, AutoSkillの変更メソッド
        public void SetWeaponSO(WeaponSO weaponSO)
        {
            _weaponSO.Value = weaponSO;
        }
        public void SetSkillSO(SkillSO skillSO)
        {
            _skillSO.Value = skillSO;
        }
        public void SetAutoSkillSO(AutoSkillSO autoSkillSO)
        {
            autoSkillSO.CreateFieldActivatorInstance();
            _autoSkillSO.Value = autoSkillSO;
        }
        
        // 武器・スキル・自動スキルのレベルアップのメソッド
        public void IncreaseWeaponLevels(int attack, int bulletSpeed, int magazine)
        {
            // 既存の配列をクローンしてから更新
            var newLevels = (int[])_weaponLevels.Value.Clone();
            newLevels[0] += attack;
            newLevels[1] += bulletSpeed;
            newLevels[2] += magazine;

            // 更新された配列をReactivePropertyに設定
            _weaponLevels.Value = newLevels;
        }

        public void IncreaseSkillLevel(int attack, int maxScale, int coolTime)
        {
            // 既存の配列をクローンしてから更新
            var newLevels = (int[])_skillLevels.Value.Clone();
            newLevels[0] += attack;
            newLevels[1] += maxScale;
            newLevels[2] += coolTime;
            
            // 更新された配列をReactivePropertyに設定
            _skillLevels.Value = newLevels;
        }

        public void IncreaseAutoSkillLevel(int attack, int maxScale, int coolTime)
        {
            // 既存の配列をクローンしてから更新
            var newLevels = (int[])_autoSkillLevels.Value.Clone();
            newLevels[0] += attack;
            newLevels[1] += maxScale;
            newLevels[2] += coolTime;
            
            // 更新された配列をReactivePropertyに設定
            _autoSkillLevels.Value = newLevels;
        }
        
        // 経験値を追加するメソッド
        public void AddColorExp(ColorType colorType, int exp)
        {
            var currentExp = _colorExp[colorType].Value;
            var levelUpExp = _requiredExpToColorLevelUp[colorType];
            var newExp = currentExp + exp;

            // レベルアップ処理
            while (newExp >= levelUpExp)
            {
                //繰り返すたびにレベルアップ
                newExp -= levelUpExp;
                LevelUpColor(colorType);
            }
            _colorExp[colorType].Value = newExp;
        }

        // colorTypeに対応したレベルアップ処理
        private void LevelUpColor(ColorType colorType)
        {
            _colorLevels[colorType].Value += 1; // レベルをインクリメント

            switch (colorType)
            {
                case ColorType.Cyan:
                    _cyanLevelUp.OnNext(Unit.Default);
                    break;
                case ColorType.Magenta:
                    _magentaLevelUp.OnNext(Unit.Default);
                    break;
                case ColorType.Yellow:
                    _yellowLevelUp.OnNext(Unit.Default);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null);
            }
        }
    }
}