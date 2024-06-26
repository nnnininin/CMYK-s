using System;
using UniRx;

namespace Player.Parameter
{
    public class ReloadTime: IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        public IReadOnlyReactiveProperty<float> ReloadTimeValue { get; }
        public ReloadTime(WeaponParameter weaponParameter)
        {
            var reloadTimeProperty = new ReactiveProperty<float>();
            ReloadTimeValue = reloadTimeProperty;
            
            weaponParameter.ReloadTimeValue
                .Select(CalculateReloadTime)
                .Subscribe(reloadTimeValue => 
                    reloadTimeProperty.Value = reloadTimeValue)
                .AddTo(_disposables);
        }
        
        //インスペクタ上で整数値で設定したものを小数値に変換する
        private static float CalculateReloadTime(int reloadTime)
        {
            return reloadTime * 0.1f;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}