using Enemy.Util;
using UniRx;
using UnityEngine;

namespace Manager.DIContainer
{
    public class GlobalEnemyEventManager : MonoBehaviour
    {
        private Subject<int> OnGlobalDamageReceived { get; } = new();
        public Subject<Unit> OnGlobalDeath { get; } = new();
        public Subject<Unit> OnGlobalSummon { get; } = new();
        public Subject<Unit> OnGlobalBossDeath { get; } = new();
        
        public void RegisterEnemyEventManager(EventManager eventManager, MonoBehaviour owner)
        {
            eventManager.OnDamageReceived.Subscribe(OnGlobalDamageReceived.OnNext).AddTo(owner);
            eventManager.OnDeath.Subscribe(OnGlobalDeath.OnNext).AddTo(owner);
            eventManager.OnSummon.Subscribe(OnGlobalSummon.OnNext).AddTo(owner);
            eventManager.OnBossDeath.Subscribe(OnGlobalBossDeath.OnNext).AddTo(owner);
        }
    }
}