using Player;
using Player.Util;
using Scene;
using TMPro;
using UniRx;
using UnityEngine;

namespace UI
{
    public class DebugGameData : MonoBehaviour
    {
        [SerializeField]
        private PlayerSpawner playerSpawner;
        
        [SerializeField]
        private TemporaryGameDataSo temporaryGameData;

        [SerializeField]
        private TextMeshProUGUI debugGameDataText;

        [SerializeField]
        private TextMeshProUGUI debugPlayerText;

        private void Awake()
        {
            SubscribeToGameDataChanges();
            if(playerSpawner == null) return;
            playerSpawner.OnPlayerSpawned.Subscribe(UpdateParameterText).AddTo(this);
        }
        private void Start()
        {
            SetColor();
            DisplayDebugText();
        }
        
        private void SetColor()
        {
            debugGameDataText.color = Color.white;
            debugPlayerText.color = Color.white;
        }
        
        private void DisplayDebugText()
        {
            if (temporaryGameData.CharaSO != null
                && temporaryGameData.WeaponSO != null
                && temporaryGameData.SkillSO != null)
            {
                debugGameDataText.text
                    = $"Chara:{temporaryGameData.CharaSO.Value.name}\n" +
                      $"Weapon:{temporaryGameData.WeaponSO.Value.name}\n" +
                      $"Skill:{temporaryGameData.SkillSO.Value.name}\n"+
                      $"AutoSkill:{temporaryGameData.AutoSkillSO.Value.name}\n"+
                      $"Cyan Level: {temporaryGameData.ColorLevels[ColorType.Cyan].Value}\n" +
                      $"Magenta Level: {temporaryGameData.ColorLevels[ColorType.Magenta].Value}\n" +
                      $"Yellow Level: {temporaryGameData.ColorLevels[ColorType.Yellow].Value}\n" +
                      $"Cyan Exp: {temporaryGameData.ColorExp[ColorType.Cyan].Value}\n" +
                      $"Magenta Exp: {temporaryGameData.ColorExp[ColorType.Magenta].Value}\n" +
                      $"Yellow Exp: {temporaryGameData.ColorExp[ColorType.Yellow].Value}\n" +
                      $"Weapon Level0: {temporaryGameData.WeaponLevel.Value[0]}\n" +
                        $"Weapon Level1: {temporaryGameData.WeaponLevel.Value[1]}\n" +
                        $"Weapon Level2: {temporaryGameData.WeaponLevel.Value[2]}\n" +
                        $"Skill Level0: {temporaryGameData.SkillLevel.Value[0]}\n" +
                        $"Skill Level1: {temporaryGameData.SkillLevel.Value[1]}\n" +
                        $"Skill Level2: {temporaryGameData.SkillLevel.Value[2]}\n" +
                        $"AutoSkill Level0: {temporaryGameData.AutoSkillLevel.Value[0]}\n" +
                        $"AutoSkill Level1: {temporaryGameData.AutoSkillLevel.Value[1]}\n" +
                        $"AutoSkill Level2: {temporaryGameData.AutoSkillLevel.Value[2]}\n";
            }
            else
            {
                debugGameDataText.text = "GameData is null";
            }
        }
        
        private void DisplayPlayerText(IPlayer player)
        {
            // 直接文字列として組み合わせる
            var formattedText = $"HP: {player.HitPoint.CurrentHitPoint}/" +
                                $"{player.HitPoint.MaxHitPoint}\n" +
                                $"Fire Rate: {player.FireRate.FireRateValue}\n" +
                                $"Accuracy: {player.Accuracy.AccuracyValue}\n" +
                                $"Reload Time: {player.ReloadTime.ReloadTimeValue}\n" +
                                $"Attack Power: {player.AttackPower.AttackPowerValue}\n" +
                                $"Bullet Speed: {player.BulletSpeed.BulletSpeedValue}\n" +
                                $"Magazine: {player.Magazine.CurrentBullet}/"+
                                $"{player.Magazine.MaxBullet}\n" +
                                $"Skill Attack: {player.SkillParameter.Attack}\n" +
                                $"Skill CoolTime: {player.SkillParameter.CoolTime}\n" +
                                $"Skill Max Scale: {player.SkillParameter.MaxScale}\n" +
                                $"Auto Skill Attack: {player.AutoSkillParameter.Attack}\n" +
                                $"Auto Skill CoolTime: {player.AutoSkillParameter.CoolTime}\n" +
                                $"Auto Skill Max Scale: {player.AutoSkillParameter.MaxScale}\n" +
                                $"Weapon Attack Level: {player.WeaponParameter.AttackPowerLevel}\n" +
                                $"Weapon Magazine Level: {player.WeaponParameter.MagazineLevel}\n" +
                                $"Weapon Bullet Speed Level: {player.WeaponParameter.BulletSpeedLevel}\n" +
                                $"Skill Attack Level: {player.SkillParameter.AttackLevel}\n" +
                                $"Skill CoolTime Level: {player.SkillParameter.CoolTimeLevel}\n" +
                                $"Skill Max Scale Level: {player.SkillParameter.MaxScaleLevel}\n" +
                                $"Auto Skill Attack Level: {player.AutoSkillParameter.AttackLevel}\n" +
                                $"Auto Skill CoolTime Level: {player.AutoSkillParameter.CoolTimeLevel}\n" +
                                $"Auto Skill Max Scale Level: {player.AutoSkillParameter.MaxScaleLevel}\n";

            // 最終的なテキストをdebugPlayerTextに設定
            debugPlayerText.text = formattedText;
        }
        
        private void UpdateParameterText(GameObject playerInstance)
        {
            var player = playerInstance.GetComponentInChildren<IPlayer>();
            if (player == null) return;
            
            player.HitPoint.CurrentHitPoint
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.HitPoint.MaxHitPoint
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.Magazine.CurrentBullet
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.Magazine.MaxBullet
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AttackPower.AttackPowerValue
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.BulletSpeed.BulletSpeedValue
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.FireRate.FireRateValue
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.Accuracy.AccuracyValue
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.ReloadTime.ReloadTimeValue
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.Attack
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.CoolTime
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.LifeTime
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.MaxScale
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.Attack
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.CoolTime
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.LifeTime
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.MaxScale
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            
            //レベル変更を監視
            player.WeaponParameter.AttackPowerLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.WeaponParameter.MagazineLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.WeaponParameter.BulletSpeedLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.AttackLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.MaxScaleLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.SkillParameter.CoolTimeLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.AttackLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.MaxScaleLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
            player.AutoSkillParameter.CoolTimeLevel
                .Subscribe(_ => DisplayPlayerText(player))
                .AddTo(this);
        }
        private void SubscribeToGameDataChanges()
        {
            // CharaSOの変更を監視
            temporaryGameData.CharaSO
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
    
            // WeaponSOの変更を監視
            temporaryGameData.WeaponSO
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);

            // SkillSOの変更を監視
            temporaryGameData.SkillSO
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
            // AutoSkillSOの変更を監視
            temporaryGameData.AutoSkillSO
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
            // Cyanのレベル変更を監視
            temporaryGameData.ColorLevels[ColorType.Cyan]
                .Subscribe(cyanLevel => DisplayDebugText())
                .AddTo(this);
            // Magentaのレベル変更を監視
            temporaryGameData.ColorLevels[ColorType.Magenta]
                .Subscribe(magentaLevel => DisplayDebugText())
                .AddTo(this);
            // Yellowのレベル変更を監視
            temporaryGameData.ColorLevels[ColorType.Yellow]
                .Subscribe(yellowLevel => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.ColorExp[ColorType.Cyan]
                .Subscribe(cyanExp => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.ColorExp[ColorType.Magenta]
                .Subscribe(magentaExp => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.ColorExp[ColorType.Yellow]
                .Subscribe(yellowExp => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.WeaponLevel
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.SkillLevel
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
            temporaryGameData.AutoSkillLevel
                .Subscribe(_ => DisplayDebugText())
                .AddTo(this);
        }
    }
}
