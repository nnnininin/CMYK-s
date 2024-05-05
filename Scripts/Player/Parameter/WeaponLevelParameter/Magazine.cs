using UniRx;

namespace Player.Parameter
{
    public class Magazine
    {
        private readonly ReactiveProperty<int> maxBullet = new();
        public IReadOnlyReactiveProperty<int> MaxBullet => maxBullet;
        private readonly ReactiveProperty<int> currentBullet;
        public IReadOnlyReactiveProperty<int> CurrentBullet => currentBullet;

        public Magazine(WeaponParameter weaponParameter)
        {
            weaponParameter.Magazine
                .Select(CalculateMaxBullet)
                .Subscribe(maxBulletValue =>
                {
                    maxBullet.Value = maxBulletValue;
                    // currentBulletがmaxBulletを超えている場合は、maxBulletに設定
                    if (currentBullet != null && currentBullet.Value > maxBullet.Value)
                    {
                        currentBullet.Value = maxBullet.Value;
                    }
                });

            currentBullet = new ReactiveProperty<int>(maxBullet.Value);
            maxBullet.Subscribe(_ => {
                // Subscribe内で再度チェックする必要はないが、念のためcurrentBulletの初期設定のロジックを維持
                currentBullet.Value = maxBullet.Value;
            });
        }

        private static int CalculateMaxBullet(int magazineValue)
        {
            return magazineValue; 
        }

        public void Reload()
        {
            currentBullet.Value = MaxBullet.Value;
        }

        public void DecreaseBullet()
        {
            if (currentBullet.Value > 0)
            {
                currentBullet.Value--;
            }
        }
    }
}