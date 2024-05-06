using System;
using UniRx;

namespace Player.Parameter
{
    public class MoveSpeed: IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        public IReadOnlyReactiveProperty<float> MoveSpeedValue { get; }
        public IReadOnlyReactiveProperty<float> MoveSpeedValueLow { get; }
        public bool IsMoveLow { get; private set; }
        public MoveSpeed(CharaParameter level)
        {
            var moveSpeedProperty = new ReactiveProperty<float>();
            MoveSpeedValue = moveSpeedProperty;
            var moveSpeedPropertyLow = new ReactiveProperty<float>();
            MoveSpeedValueLow = moveSpeedPropertyLow;
            
            level.MoveSpeed
                .Select(CalculateMoveSpeed)
                .Subscribe(moveSpeedValue => 
                    moveSpeedProperty.Value = moveSpeedValue)
                .AddTo(_disposables);
            
            level.MoveSpeed
                .Select(CalculateMoveSpeedLow)
                .Subscribe(moveSpeedValueLow => 
                    moveSpeedPropertyLow.Value = moveSpeedValueLow)
                .AddTo(_disposables);
        }
        
        private static float CalculateMoveSpeed(int moveSpeed)
        {
            return moveSpeed;
        }
        
        private static float CalculateMoveSpeedLow(int moveSpeed)
        {
            return moveSpeed * 0.7f;
        }
        
        public void SetIsMoveLow(bool isMoveLow)
        {
            IsMoveLow = isMoveLow;
        }
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}