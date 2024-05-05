using Enemy.Util;
using Manager.DIContainer;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scene
{
    public class StageManager : MonoBehaviour
    {
        [Inject]
        private GlobalEnemyEventManager _globalEnemyEventManager;
        [SerializeField]
        private EnemySpawner enemySpawner;
        public Subject<Unit> OnStageClear { get; } = new();

        private void Awake()
        {
            enemySpawner.OnStageEnd.Subscribe(
                _ => StageCleared()).
                AddTo(this);
        }
        
        private void StageCleared()
        {
            //ステージクリア時の処理
            //OnStageClear.OnNext(Unit.Default);
        }
    }
}