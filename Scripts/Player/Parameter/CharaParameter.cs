using Player.ScriptableObject;
using UniRx;

namespace Player.Parameter
{
    public class CharaParameter
    {
        public ReactiveProperty<string> Name { get; } = new();
        public ReactiveProperty<string> CharaDescription { get; } = new();
        public ReactiveProperty<int> HitPoint { get; } = new();
        public ReactiveProperty<int> MoveSpeed { get; } = new();
        
        public CharaParameter(CharaSO charaSo)
        {
            Name.Value = charaSo.CharaName;
            CharaDescription.Value = charaSo.CharaDescription;
            HitPoint.Value = charaSo.HitPoint;
            MoveSpeed.Value = charaSo.MoveSpeed;
        }
    }
}