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
        private const float DamageBoostRate = 1.5f;

        private const float DamageBoostDuration = 5.0f;
        private float _timeToDamageBoost;
        public bool IsDead => CurrentHitPoint <= 0;

        private readonly Subject<int> _onDamageReceived = new();
        public IObservable<int> OnDamageReceived => _onDamageReceived;
        
        public HitPoint(int maxHitPoint)
        {
            MaxHitPoint = maxHitPoint;
            CurrentHitPoint = maxHitPoint;
            _timeToDamageBoost = 0;
        }
        
        //ダメージを回復する
        public void RecoverDamage(int recoverPoint)
        {
            var newHitPoint = CurrentHitPoint + recoverPoint;
            CurrentHitPoint = newHitPoint > MaxHitPoint ? MaxHitPoint : newHitPoint;
        }
        
        //ダメージを受ける
        public void ReceiveDamage(int damage)
        {
            if (_timeToDamageBoost > 0)
            {
                damage = (int)(damage * DamageBoostRate);
            }
            Debug.Log("ReceiveDamage: " + damage);
            CurrentHitPoint -= damage;
            _onDamageReceived.OnNext(damage);
            Debug.Log("CurrentHitPoint: " + CurrentHitPoint);
            Debug.Log("IsDead: " + IsDead);
        }
        
        //受けるダメージが増大する、ダメージブースト状態にする
        public void SetDamageBoost()
        {
            _timeToDamageBoost = DamageBoostDuration;
        }
        
        //ダメージブーストが直るまでのカウントダウン
        public void RestoreFromDamageBoostCountDown(float deltaTime)
        {
            if (_timeToDamageBoost <= 0) return;
            _timeToDamageBoost -= deltaTime;
            if (_timeToDamageBoost > 0) return;
            _timeToDamageBoost = 0;
        }
    }
}