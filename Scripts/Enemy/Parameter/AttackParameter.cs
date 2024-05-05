namespace Enemy.Parameter
{
    public class AttackParameter
    {
        public int AttackPower { get; private set; }
        
        public AttackParameter(int attackPower)
        {
            AttackPower = attackPower;
        }
    }
}