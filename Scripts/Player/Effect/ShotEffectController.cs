using Player.Util;
using UniRx;
using UnityEngine;

namespace Player.Effect
{
    public class ShotEffectController : MonoBehaviour
    {
        private IPlayer _player;

        private EventManager eventManager;

        private GameObject ShotEffectPrefab => _player.WeaponParameter.ShotEffectPrefab.Value;
        private GameObject _shotEffectInstance;
        
        private ShotEffect _shotEffect;
        
        private const float LocalPosX = -0.1f;
        private const float LocalPosY = -0.9f;
        private const float LocalPosZ = 0.1f;

        private void Awake()
        {
            _player = GetComponent<IPlayer>();
            SetUniRx();
        }
        
        private void SetUniRx()
        {
            eventManager = _player.EventManager;

            eventManager.OnInitializeData
                .Subscribe(_ => { InstantiateShotEffect(); }).AddTo(this);
            eventManager.OnEntryShotState
                .Subscribe(_ => { DOScaleToOne(); }).AddTo(this);
            eventManager.OnExitShotState
                .Subscribe(_ => { DOScaleToZero(); }).AddTo(this);
            eventManager.OnShotComplete
                .Subscribe(data => { DOScaleMagazinePercentage(data.Percentage); }).AddTo(this); 
        }
        
        private void InstantiateShotEffect()
        {
            if (_shotEffectInstance != null)
            {
                Destroy(_shotEffectInstance);
                _shotEffectInstance = null;
            }
            var parent = transform;
            var position = parent.position + new Vector3(LocalPosX, LocalPosY, LocalPosZ);
            _shotEffectInstance = Instantiate(
                ShotEffectPrefab,
                position,
                Quaternion.identity,
                parent
            );
            _shotEffect = _shotEffectInstance.GetComponent<ShotEffect>();
            _shotEffect.SetLocalScaleZero();
        }
        
        private void DOScaleMagazinePercentage(float percentage)
        {
            _shotEffect.DOScaleMagazinePercentage(percentage);
        }
        private void DOScaleToOne()
        {
            _shotEffect.DOScaleToOne();
        }
        private void DOScaleToZero()
        {
            _shotEffect.DOScaleToZero();
        }
        private void Reload(float reloadTime)
        {
            _shotEffect.Reload(reloadTime);
        }
    }
}
