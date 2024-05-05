using Player.Util;
using UnityEngine;

namespace Player.State.IState.BaseState
{  
    public abstract class BaseState : IState, ICountSkillCoolTime
    {
        protected readonly IPlayer Player;
        protected BaseState(IPlayer player)
        {
            Player = player;
        }

        protected EventManager EventManager => Player.EventManager;

        public abstract State State { get; }

        public void Entry()
        {
            var stateName = GetType().Name;
            Debug.Log($"{stateName} Entry called");
            Player.MoveSpeed.SetIsMoveLow(false);
            OverriddenEntry();
        }
        
        protected abstract void OverriddenEntry();
        
        public abstract void Update();
        
        public void FixedUpdate()
        {
            Debug.Log($"coolTimeCount: {Player.SkillParameter.CoolTimeCount.Value}");
            CountSkillCoolTime();
            OverriddenFixedUpdate();
        }

        public abstract void OverriddenFixedUpdate();
        
        public virtual void CountSkillCoolTime()
        {
            if(Player.SkillParameter.CoolTimeCount.Value <= 0f)
                return;
            var isEndCountDown = Player.SkillParameter.CountDownCoolTime(Time.fixedDeltaTime);
            if (isEndCountDown)
            {
                Debug.Log("EndCountDown");
                EventManager.TriggerOnSkillAble();
            }
        }

        public abstract void Exit();
    }
}