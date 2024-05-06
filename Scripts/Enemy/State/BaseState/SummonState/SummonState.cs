using UniRx;
using UnityEngine;

namespace Enemy.State.BaseState.SummonState
{
    // 召喚する状態
    public class SummonState : BaseState
    {
        public SummonState(IEnemy enemy) : base(enemy) { }
        
        public override IState.State State => IState.State.Summon;
        private float _timeSinceLastSpawn;
        private const float SpawnInterval = 2.0f;
        
        protected override void OnEntry()
        {
            _timeSinceLastSpawn = SpawnInterval/2;
        }
        
        protected override void OnUpdate() { }

        protected override void OnFixedUpdate()
        {
            _timeSinceLastSpawn += Time.fixedDeltaTime;
            if (!(_timeSinceLastSpawn >= SpawnInterval)) return;
            _timeSinceLastSpawn = 0;
            TrySpawnEnemy();
        }
        protected override void OnExit() { }

        private void TrySpawnEnemy()
        {
            //eventManagerに通知
            EventManager.OnSummon.OnNext(Unit.Default);
        }
    }
}