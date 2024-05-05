using System;
using UniRx;

namespace Player.Parameter
{
    public class HitPoint: IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        public IReadOnlyReactiveProperty<int> MaxHitPoint { get; }
        
        private readonly ReactiveProperty<int> _currentHitPoint;
        public IReadOnlyReactiveProperty<int> CurrentHitPoint => _currentHitPoint;
        
        public HitPoint(CharaParameter hitPoint)
        {
            var maxHitPointProperty = new ReactiveProperty<int>();
            MaxHitPoint = maxHitPointProperty;
            
            hitPoint.HitPoint
                .Select(CalculateMaxHitPoint)
                .Subscribe(maxHitPoint => 
                    maxHitPointProperty.Value = maxHitPoint)
                .AddTo(_disposables);
            _currentHitPoint = new ReactiveProperty<int>(MaxHitPoint.Value);
        }
        private static int CalculateMaxHitPoint(int hitPoint)
        {
            return hitPoint;
        }
        public void ReceiveDamage(int damage)
        {
            if(_currentHitPoint.Value - damage < 0)
                _currentHitPoint.Value = 0;
            else
                _currentHitPoint.Value -= damage;
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}