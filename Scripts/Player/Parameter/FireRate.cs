using System;
using UniRx;

namespace Player.Parameter
{
    public class FireRate:IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        public IReadOnlyReactiveProperty<float> FireRateValue { get; }
        public FireRate(WeaponParameter weaponParameter)
        {
            var fireRateProperty = new ReactiveProperty<float>();
            FireRateValue = fireRateProperty;
            
            weaponParameter.FireRateValue
                .Select(CalculateFireRate)
                .Subscribe(fireRateValue => 
                    fireRateProperty.Value = fireRateValue)
                .AddTo(_disposables);
        }
        
        private static float CalculateFireRate(int fireRate)
        {
            return fireRate * 0.01f;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}