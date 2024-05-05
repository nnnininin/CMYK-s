using UniRx;
using UnityEngine;

namespace Player.Util
{
    public class GlobalPlayerEventManager : MonoBehaviour
    {
        public Subject<Unit> OnGlobalDamageReceived { get; } = new();
        public Subject<Unit> OnGlobalDeath { get; } = new();

        public void RegisterPlayerEventManager(EventManager eventManager, MonoBehaviour owner)
        {
            eventManager.OnDamageHitPoint.Subscribe(OnGlobalDamageReceived.OnNext).AddTo(owner);
            eventManager.OnDeath.Subscribe(OnGlobalDeath.OnNext).AddTo(owner);
        }
    }
}