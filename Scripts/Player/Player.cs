using Player.Bullet;
using Player.Effect;
using Player.Input;
using Player.Parameter;
using Player.ScriptableObject;
using Player.Skill.Skill;
using Player.State;
using Player.Util;
using Player.Weapon;
using Scene;
using UnityEngine;
using Zenject;
using UniRx;

namespace Player
{
    [RequireComponent(typeof(ButtonPressedInput))]
    [RequireComponent(typeof(ButtonHoldInput))]
    [RequireComponent(typeof(BulletController))]
    [RequireComponent(typeof(SkillController))]
    [RequireComponent(typeof(ShotEffectController))]
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MoveController))]
    public class Player : MonoBehaviour, IPlayer
    {
        [Inject] private GlobalPlayerEventManager _globalPlayerEventManager;
        
        [SerializeField]
        private Transform childTransform;
        public Transform ChildTransform => childTransform;

        //Rigidbodyを使用するためのプロパティ
        private Rigidbody _rigidbody;
        
        //EventManagerを使用するためのプロパティ
        public EventManager EventManager { get; } = new();

        //Stateパターン関連プロパティ
        public StateContext Context { get; private set; }
        public bool IsActionInputEnabled { get; private set; }  //actionの入力が有効かどうか
        public ButtonPressedAction ButtonPressedAction { get; private set; } = ButtonPressedAction.None;
        public bool IsShotInput { get; private set; }   //ショットボタンが押されているかどうか
        
        public Vector2 InputMoveDirection { get; private set; }  //移動方向の入力値

        //各種ステータス
        public CharaParameter CharaParameter { get; private set; }
        public WeaponParameter WeaponParameter { get; private set; }
        public SkillParameter SkillParameter { get; private set; }
        public AutoSkillParameter AutoSkillParameter { get; private set; }

        public Vector2 MouseInputDirection { get; private set; }  
        public AttackPower AttackPower { get; private set; }
        public BulletSpeed BulletSpeed { get; private set; }
        public Accuracy Accuracy { get; private set; }
        public FireRate FireRate { get; private set; }
        public Descriptions Descriptions { get; private set; }
        public HitPoint HitPoint { get; private set; }
        public Magazine Magazine { get; private set; }
        public ReloadTime ReloadTime { get; private set; }
        public MoveSpeed MoveSpeed { get; private set; }
        
        public Animator Animator { get; private set; }
        
        //shotのhash値を取得
        public int RunHash { get; } = Animator.StringToHash("Run");

        //各種コントローラー
        public DamageController DamageController { get; private set; }

        //Stateパターンを使用するために
        //UnityのUpdateとFixedUpdate内でcontextのUpdateとFixedUpdateを呼び出す
        private void Update() => Context.Update();
        private void FixedUpdate() => Context.FixedUpdate();

        public static IPlayer Init(
            TemporaryGameDataSo temporaryGameData,
            Vector3 hitPosition,
            DiContainer container
            )
        {
            var prefab = temporaryGameData.PlayerPrefab.Value;
            var player = container.InstantiatePrefab(prefab).GetComponent<Player>();
            player.SetTransform(hitPosition);
            player.GetRigidBody();
            player.InitializeAnimator();
            player.InitializeGameData(temporaryGameData);
            player.InitializeController();
            player.InitializeContext();
            player.SetUniRx(temporaryGameData);
            return player;
        }

        public void Awake()
        {
            _globalPlayerEventManager.RegisterPlayerEventManager(EventManager, this);
        }
        
        private void SetUniRx(TemporaryGameDataSo temporaryGameDataSo)
        {
            temporaryGameDataSo.WeaponLevel
                .Subscribe(_ =>
                {
                    Debug.Log("WeaponLevel changed");
                    WeaponParameter.SetLevel(
                        temporaryGameDataSo.WeaponLevel.Value[0],
                        temporaryGameDataSo.WeaponLevel.Value[1],
                        temporaryGameDataSo.WeaponLevel.Value[2]
                    );
                })
                .AddTo(this);

            temporaryGameDataSo.SkillLevel
                .Subscribe(_ =>
                {
                    Debug.Log("SkillLevel changed");
                    SkillParameter.SetLevel(
                        temporaryGameDataSo.SkillLevel.Value[0],
                        temporaryGameDataSo.SkillLevel.Value[1],
                        temporaryGameDataSo.SkillLevel.Value[2]
                    );
                })
                .AddTo(this);

            temporaryGameDataSo.AutoSkillLevel
                .Subscribe(_ =>
                {
                    Debug.Log("AutoSkillLevel changed");
                    AutoSkillParameter.SetLevel(
                        temporaryGameDataSo.AutoSkillLevel.Value[0],
                        temporaryGameDataSo.AutoSkillLevel.Value[1],
                        temporaryGameDataSo.AutoSkillLevel.Value[2]
                    );
                })
                .AddTo(this);
            temporaryGameDataSo.WeaponSO
                .Subscribe(weaponSO =>
                {
                    Debug.Log("WeaponSO changed");
                    WeaponParameter.SetSO(weaponSO);
                })
                .AddTo(this);
            temporaryGameDataSo.SkillSO
                .Subscribe(skillSO =>
                {
                    Debug.Log("SkillSO changed");
                    SkillParameter.SetSO(skillSO);
                })
                .AddTo(this);
            temporaryGameDataSo.AutoSkillSO
                .Subscribe(autoSkillSO =>
                {
                    Debug.Log("AutoSkillSO changed");
                    AutoSkillParameter.SetSkillSO(autoSkillSO);
                })
                .AddTo(this);
        }
        
        private void SetTransform(Vector3 hitPosition)
        {
            transform.localPosition = hitPosition;
            const int rotateY = 135;
            childTransform.rotation = Quaternion.Euler(0, rotateY, 0);
        }
        
        private void GetRigidBody()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void InitializeAnimator()
        {
            Animator = GetComponentInChildren<Animator>();
            SetRunAnimation(false);
        }
        
        public void SetRunAnimation(bool isRun)
        {
            //既にisRunが同じ値だったら何もしない
            if (Animator.GetBool(RunHash) == isRun)
                return;
            Animator.SetBool(RunHash, isRun);
        }
        
        private void InitializeGameData(TemporaryGameDataSo temporaryGameDataSo)
        {
            InitializeChara(temporaryGameDataSo.CharaSO.Value);
            InitializeWeapon(temporaryGameDataSo.WeaponSO.Value);
            InitializeSkill(temporaryGameDataSo.SkillSO.Value);
            InitializeAutoSkill(temporaryGameDataSo.AutoSkillSO.Value);
            EventManager.TriggerOnInitializeData();
        }

        private void InitializeController()
        {
            DamageController = new DamageController(this);
        }

        private void InitializeChara(CharaSO charaSO)
        {
            CharaParameter = new CharaParameter(charaSO);
            Descriptions = new Descriptions(CharaParameter);
            HitPoint = new HitPoint(CharaParameter);
            MoveSpeed = new MoveSpeed(CharaParameter);
        }
        
        private void InitializeWeapon(WeaponSO weaponSO)
        {
            WeaponParameter = new WeaponParameter(weaponSO);
            AttackPower = new AttackPower(WeaponParameter);
            BulletSpeed = new BulletSpeed(WeaponParameter);
            Accuracy = new Accuracy(WeaponParameter);
            FireRate = new FireRate(WeaponParameter);
            Magazine = new Magazine(WeaponParameter);
            ReloadTime = new ReloadTime(WeaponParameter);
        }

        private void InitializeSkill(SkillSO skillSO)
        {
            SkillParameter = new SkillParameter(skillSO);
        }
        
        private void InitializeAutoSkill(AutoSkillSO autoSkillSO)
        {
            AutoSkillParameter = new AutoSkillParameter(autoSkillSO);
        }
        
        private void InitializeContext()
        {
            Debug.Log("Player: InitializeContext");
            Context = new StateContext();
            Context.Init(this, State.IState.State.Idle);
            IsActionInputEnabled = true;
        }

        //以下setter
        public void SetButtonPressedAction(ButtonPressedAction action)
        {
            ButtonPressedAction = action;
        }

        public void SetIsActionInputEnabled(bool isActionInputEnabled)
        {
            IsActionInputEnabled = isActionInputEnabled;
        }
        public void SetIsShotInput(bool isShotInput)
        {
            IsShotInput = isShotInput;
        }
        public void SetInputMoveDirection(Vector2 inputMoveDirection)
        {
            InputMoveDirection = inputMoveDirection;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
        }
        
        public void SetMouseInputDirection(Vector2 mouseInputDirection)
        {
            MouseInputDirection = mouseInputDirection;
        }

        //以下getter
        public GameObject GetPlayerInstance()
        {
            return gameObject;
        }
    }

    //ボタンを1回押下するタイプのアクションのenum
    public enum ButtonPressedAction
    {
        None,
        Idle,
        Skill,
        Reload
    }
}
