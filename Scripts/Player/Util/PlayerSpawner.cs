using System.Collections.Generic;
using Player.Parameter;
using Player.ScriptableObject;
using Scene;
using UniRx;
using UnityEngine;
using Util.RayCaster;
using Zenject;

namespace Player.Util
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Inject]
        private DiContainer _container;

        [SerializeField]
        private TemporaryGameDataSo temporaryGameData;

        public Subject<GameObject> OnPlayerSpawned { get; } = new();
        private IPlayer _player;
        private RayCasterFromScreen rayCasterFromScreen;

        private GameObject _playerPrefab;
        private CharaSO charaSO;
        private WeaponSO weaponSO;
        private SkillSO skillSO;
        
        private void Awake()
        {
            rayCasterFromScreen = new RayCasterFromScreen();
        }
        
        private void Start()
        {
            LoadDataFromGameData();
            CheckAndSpawnPlayer();
        }

        private void LoadDataFromGameData()
        {
            _playerPrefab = temporaryGameData.PlayerPrefab.Value;
            charaSO = temporaryGameData.CharaSO.Value;
            weaponSO = temporaryGameData.WeaponSO.Value;
            skillSO = temporaryGameData.SkillSO.Value;
        }

        private void CheckAndSpawnPlayer()
        {
            if (_playerPrefab == null ||
                charaSO == null ||
                weaponSO == null ||
                skillSO == null
                )
            {
                Debug.LogError("Player data is not fully loaded.");
                return;
            }
            
            var centerScreenPosition = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, 0);
            var hitInfo = rayCasterFromScreen.GetRayCastHit(centerScreenPosition, Color.red);
            if (hitInfo == null) return;
            var hitPosition = hitInfo.Value.point;
            
            // プレイヤーの初期化
            _player = Player.Init(
                temporaryGameData,
                hitPosition,
                _container
                );
            var playerInstance = _player.GetPlayerInstance();
            playerInstance.transform.localPosition = hitPosition;
            OnPlayerSpawned.OnNext(playerInstance);
        }
    }
}