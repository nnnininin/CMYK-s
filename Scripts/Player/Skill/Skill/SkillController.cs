using Player.ScriptableObject;
using Player.Util;
using UniRx;
using UnityEngine;
using Util.RayCaster;

namespace Player.Skill.Skill
{
    public class SkillController : MonoBehaviour
    {
        private IPlayer _player;
        private EventManager _eventManager;
        [SerializeField] 
        private AudioSource _audioSource;
        
        // ゲッターをリアクティブプロパティのValueへのアクセスに変更
        private GameObject SkillPrefab => _player.SkillParameter.SkillPrefab.Value;
        private GameObject DamageZonePrefab => _player.SkillParameter.DamageZonePrefab.Value;

        private int Damage => _player.SkillParameter.Attack.Value;
        private float LifeTime => _player.SkillParameter.LifeTime.Value;
        private float MaxScale => _player.SkillParameter.MaxScale.Value;
        private SkillEffectType SkillEffectType => _player.SkillParameter.SkillEffectType.Value;
        
        private RayCasterFromScreen _rayCasterFromScreen;
        private IFieldActivator _fieldActivator;
        
        private void Awake()
        {
            _player = GetComponent<IPlayer>();
            _rayCasterFromScreen = new RayCasterFromScreen();
        }
        
        private void Start()
        {
            if (_player.SkillParameter == null)
            {
                Debug.LogError("SkillParameter is not assigned.");
                return;
            }

            SetUniRx();
        }
        
        private void SetUniRx()
        {
            _eventManager = _player.EventManager;
            _eventManager.OnEntrySkillState.Subscribe(_ => InstantiateSkillField()).AddTo(this);
        }

        private void InstantiateSkillField()
        {
            Debug.Log("SpawnSkill");
            if (DamageZonePrefab == null)
            {
                Debug.LogError("DamageZonePrefab is not assigned.");
                return;
            }
            
            var hitInfo = _rayCasterFromScreen.GetRayCastHit(UnityEngine.Input.mousePosition, Color.magenta);
            if(hitInfo == null) return;
            var instantiatePosition = hitInfo.Value.point;
            
            _eventManager.TriggerOnSkillFieldInstantiatePosition(instantiatePosition);
            
            InstantiateSkillField(instantiatePosition);
        }
        
        private void InstantiateSkillField(Vector3 targetPosition)
        {
            const float spawnMargin = 1.0f;
            var spawnPosition = transform.position;
            var direction = (targetPosition - spawnPosition).normalized;
            var marginVector = direction * spawnMargin;
            spawnPosition += marginVector;
            
            var skillGameObject = 
                Instantiate(
                    SkillPrefab,
                    spawnPosition,
                    Quaternion.identity,
                    gameObject.transform
                    );
            var skill = skillGameObject.GetComponent<Skill>();
            //効果音再生
            _audioSource.Play();
            skill.SetSkillTargetPosition(spawnPosition, targetPosition);
            skill.OnComplete.Subscribe(OnActivateField).AddTo(this);
        }
        
        private void OnActivateField(Vector3 fieldPosition)
        {
            Debug.Log($"_player.maxScale: {_player.SkillParameter.MaxScale.Value}");
            Debug.Log($"DamageZonePrefab: {DamageZonePrefab}"+
                      $"Damage: {Damage}"+
                      $"MaxScale: {MaxScale}"+
                      $"LifeTime: {LifeTime}"+
                      $"SkillEffectType: {SkillEffectType}"
                      );
            _fieldActivator ??= new SteadyFieldActivator();
            _fieldActivator.ActivatePrefab(
                DamageZonePrefab,
                fieldPosition,
                Damage,
                MaxScale,
                LifeTime,
                null,
                1,
                SkillEffectType
            );
        }
    }
}