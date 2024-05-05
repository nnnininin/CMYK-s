using System;
using Manager.DIContainer;
using Player;
using Player.Parameter;
using Player.Util;
using Scene;
using UniRx;
using UnityEngine;
using Util.Input;
using Zenject;

namespace UI.ActionScene
{
    public class PlayerPresenter : MonoBehaviour
    {
        [Inject] private GlobalEnemyEventManager _globalEnemyEventManager;
        [Inject] private GlobalPlayerEventManager _globalPlayerEventManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private PauseInput pauseInput;
        [SerializeField] private CardSelector[] card;
        [SerializeField] private TemporaryGameDataSo temporaryGameData;
        [SerializeField] private SoundManager soundManager;

        private CanvasType _currentCanvasType;

        private void Awake()
        {
            _currentCanvasType = CanvasType.InGame;
            //プレイヤーが生成された時にUIを更新
            playerSpawner.OnPlayerSpawned.Subscribe(UpdateParameterUI).AddTo(this);
            //一時停止関連の処理
            pauseInput.OnPause.Subscribe(_ => PauseInGame()).AddTo(this);
            pauseInput.OnResume.Subscribe(_ => ResumeInGame()).AddTo(this);
            //各色のレベルアップ処理時に対応したカード選択画面を表示
            temporaryGameData.OnCyanLevelUp.Subscribe(_ => SelectWeaponLevelUp()).AddTo(this);
            temporaryGameData.OnMagentaLevelUp.Subscribe(_ => SelectSkillLevelUp()).AddTo(this);
            temporaryGameData.OnYellowLevelUp.Subscribe(_ => SelectAutoSkillLevelUp()).AddTo(this);
            //カード選択画面でカードが選択された時に画面を戻す
            foreach (var c in card)
            {
                c.OnCardClicked.Subscribe(_ => EndSelectCard()).AddTo(this);
            }

            //敵死亡時の処理
            _globalEnemyEventManager.OnGlobalDeath.Subscribe(_ => DeathSound()).AddTo(this);
            //ゲームクリア時の処理
            _globalEnemyEventManager.OnGlobalBossDeath.Subscribe(_ => ClearGame()).AddTo(this);
            //ダメージを受けた時の処理
            _globalPlayerEventManager.OnGlobalDamageReceived.Subscribe(_ => PlayerDamageSound()).AddTo(this);
        }

        private void UpdateParameterUI(GameObject playerInstance)
        {
            var player = playerInstance.GetComponentInChildren<IPlayer>();
            if (player == null) return;

            //各パラメータのUIを更新するRxを登録
            SetBulletValue(player);
            SetHpValue(player);
            SetLevels(player);
            SetWeapon(player);
            SetSkill(player);
            SetAutoSkill(player);
            SetExpValue(temporaryGameData);
            SetLevelSliderMaxValue();
            SetSliderValue(player);
        }

        private void SetExpValue(TemporaryGameDataSo temporaryGameDataSO)
        {
            const int maxExp = TemporaryGameDataSo.LevelUpExp;
            foreach (var levelExpEntry in temporaryGameDataSO.ColorExp)
            {
                levelExpEntry.Value
                    .Subscribe(expValue =>
                    {
                        switch (levelExpEntry.Key)
                        {
                            case ColorType.Cyan:
                                uiManager.UpdateWeaponExpSlider(expValue, maxExp);
                                break;
                            case ColorType.Magenta:
                                uiManager.UpdateSkillExpSlider(expValue, maxExp);
                                break;
                            case ColorType.Yellow:
                                uiManager.UpdateAutoSkillExpSlider(expValue, maxExp);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    })
                    .AddTo(this);
            }
        }

        private void SetSliderValue(IPlayer player)
        {
            player.HitPoint.MaxHitPoint
                .Subscribe(maxHp => uiManager.UpdateHpSlider(maxHp, player.HitPoint.CurrentHitPoint.Value))
                .AddTo(this);

            player.HitPoint.CurrentHitPoint
                .Subscribe(currentHp => uiManager.UpdateHpSlider(player.HitPoint.MaxHitPoint.Value, currentHp))
                .AddTo(this);

            player.Magazine.MaxBullet
                .Subscribe(maxBullet => uiManager.UpdateWeaponSlider(maxBullet, player.Magazine.CurrentBullet.Value))
                .AddTo(this);

            player.Magazine.CurrentBullet
                .Subscribe(
                    currentBullet => uiManager.UpdateWeaponSlider(player.Magazine.MaxBullet.Value, currentBullet))
                .AddTo(this);

            player.SkillParameter.CoolTimeCount
                .Where(coolTimeCount =>
                    coolTimeCount <= player.SkillParameter.CoolTime.Value) // CoolTimeCountがCoolTime.value以下の時のみ
                .Subscribe(coolTimeCount => uiManager.UpdateSkillSlider(
                        player.SkillParameter.CoolTime.Value,
                        player.SkillParameter.CoolTime.Value - coolTimeCount
                    )
                ) // CoolTime.value - CoolTimeCountを渡す
                .AddTo(this);

            player.SkillParameter.CoolTime
                .Subscribe(coolTime => uiManager.UpdateSkillSlider(
                        coolTime,
                        player.SkillParameter.CoolTimeCount.Value
                    )
                )
                .AddTo(this);

            player.AutoSkillParameter.CoolTimeCount
                .Where(coolTimeCount =>
                    coolTimeCount <= player.AutoSkillParameter.CoolTime.Value) // CoolTimeCountがCoolTime.value以下の時のみ
                .Subscribe(coolTimeCount => uiManager.UpdateAutoSkillSlider(
                        player.AutoSkillParameter.CoolTime.Value,
                        player.AutoSkillParameter.CoolTime.Value - coolTimeCount
                    )
                ) // CoolTime.value - CoolTimeCountを渡す
                .AddTo(this);

            player.AutoSkillParameter.CoolTime
                .Subscribe(coolTime => uiManager.UpdateAutoSkillSlider(
                        coolTime,
                        player.AutoSkillParameter.CoolTimeCount.Value
                    )
                )
                .AddTo(this);

        }

        private void SetLevelSliderMaxValue()
        {
            uiManager.SetLevelSliderMax(
                WeaponParameter.MaxLevel,
                SkillParameter.MaxLevel,
                AutoSkillParameter.MaxLevel);
        }

        private void SetBulletValue(IPlayer player)
        {
            player.Magazine.MaxBullet
                .Subscribe(maxBullet => uiManager.UpdateBulletText(maxBullet, player.Magazine.CurrentBullet.Value))
                .AddTo(this);

            player.Magazine.CurrentBullet
                .Subscribe(currentBullet => uiManager.UpdateBulletText(player.Magazine.MaxBullet.Value, currentBullet))
                .AddTo(this);
        }

        private void SetHpValue(IPlayer player)
        {
            player.HitPoint.MaxHitPoint
                .Subscribe(maxHp => uiManager.UpdateHpText(maxHp, player.HitPoint.CurrentHitPoint.Value))
                .AddTo(this);

            player.HitPoint.CurrentHitPoint
                .Subscribe(currentHp => uiManager.UpdateHpText(player.HitPoint.MaxHitPoint.Value, currentHp))
                .AddTo(this);
        }

        private void SetLevels(IPlayer player)
        {
            SetWeaponLevels(player);
            SetSkillLevels(player);
            SetAutoSkillLevels(player);
        }

        private void SetWeaponLevels(IPlayer player)
        {
            player.WeaponParameter.AttackPowerLevel.CombineLatest(
                player.WeaponParameter.MagazineLevel,
                player.WeaponParameter.BulletSpeedLevel,
                (attackLevel, magazineLevel, bulletSpeedLevel) =>
                    new { attackLevel, magazineLevel, bulletSpeedLevel }
            ).Subscribe(x =>
                uiManager.UpdateWeaponLevelsText(
                    x.attackLevel,
                    x.magazineLevel,
                    x.bulletSpeedLevel
                )
            ).AddTo(this);
        }

        private void SetSkillLevels(IPlayer player)
        {
            player.SkillParameter.AttackLevel.CombineLatest(
                player.SkillParameter.CoolTimeLevel,
                player.SkillParameter.MaxScaleLevel,
                (attackLevel, coolTimeLevel, maxScaleLevel) =>
                    new { attackLevel, coolTimeLevel, maxScaleLevel }
            ).Subscribe(x =>
                uiManager.UpdateSkillLevelsText(
                    x.attackLevel,
                    x.coolTimeLevel,
                    x.maxScaleLevel
                )
            ).AddTo(this);
        }

        private void SetAutoSkillLevels(IPlayer player)
        {
            player.AutoSkillParameter.AttackLevel.CombineLatest(
                player.AutoSkillParameter.CoolTimeLevel,
                player.AutoSkillParameter.MaxScaleLevel,
                (attackLevel, coolTimeLevel, maxScaleLevel) =>
                    new { attackLevel, coolTimeLevel, maxScaleLevel }
            ).Subscribe(x =>
                uiManager.UpdateAutoSkillLevelsText(
                    x.attackLevel,
                    x.coolTimeLevel,
                    x.maxScaleLevel
                )
            ).AddTo(this);
        }

        private void SetWeapon(IPlayer player)
        {
            player.WeaponParameter.WeaponName.CombineLatest(
                player.WeaponParameter.WeaponDescription,
                (weaponName, weaponDescription) =>
                    new { weaponName, weaponDescription }
            ).Subscribe(x =>
                uiManager.UpdateWeaponText(
                    x.weaponName,
                    x.weaponDescription
                )
            ).AddTo(this);
            player.WeaponParameter.AttackPowerLevel.CombineLatest(
                player.WeaponParameter.MagazineLevel,
                player.WeaponParameter.BulletSpeedLevel,
                (attackLevel, magazineLevel, bulletSpeedLevel) =>
                    new { attackLevel, magazineLevel, bulletSpeedLevel }
            ).Subscribe(x =>
                uiManager.UpdateWeaponSlider(
                    x.attackLevel,
                    x.magazineLevel,
                    x.bulletSpeedLevel
                )
            ).AddTo(this);
        }

        private void SetSkill(IPlayer player)
        {
            player.SkillParameter.SkillName.CombineLatest(
                player.SkillParameter.SkillDescription,
                (skillName, skillDescription) =>
                    new { skillName, skillDescription }
            ).Subscribe(x =>
                uiManager.UpdateSkillText(
                    x.skillName,
                    x.skillDescription
                )
            ).AddTo(this);
            player.SkillParameter.AttackLevel.CombineLatest(
                player.SkillParameter.CoolTimeLevel,
                player.SkillParameter.MaxScaleLevel,
                (attackLevel, coolTimeLevel, maxScaleLevel) =>
                    new { attackLevel, coolTimeLevel, maxScaleLevel }
            ).Subscribe(x =>
                uiManager.UpdateSkillSlider(
                    x.attackLevel,
                    x.coolTimeLevel,
                    x.maxScaleLevel
                )
            ).AddTo(this);
        }

        private void SetAutoSkill(IPlayer player)
        {
            player.AutoSkillParameter.AutoSkillName.CombineLatest(
                player.AutoSkillParameter.AutoSkillDescription,
                (autoSkillName, autoSkillDescription) =>
                    new { autoSkillName, autoSkillDescription }
            ).Subscribe(x =>
                uiManager.UpdateAutoSkillText(
                    x.autoSkillName,
                    x.autoSkillDescription
                )
            ).AddTo(this);
            player.AutoSkillParameter.AttackLevel.CombineLatest(
                player.AutoSkillParameter.CoolTimeLevel,
                player.AutoSkillParameter.MaxScaleLevel,
                (attackLevel, coolTimeLevel, maxScaleLevel) =>
                    new { attackLevel, coolTimeLevel, maxScaleLevel }
            ).Subscribe(x =>
                uiManager.UpdateAutoSkillSlider(
                    x.attackLevel,
                    x.coolTimeLevel,
                    x.maxScaleLevel
                )
            ).AddTo(this);
        }

        private void PauseInGame()
        {
            //インゲーム中のみ一次停止
            if (_currentCanvasType == CanvasType.InGame)
            {
                uiManager.EnableMenuCanvas();
                _currentCanvasType = CanvasType.Menu;
            }
            else if (_currentCanvasType == CanvasType.LevelUp)
            {
                //何もしない
            }
        }

        private void ResumeInGame()
        {
            //一次停止中のみ再開
            if (_currentCanvasType == CanvasType.Menu)
            {
                uiManager.EnableInGameCanvas();
                _currentCanvasType = CanvasType.InGame;
            }
            else if (_currentCanvasType == CanvasType.LevelUp)
            {
                //何もしない
            }
        }

        private void SelectWeaponLevelUp()
        {
            ColorLevelUpSound();
            uiManager.EnableWeaponLevelUpCanvas();
        }

        private void SelectSkillLevelUp()
        {
            ColorLevelUpSound();
            uiManager.EnableSkillLevelUpCanvas();
        }

        private void SelectAutoSkillLevelUp()
        {
            ColorLevelUpSound();
            uiManager.EnableAutoSkillLevelUpCanvas();
        }

        private void ClearGame()
        {
            uiManager.EnableClearCanvas();
            soundManager.PlayClearSound();
        }

        private void EndSelectCard()
        {
            LevelUpSound();
            // ここでカードの選択が終わったときの処理を書く
            uiManager.EnableInGameCanvas();
        }

        private void DeathSound()
        {
            soundManager.PlayDeathSound();
        }

        private void ColorLevelUpSound()
        {
            soundManager.PlayColorLevelUpSound();
        }

        private void LevelUpSound()
        {
            soundManager.PlayLevelUpSound();
        }

        private void PlayerDamageSound()
        {
            soundManager.PlayPlayerDamageSound();
        }
    }
}

public enum CanvasType
{
    InGame,
    Menu,
    LevelUp
}