using DG.Tweening;
using Player.Util;
using UniRx;
using UnityEngine;

namespace Player.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        private IPlayer _player;
        private EventManager eventManager;

        private GameObject WeaponPrefab => _player.WeaponParameter.WeaponPrefab.Value;
        private GameObject _weaponInstance;
        private Weapon _weapon;

        private Vector3 _direction;

        private Tweener _tweener;

        private const float Radius = 1.3f;
        private const float PosX = 0.0f;
        private const float PosZ = -1.0f;

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
            
            eventManager.OnShotComplete
                .Subscribe(data => { UpdateTransform(data.Position); }).AddTo(this);

            eventManager.OnEntryShotState
                .Subscribe(_ => { KillTween(); }).AddTo(this);

            eventManager.OnExitShotState
                .Subscribe(_ => { SetTween(); }).AddTo(this);
            
            eventManager.OnEntryReloadState
                .Subscribe(Reload).AddTo(this);
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
                WeaponPrefab,
                position,
                Quaternion.identity,
                parent
            );
            _weapon = _weaponInstance.GetComponent<Weapon>();
        }

        private void UpdateTransform(Vector3 newPosition)
        {
            _weapon.UpdateTransform(newPosition);
        }
        
        private void SetTween()
        {
            _weapon.ShotExitTween();
        }

        private void KillTween()
        {
            _weapon.ShotEntryTween();
        }
        
        private void Reload(float reloadTime)
        {
            _weapon.Reload(reloadTime);
        }
    }
}
