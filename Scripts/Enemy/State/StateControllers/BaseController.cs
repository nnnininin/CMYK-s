using Enemy.Parameter;
using Enemy.State.BaseState.DeathState;
using Enemy.State.BaseState.IdleState;
using UnityEngine;

namespace Enemy.State.StateControllers
{
    public abstract class BaseController : IController
    {
        protected IEnemy Enemy;
        protected StateContext StateContext;
        protected HitPoint HitPoint;
        private int enemyIdNumber;
        
        public void Initialize(IEnemy enemy)
        {
            Enemy = enemy;
            StateContext = Enemy.Context;
            HitPoint = Enemy.HitPoint;
            enemyIdNumber = Enemy.EnemyIdNumber;
            Debug.Log($"EnemyIdNumber: {enemyIdNumber}");
            AddCommonStates(Enemy);
        }
        
        // 派生クラスで固有の状態を追加するためのメソッド
        public abstract void AddUniqueStates();

        public IState.State GetInitialState()
        {
            return IState.State.Idle;
        }
        
        // 状態遷移をチェックするためのメソッド
        public abstract IState.State CheckTransitions();
        
        // 共通の状態を設定
        private void AddCommonStates(IEnemy enemy)
        {
            StateContext.AddState(IState.State.Idle, new IdleState(enemy));
            StateContext.AddState(IState.State.Death, new DeathState(enemy));
        }
    }
}