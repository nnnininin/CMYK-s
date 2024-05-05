using System.Collections.Generic;
using Enemy.Parameter;
using Enemy.ScriptableObject;
using Enemy.Util;
using UnityEngine;

namespace Enemy
{
    public interface IEnemy
    {
        Camera MainCamera { get; }
        
        GameObject GameObject { get; }
        
        bool BossFlag { get; }
        
        //context
        State.StateContext Context { get; }
        
        EventManager EventManager { get; }
        
        int EnemyIdNumber { get; }

        // 敵に近い他のGameObjectのリスト
        List<GameObject> CloseEnemies { get; }

        // 敵のヒットポイント
        HitPoint HitPoint { get; }
        
        // 敵の移動パラメータ
        MovementParameter MovementParameter { get; }

        // 敵のスポーン位置（スクリーン座標）
        Vector3 SpawnPositionInScreen { get; }

        // 敵のスポーン位置（ワールド座標）
        Vector3 SpawnPositionInWorld { get; }
        
        void SetIsTouchingPlayer(bool isTouchingPlayer);
        
        void SetEnemyScriptableObject(EnemyScriptableObject enemyScriptableObject);

        void SetSpawnPositionInScreen(Vector3 spawnPositionInScreen);
        
        void SetTransformPosition(Vector3 position);
        
        void SetSpawnPositionInWorld(Vector3 spawnPositionInWorld);
        
        void SetEnemyIdNumber(int enemyIdNumber);
        
        void DestroySelf();
        void Die();
    }
}