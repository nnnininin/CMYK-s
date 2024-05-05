using System.Collections.Generic;
using Enemy.Parameter;
using Enemy.ScriptableObject;
using Enemy.State;
using Enemy.State.StateControllers;
using Enemy.Util;
using Manager.DIContainer;
using UniRx;
using UnityEngine;
using Util.Animation;
using Zenject;
using EventManager = Enemy.Util.EventManager;

namespace Enemy
{
    [RequireComponent(typeof(RotationAnimation))]
    [RequireComponent(typeof(EnemyDieAnimation))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BlinkEnemy))]
    public class BaseEnemy : MonoBehaviour, IEnemy
    {
        [Inject] private GlobalEnemyEventManager _globalEnemyEventManager;
       
        [SerializeField]
        private Collider enemyCollider;

        private const string EnemyTag = "Enemy";
        private const string PlayerTag = "Player";
        private const float DamageInterval = 1.0f;
        
        private float _damageTimer;
        public bool BossFlag => EnemyScriptableObject.BossFlag;
        public Camera MainCamera => Camera.main;
        public EnemyScriptableObject EnemyScriptableObject { get; private set; }
        private IController Controller { get; set; }
        public GameObject GameObject => gameObject;
        public GameObject PlayerInstance { get; private set; }
        private Player.IPlayer _player;
        public int EnemyIdNumber { get; private set; }  
        public EventManager EventManager { get; private set; } 
        public StateContext Context { get; private set; }   //状態遷移のコンテキスト
        public List<GameObject> CloseEnemies { get; } = new();  //近くにいる敵のリスト
        public Vector3 SpawnPositionInScreen { get; private set; }  //スクリーン座標における生成位置
        public Vector3 SpawnPositionInWorld { get; private set; }   //ワールド座標における生成位置
        public HitPoint HitPoint { get; private set; }
        private AttackParameter AttackParameter { get; set; }
        public Description Description { get; private set; }
        public MovementParameter MovementParameter { get; private set; }
        public bool IsTouchingPlayer{ get; private set; }   //プレイヤーに触れているかどうか
        
        //ReSharper disable once UnusedMethodReturnValue.Global
        public static IEnemy Init(
            GameObject prefab,
            EnemyScriptableObject enemyScriptableObject,
            Vector3 spawnPositionInScreen,
            Vector3 spawnPositionInWorld,
            int enemyIdNumber,
            DiContainer container
        )
        {
            var enemy = container.InstantiatePrefab(prefab).GetComponent<IEnemy>();
            //タグが違う場合は変更
            if (!enemy.GameObject.CompareTag(EnemyTag))
            {
                enemy.GameObject.tag = EnemyTag;
            }
            enemy.SetEnemyScriptableObject(enemyScriptableObject);
            enemy.SetSpawnPositionInScreen(spawnPositionInScreen);
            enemy.SetTransformPosition(spawnPositionInWorld);
            enemy.SetSpawnPositionInWorld(spawnPositionInWorld);
            enemy.SetEnemyIdNumber(enemyIdNumber);
            return enemy;
        }
        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            Context?.Update();
            
            const int countInterval = 600;
            //10秒ごとに近い敵のリストを更新
            if (Time.frameCount % countInterval == 0){
                CloseEnemies.RemoveAll(item => item == null);
            }
        }

        protected virtual void FixedUpdate()
        {
            var newState = Controller.CheckTransitions();
            if (newState == Context.PreviousState.State) return;

            Context.ChangeState(newState);
            Context?.FixedUpdate();
            
            CausePlayerToDamage();
            Debug.Log($"Is touching player: {IsTouchingPlayer}");
        }

        //生成時に呼ばれる
        protected virtual void Initialize()
        {
            IsTouchingPlayer = false;
            _damageTimer = 0.0f;
            FindPlayer();
            InitializeClasses();
            SetController(EnemyScriptableObject.CreateControllerInstance(this));
            EventManager = CreateEnemyEventManager();
            _globalEnemyEventManager.RegisterEnemyEventManager(EventManager, this);

            //初期状態を設定
            var initialState = Controller.GetInitialState();
            Context.ChangeState(initialState);
        }
        
        //パラメータクラスを初期化
        public void InitializeClasses()
        {
            MovementParameter = new MovementParameter(
                EnemyScriptableObject,
                PlayerInstance.transform,
                GetComponent<Rigidbody>()
            );
            Context = new StateContext();
            HitPoint = new HitPoint(EnemyScriptableObject.MaxHitPoint);
            AttackParameter = new AttackParameter(EnemyScriptableObject.AttackPower);
            Description = new Description(
                EnemyScriptableObject.EnemyName, 
                EnemyScriptableObject.Description, 
                EnemyScriptableObject.Evaluation)
                ;
        }

        private void FindPlayer()
        {
            //Playerのインスタンスを取得
            PlayerInstance = GameObject.FindWithTag("Player");
            if (PlayerInstance == null)
            {
                Debug.LogError("Player not found!");
            }
            _player = PlayerInstance.GetComponent<Player.IPlayer>();
        }

        private void CausePlayerToDamage()
        {
            // 無敵判定のタイマーを常に更新
            _damageTimer += Time.deltaTime;
            // Playerに触れているかつ、ダメージインターバルが経過していればダメージを与える
            if (!IsTouchingPlayer || _damageTimer <= DamageInterval) return;
            //Debug.Log("Dealing damage to player");
            _player.DamageController.ReceiveDamage(AttackParameter.AttackPower);
            _damageTimer = 0.0f; // ダメージタイマーリセット
        }

        //enemyタグの付いたオブジェクトのisTriggerのコライダーに侵入したらcloseEnemiesに追加し、出ていったら削除
        //playerタグの付いたオブジェクトに侵入したらダメージを与える状態になる
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                CloseEnemies.Add(other.gameObject);
            }
            if(other.CompareTag(PlayerTag))
            {
                IsTouchingPlayer = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(EnemyTag))
            {
                CloseEnemies.Remove(other.gameObject);
            }
            if(other.CompareTag(PlayerTag))
            {
                IsTouchingPlayer = false;
            }
        }
        //プロパティのセッター
        public void SetIsTouchingPlayer(bool isTouchingPlayer)
        {
            IsTouchingPlayer = isTouchingPlayer;
        }
        
        public void SetEnemyScriptableObject(EnemyScriptableObject enemyScriptableObject)
        {
            EnemyScriptableObject = enemyScriptableObject;
        }

        public void SetController(IController controller)
        {
            Controller = controller;
        }
        public void SetSpawnPositionInScreen(Vector3 spawnPositionInScreen)
        {
            SpawnPositionInScreen = spawnPositionInScreen;
        }

        public void SetTransformPosition(Vector3 position)
        {
            transform.position = position;
        }
        public void SetSpawnPositionInWorld(Vector3 spawnPositionInWorld)
        {
            SpawnPositionInWorld = spawnPositionInWorld;
        }
        public void SetEnemyIdNumber(int enemyIdNumber)
        {
            EnemyIdNumber = enemyIdNumber;
        }

        public void Die()
        {
            //collisionを無効にして、死亡時の処理を実行
            enemyCollider.enabled = false;
            OnDie();
            EventManager.OnDeath.OnNext(Unit.Default);
            if (EnemyScriptableObject.BossFlag)
            {
                EventManager.OnBossDeath.OnNext(Unit.Default);
            }
            SetIsTouchingPlayer(false);
            DestroySelf();
        }
        //派生先でオーバーライドする死亡時の処理
        protected virtual void OnDie(){ }
        
        public void DestroySelf()
        {
            enemyCollider.enabled = false;
            Destroy(gameObject);
        }
        private static EventManager CreateEnemyEventManager()
        {
            return new EventManager();
        }
    }
}