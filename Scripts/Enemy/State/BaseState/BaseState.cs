using System.Collections.Generic;
using Enemy.Parameter;
using Enemy.Util;
using UnityEngine;

namespace Enemy.State.BaseState
{
    public abstract class BaseState : IState.IState, IIsOutOfScreen
    {
        private readonly IEnemy enemy;
        private float _disappearTimer;
        private const float DisappearThreshold = 5.0f;
        protected BaseState(IEnemy enemy)
        {
            this.enemy = enemy;
            _disappearTimer = 0;
        }
        protected EventManager EventManager => enemy.EventManager;
        protected Camera MainCamera => enemy.MainCamera;
        protected List<GameObject> CloseEnemies => enemy.CloseEnemies;
        protected MovementParameter MovementParameter => enemy.MovementParameter;
        protected Vector3 SpawnPositionInScreen => enemy.SpawnPositionInScreen;
        protected Vector3 SpawnPositionInWorld => enemy.SpawnPositionInWorld;
        
        public abstract IState.State State { get; }

        public void Entry()
        {
            OnEntry();
        }
        protected abstract void OnEntry();
        
        public void Update()
        {
            OnUpdate();
        }
        protected abstract void OnUpdate();

        public void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;
            enemy.MovementParameter.RestoreCountDown(fixedDeltaTime);
            enemy.HitPoint.RestoreCountDown(fixedDeltaTime);

            if (IsOutOfScreen())
            {
                _disappearTimer += fixedDeltaTime;
                if (_disappearTimer >= DisappearThreshold&& !enemy.BossFlag)
                {
                    Disappear();
                }
            }
            else
            {
                _disappearTimer = 0;
            }

            OnFixedUpdate();
        }
        protected abstract void OnFixedUpdate();

        public void Exit()
        {
            OnExit();
        }
        protected abstract void OnExit();

        private void Disappear()
        {
            enemy.SetIsTouchingPlayer(false);
            enemy.DestroySelf();
        }

        public bool IsOutOfScreen()
        {
            const float viewOutMargin = 0.2f;
            var viewPortPosition = MainCamera.WorldToViewportPoint(MovementParameter.CurrentPosition);
            return viewPortPosition.x <= -viewOutMargin || viewPortPosition.x >= 1 + viewOutMargin ||
                   viewPortPosition.y <= -viewOutMargin || viewPortPosition.y >= 1 + viewOutMargin;
        }
        
        protected void Die()
        {
            enemy.Die();
        }
    }
}