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
        //画面外に出てから消えるまでの時間
        private const float DisappearThreshold = 10.0f;
        protected BaseState(IEnemy enemy)
        {
            this.enemy = enemy;
            _disappearTimer = 0;
        }
        protected EventManager EventManager => enemy.EventManager;
        protected Camera MainCamera => enemy.MainCamera;
        //近い敵のリスト(距離を保つため)
        protected List<GameObject> CloseEnemies => enemy.CloseEnemies;
        protected MovementParameter MovementParameter => enemy.MovementParameter;
        protected Vector3 SpawnPositionInScreen => enemy.SpawnPositionInScreen;
        protected Vector3 SpawnPositionInWorld => enemy.SpawnPositionInWorld;
        
        public abstract IState.State State { get; }

        //共通処理
        public void Entry()
        {
            OnEntry();
        }
        //各状態の処理
        protected abstract void OnEntry();
        
        //共通処理
        public void Update()
        {
            OnUpdate();
        }
        //各状態の処理
        protected abstract void OnUpdate();

        //共通処理
        public void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;
            //各状態異常の回復に関する処理
            enemy.MovementParameter.RestoreCountDown(fixedDeltaTime);
            enemy.HitPoint.RestoreFromDamageBoostCountDown(fixedDeltaTime);

            //画面外に出たら消える処理
            if (IsOutOfScreen())
            {
                _disappearTimer += fixedDeltaTime;
                if (_disappearTimer >= DisappearThreshold&& !enemy.BossFlag)
                {
                    Disappear();
                }
            }
            //画面内にいる場合はタイマーをリセット
            else
            {
                _disappearTimer = 0;
            }

            OnFixedUpdate();
        }
        //各状態の処理
        protected abstract void OnFixedUpdate();

        //共通処理
        public void Exit()
        {
            OnExit();
        }
        //各状態の処理
        protected abstract void OnExit();

        //画面外に出たら消える処理
        private void Disappear()
        {
            enemy.SetIsTouchingPlayer(false);
            enemy.DestroySelf();
        }

        //画面外に出たかどうか
        public bool IsOutOfScreen()
        {
            //marginを設けておく
            //この0.2fはviewPort座標の0.2なので結構大きめ
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