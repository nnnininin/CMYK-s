using System;
using UniRx;
using UnityEngine;
using Util.Interface;

namespace Enemy.Parameter
{
    public class HitPoint: IReceiveDamage
    {
        private int MaxHitPoint { get; }
        private int CurrentHitPoint { get; set; }
        //ダメージブースト倍率
        private float _damageBoostRate = 1.5f;
        
        private const float DamageBoostDuration = 5.0f;
        private float _timeToRestoreDamageBoost;
        public bool IsDead => CurrentHitPoint <= 0;

        private readonly Subject<int> _onDamageReceived = new();
        public IObservable<int> OnDamageReceived => _onDamageReceived;
        
        public HitPoint(int maxHitPoint)
        {
            MaxHitPoint = maxHitPoint;
            CurrentHitPoint = maxHitPoint;
            _timeToRestoreDamageBoost = 0;
        }
        
        public void RecoverDamage(int recoverPoint)
        {
            var newHitPoint = CurrentHitPoint + recoverPoint;
            CurrentHitPoint = newHitPoint > MaxHitPoint ? MaxHitPoint : newHitPoint;
        }
        
        public void ReceiveDamage(int damage)
        {
            if (_timeToRestoreDamageBoost > 0)
            {
                damage = (int)(damage * _damageBoostRate);
            }
            Debug.Log("ReceiveDamage: " + damage);
            CurrentHitPoint -= damage;
            _onDamageReceived.OnNext(damage);
            Debug.Log("CurrentHitPoint: " + CurrentHitPoint);
            Debug.Log("IsDead: " + IsDead);
        }
        
        public void SetDamageBoost()
        {
            _timeToRestoreDamageBoost = DamageBoostDuration;
        }
        
        public void RestoreCountDown(float deltaTime)
        {
            if (_timeToRestoreDamageBoost <= 0) return;
            _timeToRestoreDamageBoost -= deltaTime;
            if (_timeToRestoreDamageBoost > 0) return;
            _timeToRestoreDamageBoost = 0;
        }
    }
}