using System;
using Enemy.State.StateControllers;
using UnityEngine;
using Util;

namespace Enemy.ScriptableObject
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObject : UnityEngine.ScriptableObject
    {
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
        
        [SerializeField]
        private float chasingAccuracy;
        public float ChasingAccuracy => chasingAccuracy;
        
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