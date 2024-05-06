using System;
using Enemy.State.StateControllers;
using UnityEngine;
using Util;

namespace Enemy.ScriptableObject
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : UnityEngine.ScriptableObject
    {
        //敵のプレハブはここで設定する
        [SerializeField]
        private GameObject enemyPrefab;
        public GameObject EnemyPrefab => enemyPrefab;
        
        //bossであればtrue
        [SerializeField] private bool bossFlag;
        public bool BossFlag => bossFlag;
        
        [SerializeField]
        private string enemyName;
        public string EnemyName => enemyName;
        
        [SerializeField]
        private string description;
        public string Description => description;
        //評価値(敵の出現パターンに関係)
        [SerializeField]
        private int evaluation;
        public int Evaluation => evaluation;

        [SerializeField]
        private int maxHitPoint;
        public int MaxHitPoint => maxHitPoint;

        [SerializeField]
        private int attackPower;
        public int AttackPower => attackPower;

        [SerializeField]
        private int movingSpeed;
        public int MovingSpeed => movingSpeed;
        
        //追跡の精度
        [SerializeField]
        private float chasingAccuracy;
        public float ChasingAccuracy => chasingAccuracy;
        
        //敵の行動パターン遷移条件をIControllerを継承したクラスで設定する
        [SerializeReference][SubclassSelector]
        private IController controller;
        
        public IController CreateControllerInstance(IEnemy enemy)
        {
            var controllerType = controller.GetType();
            var controllerInstance = Activator.CreateInstance(controllerType);
            if (controllerInstance is not IController newController) return null;
            newController.Initialize(enemy);
            newController.AddUniqueStates();
            return newController;
        }
    }
}