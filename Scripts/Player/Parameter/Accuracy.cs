using UniRx;
using System;

namespace Player.Parameter
{
    public class Accuracy : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public IReadOnlyReactiveProperty<float> AccuracyValue { get; private set; }

        public Accuracy(WeaponParameter weaponParameter)
        {
            var accuracyProperty = new ReactiveProperty<float>();
            AccuracyValue = accuracyProperty;

            weaponParameter.AccuracyValue
                .Select(CalculateAccuracy)
                .Subscribe(accuracyValue => 
                    accuracyProperty.Value = accuracyValue)
                .AddTo(_disposables);
        }
        private static float CalculateAccuracy(int accuracy)
        {
            const float maxAccuracy = 3.0f;
            return maxAccuracy - accuracy * 0.1f;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}