using Cinemachine;
using Manager.DIContainer;
using Player.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraScript
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ShakeCamera: MonoBehaviour
    {
        [Inject]
        private GlobalEnemyEventManager _globalEnemyEventManager;
        [Inject]
        private GlobalPlayerEventManager _globalPlayerEventManager;

        //敵の死亡時の振動
        private CinemachineImpulseSource _enemyDeathImpulseSource;
        //プレイヤーのダメージ時の振動
        private CinemachineImpulseSource _playerDamageImpulseSource;
        
        private void Start()
        {
            var impulseSources = GetComponents<CinemachineImpulseSource>();
            _enemyDeathImpulseSource = impulseSources[0];
            _playerDamageImpulseSource = impulseSources[1];
            
            SetUniRx();
        }

        private void SetUniRx()
        {
            _globalEnemyEventManager.OnGlobalDeath.Subscribe(_ =>
                {
                    _enemyDeathImpulseSource.GenerateImpulse();
                }
                ).AddTo(this);
            _globalPlayerEventManager.OnGlobalDamageReceived.Subscribe(_ =>
                {
                    _playerDamageImpulseSource.GenerateImpulse();
                }
            ).AddTo(this);
        }
    }
}