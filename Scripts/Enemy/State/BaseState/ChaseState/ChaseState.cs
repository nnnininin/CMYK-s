using System.Collections.Generic;
using UnityEngine;
using Util.Calculator;

namespace Enemy.State.BaseState.ChaseState
{
    // 追跡状態
    //近い敵との距離を保つ処理あり
    public class ChaseState : BaseState, IGetSeparationVector
    {
        public ChaseState(IEnemy enemy)
            : base(enemy) { }
        
        public override IState.State State => IState.State.Chase;
        
        protected override void OnEntry()
        {
            var currentVelocity =
                (MovementParameter.TargetPosition - 
                 MovementParameter.CurrentPosition).normalized *
                MovementParameter.MovingSpeed;
            MovementParameter.SetCurrentVelocity(currentVelocity);
        }
        protected override void OnUpdate() { }

        protected override void OnFixedUpdate()
        {
            var newVelocity =
                ConstantSpeedAutoTracking.CalculateTrackingVector(
                MovementParameter.TargetPosition,
                MovementParameter.CurrentPosition,
                MovementParameter.CurrentVelocity,
                MovementParameter.ChasingAccuracy
            );

            var separationVector = GetSeparationVector();
            newVelocity += separationVector;
            //newVelocityのy成分を0にする
            newVelocity.y = 0;
            
            //newVelocityの長さを速さに設定
            var newVelocityNormalized = newVelocity.normalized * MovementParameter.MovingSpeed;
            MovementParameter.SetCurrentVelocity(newVelocityNormalized);
        }
        protected override void OnExit() { }
        
        // 敵と他の敵との間に適切な距離を保つためのベクトルを取得
        public Vector3 GetSeparationVector()
        {
            const float separationDistance = 5.0f;
            var separationVector = Vector3.zero;
            var enemiesToRemove = new List<GameObject>(); // 削除する敵を一時的に格納するリスト

            //距離が近い敵を取得
            foreach (var closeEnemy in CloseEnemies)
            {
                if (!closeEnemy)
                {
                    enemiesToRemove.Add(closeEnemy); // 削除リストに追加
                    continue;
                }
                var distanceVector = MovementParameter.CurrentPosition - closeEnemy.transform.position;
                var distance = distanceVector.magnitude;
                if (distance is > 0 and < separationDistance)
                {
                    separationVector += distanceVector.normalized *
                                        (separationDistance - distance) /
                                        separationDistance;
                }
            }
            // 削除対象の敵をリストから削除
            foreach (var enemyToRemove in enemiesToRemove)
            {
                CloseEnemies.Remove(enemyToRemove);
            }
            separationVector.y = 0;
            return separationVector;
        }
    }
}
