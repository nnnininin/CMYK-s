using Player.Skill;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Util;

namespace Enemy.State.BaseState.GoStraightState.ExplodeState
{
    public class ExplodeState : 
        GoStraightState
    {
        public ExplodeState(IEnemy enemy)
            : base(enemy) { }
        public override IState.State State => IState.State.Explode;
        
        private IFieldActivator _fieldActivator;

        protected override void OnEntry()
        {
            // プレハブのみを非同期でロード
            Addressables.LoadAssetAsync<GameObject>(StaticAddressableKeys.DamageZone)
                .Completed += OnPrefabLoaded;

        }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnExit() { }
        
        private void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            _fieldActivator ??= new SteadyFieldActivator();
            
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                const int damage = 10;
                const float initialScale = 1.0f;
                const float lifeTime = 5.0f;
                
                // プレハブのロードが成功したので、それを使ってダメージフィールドを生成
                var prefab = obj.Result;
                    _fieldActivator.ActivatePrefab(
                    prefab,
                    MovementParameter.CurrentPosition,
                    damage,
                    initialScale,
                    lifeTime
                );
            }
            else
            {
                Debug.LogError("Failed to load the DamageZone prefab.");
            }
            Die();
        }
    }
}