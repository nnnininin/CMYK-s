using Player.ScriptableObject;
using Scene;
using UnityEngine;

namespace UI.StartScene
{
    public class PlayerSelectButton : MonoBehaviour
    {
        [SerializeField]
        private SceneLoader sceneLoader;
        [SerializeField]
        private TemporaryGameDataSo temporaryGameData;
        [SerializeField]
        private GameObject playerPrefab;
        [SerializeField]
        private CharaSO charaSO;
        [SerializeField]
        private WeaponSO mgWeaponSO;
        [SerializeField]
        private WeaponSO sgWeaponSO;
        [SerializeField]
        private WeaponSO smgWeaponSO;
        [SerializeField]
        private WeaponSO penetrateWeaponSO;
        [SerializeField]
        private SkillSO speedDownSkillSO;
        [SerializeField]
        private SkillSO boostSkillSO;
        [SerializeField]
        private AutoSkillSO crossAutoSkillSO;
        [SerializeField]
        private AutoSkillSO circulateAutoSkillSO;
        [SerializeField]
        private AutoSkillSO surroundAutoSkillSO;
        
        private const string NextSceneName = "ActionScene";

        //load
        public void OnClickLoad()
        {
            SetupScriptableObject();
            if (temporaryGameData.CharaSO == null)
            {
                Debug.LogError("GameData is null");
            }
            sceneLoader.LoadScene(NextSceneName);
        }
        
        private void SetupScriptableObject()
        {
            var weaponSO = RandomWeaponSO();
            var skillSO = RandomSkillSO();
            var autoSkillSO = RandomAutoSkillSO();
            temporaryGameData.Initialize(
                playerPrefab,
                charaSO, 
                weaponSO,
                skillSO,
                autoSkillSO
                );
        }
        
        private WeaponSO RandomWeaponSO()
        {
            var random = Random.Range(0, 4);
            return random switch
            {
                0 => mgWeaponSO,
                1 => sgWeaponSO,
                2 => smgWeaponSO,
                3 => penetrateWeaponSO,
                _ => penetrateWeaponSO
            };
        }
        
        private SkillSO RandomSkillSO()
        {
            var random = Random.Range(0, 2);
            return random switch
            {
                0 => speedDownSkillSO,
                1 => boostSkillSO,
                _ => speedDownSkillSO
            };
        }
        
        private AutoSkillSO RandomAutoSkillSO()
        {
            var random = Random.Range(0, 3);
            return random switch
            {
                0 => crossAutoSkillSO,
                1 => circulateAutoSkillSO,
                2 => surroundAutoSkillSO,
                _ => crossAutoSkillSO
            };
        }
    }
}