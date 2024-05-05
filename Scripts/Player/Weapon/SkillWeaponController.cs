using DG.Tweening;
using Player.Util;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Weapon
{
    public class SkillWeaponController : MonoBehaviour
    {
        private IPlayer _player;
        private EventManager eventManager;

        [SerializeField]
        private GameObject weaponPrefab;
        private GameObject _weaponInstance;
        private Weapon _weapon;

        private Vector3 _direction;

        private Tweener _tweener;

        private const float Radius = 1.3f;
        private const float PosX = 1.0f;
        private const float PosZ = 1.0f;

        private void Awake()
        {
            _player = GetComponent<IPlayer>();
            _direction = new Vector3(PosX, 0, PosZ);
            SetUniRx();
        }

        private void Start()
        {
            SetTween();
        }

        private void SetUniRx()
        {
            eventManager = _player.EventManager;

            eventManager.OnInitializeData
                .Subscribe(_ => { InstantiateWeapon(); }).AddTo(this);
            eventManager.OnSkillFieldInstantiatePosition
                .Subscribe(UpdateTransform).AddTo(this);
            eventManager.OnSkillAble
                .Subscribe(_ => { SetTween(); }).AddTo(this);
        }

        private void InstantiateWeapon()
        {
            if(_weaponInstance != null)
            {
                Destroy(_weaponInstance);
                _weaponInstance = null;
            }
            var parent = transform;
            var position = parent.position+_direction*Radius;
            _weaponInstance = Instantiate(
                weaponPrefab,
                position,
                Quaternion.identity,
                parent
            );
            _weapon = _weaponInstance.GetComponent<Weapon>();
        }

        private void UpdateTransform(Vector3 newPosition)
        {
            //このコンポーネントのついたオブジェクトの位置とnewPositionの位置を結ぶベクトルを求めて正規化
            var parent = transform;
            var position1 = parent.position;
            var direction = (newPosition - position1).normalized;
            Debug.Log($"direction: {direction}");
            var position = direction * Radius;
            
            _weapon.UpdateTransform(position);
            
            const float second = 0.3f;
            
            Observable.Timer(System.TimeSpan.FromSeconds(second))
                .Subscribe(_ =>
                {
                    _weapon.ScaleTo(0f, second);
                })
                .AddTo(this); // 忘れずにDisposeするためにAddToを使う
        }
        
        private void SetTween()
        {
            _weapon.ScaleTo(1f,0.5f);
        }
    }
}
