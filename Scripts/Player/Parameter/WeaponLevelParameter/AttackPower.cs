using UniRx;

namespace Player.Parameter
{
    public class AttackPower
    {
        public IReadOnlyReactiveProperty<int> AttackPowerValue { get; }

        public AttackPower(WeaponParameter weaponParameter)
        {
            // AttackPowerValueを直接weaponParameter.AttackPowerValueにバインド
            AttackPowerValue = weaponParameter.AttackPower
                .Select(CalculateAttackPower)
                .ToReactiveProperty();
        }
        
        private static int CalculateAttackPower(int baseAttackPower)
        {
            return baseAttackPower;
        }
    }
}