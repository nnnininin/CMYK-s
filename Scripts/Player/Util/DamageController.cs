using UnityEngine;
using Util.Interface;

namespace Player.Util
{
    public class DamageController :IReceiveDamage
    {
        private readonly IPlayer _player;
        
        private EventManager eventManager;

        public DamageController(IPlayer player)
        {
            _player = player;
            SetUniRx();
        }
        private void SetUniRx()
        {
            eventManager = _player.EventManager;
        }
        
        public void RecoverDamage(int recoverPoint)
        {
            Debug.Log($"Player recovered {recoverPoint} damage.");
        }
        
        public void ReceiveDamage(int damage)
        {
            Debug.Log($"Player received {damage} damage.");
            
                _player.HitPoint.ReceiveDamage(damage);
                eventManager.TriggerOnDamageHitPoint();
                if(_player.HitPoint.CurrentHitPoint.Value <= 0)
                {
                    eventManager.TriggerOnDeath();
                }
        }
    }
}