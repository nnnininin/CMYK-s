using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.ActionScene
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas inGameCanvas;
        [SerializeField] private Canvas menuCanvas;
        [SerializeField] private Canvas weaponLevelUpCanvas;
        [SerializeField] private Canvas skillLevelUpCanvas;
        [SerializeField] private Canvas autoSkillLevelUpCanvas;
        [SerializeField] private Canvas clearCanvas;
        
        [SerializeField] private TextMeshProUGUI magazineText;
        [SerializeField] private TextMeshProUGUI hitPointText;
        
        [SerializeField] private TextMeshProUGUI weaponLevelsText;
        [SerializeField] private TextMeshProUGUI skillLevelsText;
        [SerializeField] private TextMeshProUGUI autoSkillLevelsText;

        [SerializeField] private TextMeshProUGUI currentWeaponNameText;
        [SerializeField] private TextMeshProUGUI currentWeaponDescriptionText;
        [SerializeField]private TextMeshProUGUI currentSkillNameText;
        [SerializeField] private TextMeshProUGUI currentSkillDescriptionText;
        [SerializeField] private TextMeshProUGUI currentAutoSkillText;
        [SerializeField] private TextMeshProUGUI currentAutoSkillDescriptionText;
        
        [SerializeField] private Slider weaponExpSlider;
        [SerializeField] private Slider skillExpSlider;
        [SerializeField] private Slider autoSkillExpSlider;
        
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider weaponSlider;
        [SerializeField] private Slider skillSlider;
        [SerializeField] private Slider autoSkillSlider;
        
        [SerializeField] private Slider weaponAttackSlider;
        [SerializeField] private Slider weaponMagazineSlider;
        [SerializeField] private Slider weaponBulletSpeedSlider;
        [SerializeField] private Slider skillAttackSlider;
        [SerializeField] private Slider skillCoolTimeSlider;
        [SerializeField] private Slider skillMaxScaleSlider;
        [SerializeField] private Slider autoSkillAttackSlider;
        [SerializeField] private Slider autoSkillCoolTimeSlider;
        [SerializeField] private Slider autoSkillMaxScaleSlider;
        
        [SerializeField] private TimeScaleManager timeScaleManager;
        
        private List<Canvas> _canvasList;
        
        private void Awake()
        {
            //リストに全てのcanvasを追加
            _canvasList = new List<Canvas>
            {
                inGameCanvas,
                menuCanvas,
                weaponLevelUpCanvas,
                skillLevelUpCanvas,
                autoSkillLevelUpCanvas,
                clearCanvas
            };
            EnableInGameCanvas();
        }
        
        public void UpdateWeaponExpSlider(float currentExp, float maxExp)
        {
            weaponExpSlider.maxValue = maxExp;
            weaponExpSlider.value = currentExp;
        }
        
        public void UpdateSkillExpSlider(float currentExp, float maxExp)
        {
            skillExpSlider.maxValue = maxExp;
            skillExpSlider.value = currentExp;
        }
        
        public void UpdateAutoSkillExpSlider(float currentExp, float maxExp)
        {
            autoSkillExpSlider.maxValue = maxExp;
            autoSkillExpSlider.value = currentExp;
        }
        
        public void UpdateHpSlider(float maxHp, float currentHp)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
        }
        
        public void UpdateWeaponSlider(float maxWeapon, float currentWeapon)
        {
            weaponSlider.maxValue = maxWeapon;
            weaponSlider.value = currentWeapon;
        }
        
        public void UpdateSkillSlider(float maxSkill, float currentSkill)
        {
            skillSlider.maxValue = maxSkill;
            skillSlider.value = currentSkill;
        }
        
        public void UpdateAutoSkillSlider(float maxAutoSkill, float currentAutoSkill)
        {
            autoSkillSlider.maxValue = maxAutoSkill;
            autoSkillSlider.value = currentAutoSkill;
        }
        
        public void UpdateBulletText(int maxBullet,int currentBullet)
        {
            var bulletText = $"{currentBullet}/{maxBullet}";
            magazineText.text = bulletText;
        }
        
        public void UpdateHpText(int maxHp,int currentHp)
        {
            var hpText = $"HP:{currentHp}/{maxHp}";
            hitPointText.text = hpText;
        }
        
        public void UpdateWeaponLevelsText(
            int attackLevel,
            int magazineLevel,
            int bulletSpeedLevel
            )
        {
            var formattedText = $"{attackLevel}\n{magazineLevel}\n{bulletSpeedLevel}";
            weaponLevelsText.text = formattedText;
        }
        
        public void UpdateSkillLevelsText(
            int attackLevel,
            int coolTimeLevel,
            int maxScaleLevel
            )
        {
            var formattedText = $"{attackLevel}\n{coolTimeLevel}\n{maxScaleLevel}";
            skillLevelsText.text = formattedText;
        }
        
        public void UpdateAutoSkillLevelsText(
            int attackLevel,
            int coolTimeLevel, 
            int maxScaleLevel
            )
        {
            var formattedText = $"{attackLevel}\n{coolTimeLevel}\n{maxScaleLevel}";
            autoSkillLevelsText.text = formattedText;
        }

        public void UpdateWeaponText(string weaponName, string weaponDescription)
        {
            var nameText = "現在の左クリックスキル\n" + $"<size=180%>{weaponName}</size>";
            var descriptionText = $"<color=white>{weaponDescription}</color>";
            //改行を補正
            if(descriptionText.Contains("\\n"))
                descriptionText = descriptionText.Replace("\\n", "\n");
            currentWeaponNameText.text = nameText;
            currentWeaponDescriptionText.text = descriptionText;
        }

        public void UpdateSkillText(string skillName, string skillDescription)
        {
            var nameText = "現在の右クリックスキル\n" + $"<size=180%>{skillName}</size>";
            var descriptionText = $"<color=white>{skillDescription}</color>";
            //改行を補正
            if(descriptionText.Contains("\\n"))
                descriptionText = descriptionText.Replace("\\n", "\n");
            currentSkillNameText.text = nameText;
            currentSkillDescriptionText.text = descriptionText;
        }
        
        public void UpdateAutoSkillText(string autoSkillName, string autoSkillDescription)
        {
            var nameText = "現在の自動スキル\n" + $"<size=180%>{autoSkillName}</size>";
            var descriptionText = $"<color=white>{autoSkillDescription}</color>";
            //改行を補正
            if(descriptionText.Contains("\\n"))
                descriptionText = descriptionText.Replace("\\n", "\n");
            currentAutoSkillText.text = nameText;
            currentAutoSkillDescriptionText.text = descriptionText;
        }
        
        public void SetLevelSliderMax(
            int maxWeaponLevel,
            int maxSkillLevel,
            int maxAutoSkillLevel
            )
        {
            weaponAttackSlider.maxValue = maxWeaponLevel;
            weaponMagazineSlider.maxValue = maxWeaponLevel;
            weaponBulletSpeedSlider.maxValue = maxWeaponLevel;
            skillAttackSlider.maxValue = maxSkillLevel;
            skillCoolTimeSlider.maxValue = maxSkillLevel;
            skillMaxScaleSlider.maxValue = maxSkillLevel;
            autoSkillAttackSlider.maxValue = maxAutoSkillLevel;
            autoSkillCoolTimeSlider.maxValue = maxAutoSkillLevel;
            autoSkillMaxScaleSlider.maxValue = maxAutoSkillLevel;
        }
        
        public void UpdateWeaponSlider(
            int attackPower,
            int magazine,
            int bulletSpeed
            )
        {
            weaponAttackSlider.value = attackPower;
            weaponMagazineSlider.value = magazine;
            weaponBulletSpeedSlider.value = bulletSpeed;
        }
        public void UpdateSkillSlider(
            int attack,
            int coolTime,
            float maxScale
            )
        {
            skillAttackSlider.value = attack;
            skillCoolTimeSlider.value = coolTime;
            skillMaxScaleSlider.value = maxScale;
        }
        public void UpdateAutoSkillSlider(
            int attack,
            int coolTime,
            float maxScale
            )
        {
            autoSkillAttackSlider.value = attack;
            autoSkillCoolTimeSlider.value = coolTime;
            autoSkillMaxScaleSlider.value = maxScale;
        }
        
        private void DisAbleAllCanvas()
        {
            foreach (var canvas in _canvasList)
            {
                canvas.enabled = false;
            }
        }
        
        public void EnableInGameCanvas()
        {
            timeScaleManager.ResumeTime();
            DisAbleAllCanvas();
            inGameCanvas.enabled = true;
        }
        
        public void EnableMenuCanvas()
        {
            timeScaleManager.StopTime();
            DisAbleAllCanvas();
            menuCanvas.enabled = true;
        }
        
        public void EnableWeaponLevelUpCanvas()
        {
            timeScaleManager.StopTime();
            DisAbleAllCanvas();
            weaponLevelUpCanvas.enabled = true;
        }
        
        public void EnableSkillLevelUpCanvas()
        {
            timeScaleManager.StopTime();
            DisAbleAllCanvas();
            skillLevelUpCanvas.enabled = true;
        }

        public void EnableAutoSkillLevelUpCanvas()
        {
            timeScaleManager.StopTime();
            DisAbleAllCanvas();
            autoSkillLevelUpCanvas.enabled = true;
        }
        
        public void EnableClearCanvas()
        {
            timeScaleManager.StopTime();
            DisAbleAllCanvas();
            clearCanvas.enabled = true;
        }
    }
}